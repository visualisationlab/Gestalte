using System.Collections.Generic;
using UnityEngine;

namespace Examples.Factory101.Scripts
{
    [CreateAssetMenu(fileName = "FallbackDescriptions", menuName = "McGibble/FallbackDescriptions")]
    public class McGibbleDescriptionFallbackList:ScriptableObject
    {
        public List<McGibbleDescription> descriptions;
    }
}