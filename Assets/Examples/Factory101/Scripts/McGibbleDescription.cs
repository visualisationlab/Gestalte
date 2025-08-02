using System;

namespace Examples.Factory101.Scripts
{
    [Serializable]
    public class McGibbleDescription
    {
        public string name;
        public float normalizedRarity;
        public string singleEmoji;
        public float normalizedHeatAffinity;
        public int uniqueCreated; //Where in the order was this description generated newly
        //not communicated:
        public int raritySalePrice;
        public string shortDescription;

        public static string Format()
        {
            return @"
{
""name"": ""gibble"",
""singleEmoji"": ""🤷‍"",
""normalizedRarity"": 0.2,
""normalizedHeatAffinity"": 0.3,
""shortDescription"": ""A curious blend with a faint citrus aftertaste.""
}
";
        }
    }
}