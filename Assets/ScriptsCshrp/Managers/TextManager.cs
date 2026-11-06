using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 读取文本表，保存并按编号查询文字。
/// </summary>
public class TextManager : MonoBehaviour
{
    // ==================== 字段 ====================

    // _resourceManager：用于读取文本资源的资源管理器。
    private ResourceManager _resourceManager;

    // _texts：保存文本编号与对应文字的字典。
    private Dictionary<int, string> _texts = new Dictionary<int, string>();

    // ==================== Unity 生命周期 ====================

    // ==================== 初始化 ====================

    /// <summary>
    /// 保存资源管理器引用，并加载文本表。
    /// </summary>
    /// <param name="resourceManager">用于读取文本资源的资源管理器。</param>
    public void Initialize(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
        LoadTexts();
    }

    // ==================== 对外接口 ====================

    /// <summary>
    /// 根据文本编号查询文字。
    /// </summary>
    /// <param name="textId">需要查询的文本编号。</param>
    /// <returns>找到的文字，未找到时返回空字符串。</returns>
    public string GetText(int textId)
    {
        // text：根据编号查找到的文字。
        if (_texts.TryGetValue(textId, out string text))
        {
            return text;
        }

        Debug.LogError("未找到文本 ID：" + textId);
        return "";
    }

    // ==================== 核心逻辑 ====================

    /// <summary>
    /// 加载 CSV，检查各行并保存编号与文字。
    /// </summary>
    private void LoadTexts()
    {
        // 第一步：读取文本资源。
        // textAsset：加载到的 CSV 文本资源。
        TextAsset textAsset = _resourceManager.LoadTextAsset("Data/文本表");

        if (textAsset != null)
        {
            // 第二步：加载成功后清除旧记录，再按换行符拆分记录。
            _texts.Clear();

            // lines：保存文本表的每一行。
            string[] lines = textAsset.text.Split('\n');    // Split('\n')：遇到换行符就拆分为多行。

            // 第三步：跳过表头，检查列数、编号和重复项，保存有效记录。
            // i：当前读取的行下标。
            for (int i = 1; i < lines.Length; i++)
            {
                // line：去掉末尾回车符的当前行。
                string line = lines[i].TrimEnd('\r');   // TrimEnd('\r')：去掉行末回车符，文字内容不变。

                // 当前行为空白时，跳过并读取下一行。
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                // columns：当前行拆分出的编号列与文字列。
                string[] columns = ParseCsvline(line);
                if (columns.Length != 2)
                {
                    Debug.LogError("文本行格式错误，应有 ID 和 Text 两列：" + line);
                    continue;
                }

                // textId：从编号列转换得到的整数编号。
                if (!int.TryParse(columns[0], out int textId))
                {
                    Debug.LogError("文本 ID 不是有效整数：" + columns[0]);
                    continue;
                }

                if (_texts.ContainsKey(textId))
                {
                    Debug.LogError("文本 ID 重复：" + textId);
                    continue;
                }

                _texts.Add(textId, columns[1]);
            }

            // 第四步：输出成功保存的文本数量。
            Debug.Log("已加载文本数量：" + _texts.Count);
        }
    }

    // ==================== 事件响应 ====================

    // ==================== 内部辅助 ====================

    /// <summary>
    /// 拆分支持引号内逗号和转义双引号的单行 CSV 记录。
    /// </summary>
    /// <param name="line">需要拆分的一条 CSV 记录。</param>
    /// <returns>字段数组；引号未闭合时返回空数组。</returns>
    private string[] ParseCsvline(string line)
    {
        // insideQuotes：当前是否位于双引号包围的文字中。
        bool insideQuotes = false;

        // fields：保存已经拆分完成的各列。
        List<string> fields = new List<string>();

        // currentField：正在拼接的当前列内容。
        string currentField = "";

        // 第一步：逐个检查字符，拆分各列。
        // i：当前检查的字符下标。
        for (int i = 0; i < line.Length; i++)
        {
            // currentChar：本次检查的单个字符。
            char currentChar = line[i];

            if (currentChar == '"')
            {
                // 引号内部连续两个双引号表示一个文字双引号，保存后跳过第二个。
                if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentField += '"';
                    i++;
                }
                else
                {
                    insideQuotes = !insideQuotes;
                }
            }
            else if (currentChar == ',' && !insideQuotes)
            {
                fields.Add(currentField);
                currentField = "";
            }
            else
            {
                currentField += currentChar;
            }
        }

        // 第二步：检查包围文字的引号是否闭合。
        if (insideQuotes)
        {
            return new string[0];
        }

        // 第三步：保存末尾没有逗号分隔的最后一列，返回全部字段。
        fields.Add(currentField);
        return fields.ToArray();
    }

    // ==================== 调试显示 ====================
}
