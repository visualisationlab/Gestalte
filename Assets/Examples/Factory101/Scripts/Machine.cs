using UnityEngine;

namespace Examples.Factory101.Scripts
{
    public abstract class Machine: MonoBehaviour
    {
        public abstract void SetScript(string code);
    }
}