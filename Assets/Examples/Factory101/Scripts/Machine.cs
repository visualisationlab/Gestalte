using MoonSharp.Interpreter;
using UnityEngine;

namespace Examples.Factory101.Scripts
{
    public abstract class Machine: MonoBehaviour
    {
        [HideInInspector] public string script;
        public Script luaScript;
        
        protected virtual void Awake()
        {
            RegisterLua();
            AfterAwake();
        }
        
        public void SetScript(string code)
        {
            script = code;
            AfterSetScript();
        }

        protected abstract void RegisterLua();
        public abstract string GetStatus();

        protected virtual void AfterSetScript()
        {
        }
        
        protected virtual void AfterAwake() {}

    }
}