using UnityEditor;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

public class TextTableExporter
{
    // ==================== 字段 ====================
    
    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    // ==================== 对外接口 ====================

    /// <summary>
    /// 定位文本源表并检查文件是否存在。
    /// </summary>
    [MenuItem("工具/导出文本表")]
    private static void ExportTextTable()
    {
        // projectRoot：工程根目录，用来定位 Docs 文件夹。
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;

        // sourcePath：Excel 文本源表的完整路径。
        string sourcePath = Path.Combine(projectRoot, "Docs", "DataTables", "文本表.xlsx");
        
        if (!File.Exists(sourcePath))
        {
            Debug.LogError($"找不到文本源表：{sourcePath}");
            return;
        }

        // workbook：以只读方式打开的 Excel 压缩包。
        using (ZipArchive workbook = ZipFile.OpenRead(sourcePath))
        {
            // workbookEntry：压缩包里保存工作表清单的文件条目。
            ZipArchiveEntry workbookEntry = workbook.GetEntry("xl/workbook.xml");

            if (workbookEntry == null)
            {
                Debug.LogError($"工作簿内缺少 xl/workbook.xml：{sourcePath}");
                return;
            }

            // workbookStream：用来读取清单内容的文件流。
            using (Stream workbookStream = workbookEntry.Open())
            {
                // workbookXml：从清单读出的 XML 文档。
                XDocument workbookXml =XDocument.Load(workbookStream);
                
                // xmlNamespace：工作簿里的 XML 命名空间，用来定位 sheet 元素。
                XNamespace xmlNamespace = workbookXml.Root.Name.Namespace;

                // sheet：这次循环取到的一条工作表记录。
                foreach (XElement sheet in workbookXml.Descendants(xmlNamespace + "sheet"))
                {
                    // sheetName：当前工作表在 Excel 底部显示的名称。
                    string sheetName = sheet.Attribute("name").Value;
                    Debug.Log(sheetName);
                }
            }
        }
        Debug.Log($"找到文本源表：{sourcePath}");
    }
    
    // ==================== 核心逻辑 ====================

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    // ==================== 调试显示 ====================
    
}
