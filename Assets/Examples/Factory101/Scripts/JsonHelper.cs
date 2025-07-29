using System.Text.RegularExpressions;

namespace Examples.Factory101.Scripts
{
    public static class JsonHelper
    {
        public static string ExtractJson(string input)
        {
            var match = Regex.Match(input, @"\{[\s\S]*?\}");
            return match.Success ? match.Value : null;
        }
    }
}