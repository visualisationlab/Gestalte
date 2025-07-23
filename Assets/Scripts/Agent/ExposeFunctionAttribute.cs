using System;

namespace Mediator
{
    [AttributeUsage(AttributeTargets.Method)]
    
    //Notice! This Attribute will only work on MonoBehaviours
    public class ExposeFunctionAttribute : Attribute
    {
        public string DisplayName { get; }

        public ExposeFunctionAttribute(string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}