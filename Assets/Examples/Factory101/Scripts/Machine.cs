using MoonSharp.Interpreter;
using UnityEngine;

namespace Examples.Factory101.Scripts
{
    public abstract class Machine: MonoBehaviour
    {
        [HideInInspector] public string script;
        public Script luaScript;
        public int basePrice;
        public int baseUpgradePrice;
        public int upgradeLevel = 1;// The Upgraded Level
        public int maxUpgradeLevel;
        [TextArea(3,12)] public string description;

        public bool MaxUpgradeLevelReached()
        {
            return upgradeLevel >= maxUpgradeLevel;
        }
        
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

        public void ExecuteScript()
        {
            if (!string.IsNullOrWhiteSpace(script))
            {
                luaScript.DoString(script);
            }
        }

        protected abstract void RegisterLua();
        public abstract string GetStatus();
        public abstract void UpgradeMachine();

        protected virtual void AfterSetScript()
        {
        }
        
        protected virtual void AfterAwake() {}

    }
}