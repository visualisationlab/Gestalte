using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Examples.Factory101.Scripts
{
    public abstract class Machine: MonoBehaviour
    {
        [HideInInspector] public string script;
        public Script luaScript;

        public void SetScript(string code)
        {
            script = code;
            AfterSetScript();
        }

        protected abstract void RegisterLua();
        protected virtual void AfterSetScript()
        {
        }

    }
}