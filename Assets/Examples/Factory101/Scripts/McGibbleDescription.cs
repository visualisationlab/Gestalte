using System;

namespace Examples.Factory101.Scripts
{
    [Serializable]
    public class McGibbleDescription
    {
        public string name;
        public float normalizedRarity;
        public string singleEmoji;
        public float normalizedHeatResistance;
        public int uniqueCreated; //Where in the order was this description generated newly
        //not communicated:
        public int salePrice;

        public static string Format()
        {
            return @"
{
""name"": ""gibble"",
""singleEmoji"": ""🤷‍"",
""normalizedRarity"": 0.2,
""normalizedHeatResistance"": 0.3
}
";
        }
    }
}