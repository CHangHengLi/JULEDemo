using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 查询字符串解析器
/// </summary>
public class QueryStringParser
{
    /// <summary>
    /// 解析查询字符串并返回键值对字典
    /// </summary>
    /// <param name="queryString">要解析的查询字符串，格式如："name=value&key=data"</param>
    /// <returns>包含键值对的字典</returns>
    public static Dictionary<string, string> ParseQueryString(string queryString)
    {
        // 创建一个新的字典来存储解析后的键值对
        var result = new Dictionary<string, string>();
        
        // 如果查询字符串为空，则返回空字典
        if (string.IsNullOrWhiteSpace(queryString))
        {
            return result;
        }        
        // 如果查询字符串以问号开头，删除问号
        if (queryString.StartsWith("?"))
        {
            queryString = queryString.Substring(1);
        }
        
        // 按 & 符号分割查询字符串为多个参数
        string[] parameters = queryString.Split('&');
        
        // 遍历每个参数
        foreach (var parameter in parameters)
        {
            // 跳过空参数
            if (string.IsNullOrWhiteSpace(parameter))
            {
                continue;
            }
            
            // 按 = 符号分割每个参数为键和值
            int equalsIndex = parameter.IndexOf('=');
            
            // 处理没有值的参数（如 "param"，而不是 "param=value"）
            if (equalsIndex < 0)
            {
                // 将没有值的参数键添加到字典中，值设为空字符串
                result[Uri.UnescapeDataString(parameter)] = string.Empty;
            }
            else
            {
                // 提取键和值，并进行 URL 解码
                string key = Uri.UnescapeDataString(parameter.Substring(0, equalsIndex));
                string value = equalsIndex < parameter.Length - 1 
                    ? Uri.UnescapeDataString(parameter.Substring(equalsIndex + 1)) 
                    : string.Empty;
                
                // 将键值对添加到字典中
                result[key] = value;
            }
        }
        
        return result;
    }
}
