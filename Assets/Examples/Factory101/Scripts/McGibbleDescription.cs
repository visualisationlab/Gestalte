using System;
using UnityEngine.Serialization;

namespace Examples.Factory101.Scripts
{
    [Serializable]
    public class McGibbleDescription
    {
        public string name;
        public float normalizedRarity;
        public string singleEmoji;
        //not communicated:
        public int salePrice;
        public int heatResistance;
        
        public static string Format()
        {
            return @"
{
""name"": ""gibble"",
""singleEmoji"": ""🤷‍"",
""normalizedRarity"" ""0.2""
}
";
        }
    }
}