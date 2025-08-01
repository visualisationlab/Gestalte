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
        public int upgradedAmount;
        public int maxUpgradeLevel;

        public bool MaxUpgradeLevelReached()
        {
            return upgradedAmount >= maxUpgradeLevel;
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

        protected abstract void RegisterLua();
        public abstract string GetStatus();
        public abstract string UpgradeMachine();

        protected virtual void AfterSetScript()
        {
        }
        
        protected virtual void AfterAwake() {}

    }
}