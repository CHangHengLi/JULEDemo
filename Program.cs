/*
要编译并运行此程序：

1. 将 `QueryStringParser.cs` 和 `Program.cs` 保存在同一目录中。
2. 打开命令提示符或终端。
3. 导航到保存文件的目录。

4. 编译代码：
   - 使用 .NET 8.0 SDK：
     dotnet build
   - 这将创建一个可执行文件。

5. 运行可执行文件：
   - 使用 .NET CLI: dotnet run
   - 或直接运行生成的可执行文件

示例（使用.NET CLI）：
  cd 文件路径
  dotnet run

预期输出：
程序将打印解析多个示例查询字符串的结果，
显示由QueryStringParser提取的键值对。
*/
using System;
using System.Collections.Generic;

/// <summary>
/// 主程序类
/// </summary>
public class Program
{
    /// <summary>
    /// 程序入口点
    /// </summary>
    /// <param name="args">命令行参数</param>
    public static void Main(string[] args)
    {
        Console.WriteLine("查询字符串解析器演示");
        Console.WriteLine("------------------------");

        // 示例1：基本查询字符串
        string queryString1 = "name=JohnDoe&age=30&city=NewYork";
        Console.WriteLine($"\n正在解析: \"{queryString1}\"");
        Dictionary<string, string> parsed1 = QueryStringParser.ParseQueryString(queryString1);
        PrintDictionary(parsed1);

        // 示例2：带有前导'?'的查询字符串
        string queryString2 = "?product=Laptop&price=1200&available=true";
        Console.WriteLine($"\n正在解析: \"{queryString2}\"");
        Dictionary<string, string> parsed2 = QueryStringParser.ParseQueryString(queryString2);
        PrintDictionary(parsed2);

        // 示例3：包含URL编码字符的查询字符串
        string queryString3 = "search=C%23%20Programming&category=Development";
        Console.WriteLine($"\n正在解析: \"{queryString3}\"");
        Dictionary<string, string> parsed3 = QueryStringParser.ParseQueryString(queryString3);
        PrintDictionary(parsed3);

        // 示例4：包含空值和无值参数的查询字符串
        string queryString4 = "name=Alice&id=&status=active&isAdmin";
        Console.WriteLine($"\n正在解析: \"{queryString4}\"");
        Dictionary<string, string> parsed4 = QueryStringParser.ParseQueryString(queryString4);
        PrintDictionary(parsed4);
        
        // 示例5：空查询字符串
        string queryString5 = "";
        Console.WriteLine($"\n正在解析: \"{queryString5}\"");
        Dictionary<string, string> parsed5 = QueryStringParser.ParseQueryString(queryString5);
        PrintDictionary(parsed5);

        // 示例6：只有'?'的查询字符串
        string queryString6 = "?";
        Console.WriteLine($"\n正在解析: \"{queryString6}\"");
        Dictionary<string, string> parsed6 = QueryStringParser.ParseQueryString(queryString6);
        PrintDictionary(parsed6);

        // 示例7：包含特殊字符的查询字符串
        string queryString7 = "email=test%40example.com&message=Hello%20World%21";
        Console.WriteLine($"\n正在解析: \"{queryString7}\"");
        Dictionary<string, string> parsed7 = QueryStringParser.ParseQueryString(queryString7);
        PrintDictionary(parsed7);

        // 显示明显的退出提示
        Console.WriteLine("\n演示结束。按任意键退出...");
        Console.WriteLine("(如果窗口自动关闭，请在命令行中运行程序)");
        Console.ReadKey();
    }

    /// <summary>
    /// 辅助方法：打印字典内容
    /// </summary>
    /// <param name="dict">要打印的字典</param>
    public static void PrintDictionary(Dictionary<string, string> dict)
    {
        if (dict.Count == 0)
        {
            Console.WriteLine("  (空字典)");
            return;
        }
        foreach (var kvp in dict)
        {
            Console.WriteLine($"  键: \"{kvp.Key}\", 值: \"{kvp.Value}\"");
        }
    }
}
