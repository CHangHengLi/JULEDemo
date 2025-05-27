/*
To run these tests:

1. Ensure you have the .NET SDK installed.
2. Navigate to the `QueryApp.Tests` directory in your terminal or command prompt.
   (This is the directory containing `QueryApp.Tests.csproj` and this test file).

3. Execute the tests using the .NET CLI:
   dotnet test

   This command will discover, compile, and run the tests in the project.
   The output will indicate whether the tests passed or failed.

Alternatively, if you are using an IDE like Visual Studio or Rider:
- Open the solution/folder containing both `QueryApp` and `QueryApp.Tests`.
- Use the Test Explorer window to run the tests.
  - In Visual Studio: Test > Test Explorer
  - In Rider: View > Tool Windows > Unit Tests
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System; // Required for Uri class

namespace QueryApp.Tests
{
    [TestClass]
    public class QueryStringParserTests
    {
        private void AssertDictionariesEqual(Dictionary<string, string> expected, Dictionary<string, string> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Dictionary counts are not equal.");
            foreach (var kvp in expected)
            {
                Assert.IsTrue(actual.ContainsKey(kvp.Key), $"Actual dictionary does not contain key '{kvp.Key}'.");
                Assert.AreEqual(kvp.Value, actual[kvp.Key], $"Value for key '{kvp.Key}' is not as expected.");
            }
        }

        [TestMethod]
        public void TestEmptyQueryString()
        {
            var result = QueryStringParser.ParseQueryString("");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestNullQueryString()
        {
            var result = QueryStringParser.ParseQueryString(null);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestQueryStringWithQuestionMark()
        {
            var expected = new Dictionary<string, string> { { "name", "value" } };
            var actual = QueryStringParser.ParseQueryString("?name=value");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestQueryStringIsOnlyQuestionMark()
        {
            var result = QueryStringParser.ParseQueryString("?");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestMultipleParameters()
        {
            var expected = new Dictionary<string, string> 
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" }
            };
            var actual = QueryStringParser.ParseQueryString("key1=value1&key2=value2&key3=value3");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestUrlEncodedParameters()
        {
            var expected = new Dictionary<string, string>
            {
                { "name", "John Doe" },
                { "city", "New York" },
                { "email", "test@example.com" }
            };
            var actual = QueryStringParser.ParseQueryString("name=John%20Doe&city=New%20York&email=test%40example.com");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestParameterWithoutValue()
        {
            // According to the original code, parameters without values are stored with string.Empty as their value.
            var expected = new Dictionary<string, string>
            {
                { "param1", "" },
                { "param2", "value2" },
                { "param3", "" }
            };
            var actual = QueryStringParser.ParseQueryString("param1&param2=value2&param3");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestParameterWithEmptyValue()
        {
            var expected = new Dictionary<string, string>
            {
                { "key1", "" },
                { "key2", "value2" }
            };
            var actual = QueryStringParser.ParseQueryString("key1=&key2=value2");
            AssertDictionariesEqual(expected, actual);
        }
        
        [TestMethod]
        public void TestParameterWithEmptyValueAndNoMoreParameters()
        {
            var expected = new Dictionary<string, string> { { "key1", "" } };
            var actual = QueryStringParser.ParseQueryString("key1=");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestComplexQueryString()
        {
            var expected = new Dictionary<string, string>
            {
                { "name", "test user" },
                { "id", "" },
                { "status", "active" },
                { "isAdmin", "" },
                { "emptyVal", "" },
                { "data", "some data" }
            };
            // Starting with '?', contains URL encoding, empty values, valueless parameters
            var actual = QueryStringParser.ParseQueryString("?name=test%20user&id=&status=active&isAdmin&emptyVal=&data=some%20data");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestQueryStringWithSpacesAroundAmpersand()
        {
            // The current implementation of Split('&') will treat spaces as part of the key/value if not trimmed.
            // Let's test current behavior. If desired, QueryStringParser could be updated to trim parts.
            // Based on current QueryStringParser: " key2" would be the key.
            // However, Uri.UnescapeDataString might handle some whitespace. Let's assume it doesn't trim outer spaces.
            // string[] parameters = queryString.Split('&'); -> "key1=value1 ", " key2=value2"
            // So, key becomes "key1", value becomes "value1 ". Key becomes " key2", value "value2".
            // This needs to be verified against actual Uri.UnescapeDataString behavior for spaces.
            // Uri.UnescapeDataString("value1 ") is "value1 ". Uri.UnescapeDataString(" key2") is " key2".
            var expected = new Dictionary<string, string>
            {
                { "key1", "value1 " }, // Note the trailing space if not trimmed by UnescapeDataString or Split
                { " key2", "value2" }  // Note the leading space
            };
            // Let's refine based on typical parsing expectations where keys/values are trimmed.
            // The provided code does *not* explicitly trim whitespace from split parts.
            // Let's assume the intent is that `Uri.UnescapeDataString` handles it or they are part of the value/key.

            // Re-evaluating based on `parameter.Substring` and `Uri.UnescapeDataString`:
            // "key1=value1 & key2=value2"
            // parameters[0] = "key1=value1 " -> key = "key1", value = "value1 "
            // parameters[1] = " key2=value2" -> key = " key2", value = "value2"
            var actual = QueryStringParser.ParseQueryString("key1=value1 & key2=value2");
             AssertDictionariesEqual(expected, actual);
        }
        
        [TestMethod]
        public void TestQueryStringWithSpacesAroundEquals()
        {
            // "key1 = value1&key2 =value2 "
            // parameters[0] = "key1 = value1" -> key = "key1 ", value = " value1"
            // parameters[1] = "key2 =value2 " -> key = "key2 ", value = "value2 "
            var expected = new Dictionary<string, string>
            {
                { "key1 ", " value1" },
                { "key2 ", "value2 " }
            };
            var actual = QueryStringParser.ParseQueryString("key1 = value1&key2 =value2 ");
            AssertDictionariesEqual(expected, actual);
        }

        [TestMethod]
        public void TestQueryStringWithOnlyAmpersands()
        {
            var result = QueryStringParser.ParseQueryString("&&&");
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count, "Dictionary should be empty for query string with only ampersands.");
        }

        [TestMethod]
        public void TestQueryStringWithEmptyParametersBetweenValid()
        {
            var expected = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };
            var actual = QueryStringParser.ParseQueryString("key1=value1&&key2=value2");
            AssertDictionariesEqual(expected, actual);
        }
    }
}
