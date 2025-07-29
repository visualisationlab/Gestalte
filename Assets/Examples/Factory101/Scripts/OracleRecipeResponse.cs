namespace Examples.Factory101.Scripts
{
    public class OracleRecipeResponse
    {
        public string emoji;
        public float normalizedRarity;
        
        public static string Format()
        {
            return @"
{
""emoji"": ""🤷‍"",
""normalizedRarity"" ""0.2""
}
";
        }
    }
    
}