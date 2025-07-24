using System;

namespace Mediator
{
    [AttributeUsage(AttributeTargets.Method)]
    
    //Notice! This Attribute will only work on MonoBehaviours
    public class ExposeMethodAttribute : Attribute
    {
        public string DisplayName { get; }

        public ExposeMethodAttribute(string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}