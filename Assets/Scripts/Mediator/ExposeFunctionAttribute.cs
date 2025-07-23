using System;

namespace Mediator
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExposeFunctionAttribute : Attribute
    {
        public string DisplayName { get; }

        public ExposeFunctionAttribute(string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}