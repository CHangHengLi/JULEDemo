/*
To compile and run this program:

1. Save `QueryStringParser.cs` and `Program.cs` in the same directory.
2. Open a command prompt or terminal.
3. Navigate to the directory where you saved the files.

4. Compile the code:
   - If you have .NET SDK (recommended):
     csc Program.cs QueryStringParser.cs
   - This will create an executable file (e.g., Program.exe or Program, depending on your OS).

5. Run the executable:
   - Windows: Program.exe
   - macOS/Linux: ./Program

Example (using .NET SDK):
  cd path/to/your/files
  csc Program.cs QueryStringParser.cs
  ./Program 
  (or Program.exe on Windows)

Expected Output:
The program will print the results of parsing several example query strings,
showing the key-value pairs extracted by the QueryStringParser.
*/
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Query String Parser Demo");
        Console.WriteLine("------------------------");

        // Example 1: Basic query string
        string queryString1 = "name=JohnDoe&age=30&city=NewYork";
        Console.WriteLine($"\nParsing: \"{queryString1}\"");
        Dictionary<string, string> parsed1 = QueryStringParser.ParseQueryString(queryString1);
        PrintDictionary(parsed1);

        // Example 2: Query string with a leading '?'
        string queryString2 = "?product=Laptop&price=1200&available=true";
        Console.WriteLine($"\nParsing: \"{queryString2}\"");
        Dictionary<string, string> parsed2 = QueryStringParser.ParseQueryString(queryString2);
        PrintDictionary(parsed2);

        // Example 3: Query string with URL encoded characters
        string queryString3 = "search=C%23%20Programming&category=Development";
        Console.WriteLine($"\nParsing: \"{queryString3}\"");
        Dictionary<string, string> parsed3 = QueryStringParser.ParseQueryString(queryString3);
        PrintDictionary(parsed3);

        // Example 4: Query string with empty values and parameters without values
        string queryString4 = "name=Alice&id=&status=active&isAdmin";
        Console.WriteLine($"\nParsing: \"{queryString4}\"");
        Dictionary<string, string> parsed4 = QueryStringParser.ParseQueryString(queryString4);
        PrintDictionary(parsed4);
        
        // Example 5: Empty query string
        string queryString5 = "";
        Console.WriteLine($"\nParsing: \"{queryString5}\"");
        Dictionary<string, string> parsed5 = QueryStringParser.ParseQueryString(queryString5);
        PrintDictionary(parsed5);

        // Example 6: Query string with only '?'
        string queryString6 = "?";
        Console.WriteLine($"\nParsing: \"{queryString6}\"");
        Dictionary<string, string> parsed6 = QueryStringParser.ParseQueryString(queryString6);
        PrintDictionary(parsed6);

        // Example 7: Query string with special characters
        string queryString7 = "email=test%40example.com&message=Hello%20World%21";
        Console.WriteLine($"\nParsing: \"{queryString7}\"");
        Dictionary<string, string> parsed7 = QueryStringParser.ParseQueryString(queryString7);
        PrintDictionary(parsed7);

        Console.WriteLine("\nDemo finished. Press any key to exit.");
        Console.ReadKey();
    }

    // Helper method to print dictionary contents
    public static void PrintDictionary(Dictionary<string, string> dict)
    {
        if (dict.Count == 0)
        {
            Console.WriteLine("  (empty dictionary)");
            return;
        }
        foreach (var kvp in dict)
        {
            Console.WriteLine($"  Key: \"{kvp.Key}\", Value: \"{kvp.Value}\"");
        }
    }
}
