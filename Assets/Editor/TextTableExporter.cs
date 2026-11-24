using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 将 Excel 文本源表中的分类工作表合并为游戏读取的 CSV。
/// </summary>
public class TextTableExporter
{
    // ==================== 字段 ====================

    // SourcePath：Excel 源表的工程相对路径。
    private const string SourcePath = "Docs/DataTables/文本表.xlsx";

    // OutputAssetPath：游戏加载的 CSV 资源路径，也用于通知编辑器重新导入。
    private const string OutputAssetPath = "Assets/Resources/Data/文本表.csv";

    // WorkbookPartPath：工作簿清单在 Excel 压缩包里的路径。
    private const string WorkbookPartPath = "xl/workbook.xml";

    // WorkbookRelationshipsPath：保存工作表和共享文本表路径的关联清单。
    private const string WorkbookRelationshipsPath = "xl/_rels/workbook.xml.rels";

    // OfficeRelationshipNamespace：工作表 r:id 属性所属的 XML 命名空间。
    private static readonly XNamespace OfficeRelationshipNamespace =
        "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

    // WorkbookPartUri：解析关联目标时使用的包内基准路径。
    private static readonly Uri WorkbookPartUri = new Uri("file:///" + WorkbookPartPath);

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 校验并导出文本表，完成后重新导入 CSV 资源。
    /// </summary>
    [MenuItem("工具/导出文本表")]
    private static void ExportTextTable()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogError("请先停止运行，再导出文本表。");
            return;
        }

        // projectRoot：Assets 的上一级目录，用来定位源表和输出文件。
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;

        // sourcePath：Excel 源表的磁盘路径。
        string sourcePath = Path.Combine(projectRoot, SourcePath);

        // csvPath：生成的 CSV 文件的磁盘路径。
        string csvPath = Path.Combine(projectRoot, OutputAssetPath);

        try
        {
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException($"找不到文本源表：{sourcePath}");
            }

            // 第一步：读完整份工作簿并校验记录。
            // records：按文本 ID 排序的所有有效文案。
            SortedDictionary<int, string> records;

            // sourceStream：打开源表的只读文件流。
            using (FileStream sourceStream = File.Open(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            // workbook：用于读取 Excel 包内文件的对象。
            using (ZipArchive workbook = new ZipArchive(sourceStream, ZipArchiveMode.Read))
            {
                records = ReadWorkbook(workbook);
            }

            if (records.Count == 0)
            {
                throw new InvalidDataException("源表没有可导出的文本记录。");
            }

            // 第二步：校验通过后生成 CSV，并通知编辑器读取新文件。
            WriteCsv(csvPath, records);
            AssetDatabase.ImportAsset(OutputAssetPath);
            Debug.Log($"文本表导出成功，共 {records.Count} 条。");
        }
        catch (Exception error)
        {
            Debug.LogError($"文本表导出失败：{error.Message}");
        }
    }

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 读取所有工作表，汇总通过校验的文本。
    /// </summary>
    /// <param name="workbook">以只读方式打开的 Excel 压缩包。</param>
    /// <returns>按 ID 排序的编号与文案。</returns>
    private static SortedDictionary<int, string> ReadWorkbook(ZipArchive workbook)
    {
        // 第一步：读取工作簿清单、关联清单和共享文本。
        // workbookRoot：列出工作表的 XML 根元素。
        XElement workbookRoot = ReadXmlPart(workbook, WorkbookPartPath);

        // workbookNamespace：用来定位 sheets 和 sheet 元素的命名空间。
        XNamespace workbookNamespace = workbookRoot.Name.Namespace;

        // relationshipsRoot：保存包内关联关系的 XML 根元素。
        XElement relationshipsRoot = ReadXmlPart(workbook, WorkbookRelationshipsPath);

        // relationships：按关联编号保存工作表和共享文本表等目标信息。
        Dictionary<string, XElement> relationships = relationshipsRoot
            .Elements(relationshipsRoot.Name.Namespace + "Relationship")
            .ToDictionary(relationship => (string)relationship.Attribute("Id"), StringComparer.Ordinal);

        // sharedRelationship：指向共享文本表的关联记录，纯内嵌文本的工作簿可能没有它。
        XElement sharedRelationship = relationships.Values.FirstOrDefault(relationship =>
            ((string)relationship.Attribute("Type"))?.EndsWith("/sharedStrings", StringComparison.Ordinal) == true);

        // sharedTexts：按索引排列的共享文案，只读一次，供所有工作表复用。
        List<string> sharedTexts = ReadSharedTexts(workbook, sharedRelationship);

        // sheets：从工作簿清单中找到的工作表列表。
        XElement sheets = workbookRoot.Element(workbookNamespace + "sheets");
        if (sheets == null)
        {
            throw new InvalidDataException("工作簿缺少工作表清单。");
        }

        // 第二步：按关联编号打开每张工作表，合并 ID 和文案。
        // records：所有分类工作表共同写入的文本字典。
        SortedDictionary<int, string> records = new SortedDictionary<int, string>();

        // sheet：本次循环处理的一条工作表记录。
        foreach (XElement sheet in sheets.Elements(workbookNamespace + "sheet"))
        {
            // sheetName：Excel 中显示的工作表名称，用于定位错误。
            string sheetName = (string)sheet.Attribute("name");

            // relationshipId：指向当前工作表数据的关联编号。
            string relationshipId = (string)sheet.Attribute(OfficeRelationshipNamespace + "id");

            // relationship：记录了当前工作表的数据文件位置和类型。
            if (string.IsNullOrEmpty(relationshipId) || !relationships.TryGetValue(relationshipId, out XElement relationship))
            {
                throw new InvalidDataException($"找不到工作表「{sheetName}」的数据文件关联。");
            }

            // relationshipType：区分普通工作表和图表页等其他类型。
            string relationshipType = (string)relationship.Attribute("Type");
            if (relationshipType?.EndsWith("/worksheet", StringComparison.Ordinal) != true)
            {
                continue;
            }

            // worksheetRoot：当前工作表的数据 XML 根元素。
            XElement worksheetRoot = ReadXmlPart(workbook, ResolveWorkbookTarget(relationship));
            ReadWorksheet(worksheetRoot, sheetName, sharedTexts, records);
        }

        return records;
    }

    /// <summary>
    /// 校验一张工作表的 ID 和 Text 列，并把文本加入汇总字典。
    /// </summary>
    /// <param name="worksheetRoot">当前工作表的数据 XML 根元素。</param>
    /// <param name="sheetName">工作表的名称，用于报错定位。</param>
    /// <param name="sharedTexts">按索引保存的 Excel 共享文案。</param>
    /// <param name="records">所有工作表共同写入的文本字典。</param>
    private static void ReadWorksheet(XElement worksheetRoot, string sheetName, List<string> sharedTexts,
        SortedDictionary<int, string> records)
    {
        // worksheetNamespace：当前工作表中行、单元格和值所用的命名空间。
        XNamespace worksheetNamespace = worksheetRoot.Name.Namespace;

        // sheetData：保存工作表各行的 XML 元素。
        XElement sheetData = worksheetRoot.Element(worksheetNamespace + "sheetData");
        if (sheetData == null)
        {
            throw new InvalidDataException($"工作表「{sheetName}」缺少单元格数据区域。");
        }

        // headerRead：是否已经检查过第一行有内容的 ID、Text 表头。
        bool headerRead = false;

        // row：本次循环读取的工作表行。
        foreach (XElement row in sheetData.Elements(worksheetNamespace + "row"))
        {
            // location：错误提示所用的表名和行号。
            string location = $"工作表「{sheetName}」第 {(string)row.Attribute("r") ?? "未知"} 行";

            // idText：本行 A 列的原始编号内容。
            string idText = string.Empty;

            // text：本行 B 列读出的完整文案。
            string text = string.Empty;

            // 第一步：只读取当前行的 A、B 两列，忽略其他列。
            // cell：当前行中正在处理的单元格。
            foreach (XElement cell in row.Elements(worksheetNamespace + "c"))
            {
                // address：单元格地址，用来区分 A 列和 B 列。
                string address = (string)cell.Attribute("r");
                if (string.IsNullOrEmpty(address) || address.Length < 2 || !char.IsDigit(address[1]))
                {
                    continue;
                }

                if (address[0] == 'A')
                {
                    idText = ReadCellText(cell, sharedTexts, location);
                }
                else if (address[0] == 'B')
                {
                    text = ReadCellText(cell, sharedTexts, location);
                }
            }

            // 第二步：跳过空行，并将第一行有内容的记录识别为表头。
            if (string.IsNullOrWhiteSpace(idText) && string.IsNullOrWhiteSpace(text))
            {
                continue;
            }

            if (!headerRead)
            {
                if (!string.Equals(idText.Trim(), "ID", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(text.Trim(), "Text", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException($"{location}的表头应为 A 列 ID、B 列 Text。");
                }

                headerRead = true;
                continue;
            }

            // 第三步：校验编号、文案和重复项，再保存有效记录。
            // id：转换成正整数的文本编号。
            if (!int.TryParse(idText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id) || id <= 0)
            {
                throw new InvalidDataException($"{location}的文本 ID 无效：{idText}");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidDataException($"{location}的文本为空，ID 为 {id}。");
            }

            if (text.IndexOf('\n') >= 0 || text.IndexOf('\r') >= 0)
            {
                throw new InvalidDataException($"{location}的文本含实际换行，ID 为 {id}。");
            }

            if (records.ContainsKey(id))
            {
                throw new InvalidDataException($"{location}的文本 ID {id} 重复。");
            }

            records.Add(id, text);
        }
    }

    /// <summary>
    /// 将校验通过的记录写成 UTF-8 CSV。
    /// </summary>
    /// <param name="csvPath">最终 CSV 的磁盘路径。</param>
    /// <param name="records">所有通过校验的文本。</param>
    private static void WriteCsv(string csvPath, SortedDictionary<int, string> records)
    {
        // 第一步：写一次表头，再逐条拼接记录。
        // csv：正在生成的完整 CSV 内容。
        StringBuilder csv = new StringBuilder("ID,Text");

        // record：本次写入 CSV 的一组编号与文案。
        foreach (KeyValuePair<int, string> record in records)
        {
            // text：当前行的文案，必要时按 CSV 规则处理逗号和双引号。
            string text = record.Value;
            if (text.IndexOf(',') >= 0 || text.IndexOf('"') >= 0)
            {
                text = "\"" + text.Replace("\"", "\"\"") + "\"";
            }

            csv.AppendLine()
                .Append(record.Key.ToString(CultureInfo.InvariantCulture))
                .Append(',')
                .Append(text);
        }

        // 第二步：完整写入临时文件后，再替换游戏读取的 CSV。
        // temporaryPath：与最终 CSV 同目录的临时文件路径。
        string temporaryPath = Path.Combine(Path.GetDirectoryName(csvPath), "." + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            File.WriteAllText(temporaryPath, csv.ToString(), new UTF8Encoding(true));
            if (File.Exists(csvPath))
            {
                File.Replace(temporaryPath, csvPath, null);
            }
            else
            {
                File.Move(temporaryPath, csvPath);
            }
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    /// <summary>
    /// 打开 Excel 压缩包中的 XML 文件并返回根元素。
    /// </summary>
    /// <param name="workbook">以只读方式打开的 Excel 压缩包。</param>
    /// <param name="partPath">XML 文件在压缩包中的路径。</param>
    /// <returns>读到的 XML 根元素。</returns>
    private static XElement ReadXmlPart(ZipArchive workbook, string partPath)
    {
        // entry：要读取的 XML 文件在压缩包中的条目。
        ZipArchiveEntry entry = workbook.GetEntry(partPath);
        if (entry == null)
        {
            throw new InvalidDataException($"工作簿内缺少文件：{partPath}");
        }

        // stream：用于读取包内 XML 的文件流。
        using (Stream stream = entry.Open())
        {
            return XDocument.Load(stream, LoadOptions.PreserveWhitespace).Root;
        }
    }

    /// <summary>
    /// 将关联记录的目标转换成可以在压缩包内查找的路径。
    /// </summary>
    /// <param name="relationship">包含目标路径的关联记录。</param>
    /// <returns>压缩包内的文件路径。</returns>
    private static string ResolveWorkbookTarget(XElement relationship)
    {
        // target：关联记录中的原始目标路径。
        string target = (string)relationship.Attribute("Target");
        if (string.IsNullOrWhiteSpace(target) || (string)relationship.Attribute("TargetMode") == "External")
        {
            throw new InvalidDataException("工作簿的关联记录未指向包内文件。");
        }

        // targetUri：相对于工作簿位置解析出的包内目标。
        Uri targetUri = new Uri(WorkbookPartUri, target);
        if (!targetUri.IsFile || targetUri.Host.Length != 0)
        {
            throw new InvalidDataException($"工作簿的关联路径无效：{target}");
        }

        return Uri.UnescapeDataString(targetUri.AbsolutePath).TrimStart('/');
    }

    /// <summary>
    /// 按存储顺序读取 Excel 的共享文案。
    /// </summary>
    /// <param name="workbook">以只读方式打开的 Excel 压缩包。</param>
    /// <param name="relationship">共享文本表的关联记录。</param>
    /// <returns>以索引位置保存的共享文案。</returns>
    private static List<string> ReadSharedTexts(ZipArchive workbook, XElement relationship)
    {
        if (relationship == null)
        {
            return new List<string>();
        }

        // sharedRoot：共享文本表的 XML 根元素。
        XElement sharedRoot = ReadXmlPart(workbook, ResolveWorkbookTarget(relationship));

        // sharedNamespace：共享文本条目使用的 XML 命名空间。
        XNamespace sharedNamespace = sharedRoot.Name.Namespace;

        return sharedRoot.Elements(sharedNamespace + "si").Select(ReadPlainText).ToList();
    }

    /// <summary>
    /// 取出单元格的值，并将共享文本编号还原成文案。
    /// </summary>
    /// <param name="cell">当前需要读取的单元格。</param>
    /// <param name="sharedTexts">工作簿的共享文案。</param>
    /// <param name="location">单元格所在表和行的说明。</param>
    /// <returns>单元格中的文字或数字原文。</returns>
    private static string ReadCellText(XElement cell, List<string> sharedTexts, string location)
    {
        // cellType：表示单元格是共享文本、内嵌文本、错误值还是普通值。
        string cellType = (string)cell.Attribute("t");

        // cellNamespace：用来定位单元格内容元素的 XML 命名空间。
        XNamespace cellNamespace = cell.Name.Namespace;

        if (cellType == "inlineStr")
        {
            return ReadPlainText(cell.Element(cellNamespace + "is"));
        }

        if (cellType == "e")
        {
            throw new InvalidDataException($"{location}含有 Excel 错误值。");
        }

        // value：数字、普通文字或共享文本索引的原始值。
        string value = (string)cell.Element(cellNamespace + "v") ?? string.Empty;
        if (cellType != "s")
        {
            return value;
        }

        // index：从零开始的共享文本编号。
        if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out int index) ||
            index < 0 || index >= sharedTexts.Count)
        {
            throw new InvalidDataException($"{location}引用了无效的共享文本编号：{value}");
        }

        return sharedTexts[index];
    }

    /// <summary>
    /// 提取一条 Excel 文案，合并因字体等格式而分开的文字片段。
    /// </summary>
    /// <param name="item">共享文本或内嵌文本的 XML 元素。</param>
    /// <returns>拼接后的文案。</returns>
    private static string ReadPlainText(XElement item)
    {
        if (item == null)
        {
            return string.Empty;
        }

        // textNamespace：文本元素所用的 XML 命名空间。
        XNamespace textNamespace = item.Name.Namespace;

        // plainText：未分成多个格式片段时的文本元素。
        XElement plainText = item.Element(textNamespace + "t");
        if (plainText != null)
        {
            return plainText.Value;
        }

        return string.Concat(item.Elements(textNamespace + "r").Select(run => (string)run.Element(textNamespace + "t")));
    }

    // ==================== 调试显示 ====================
}
