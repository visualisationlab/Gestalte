using UnityEngine;
using UnityEngine.Events;

namespace Examples.Factory101.Scripts.Input
{
    public class GlobalMenuManager:MonoBehaviour
    {
        public static GlobalMenuManager Instance { get; private set; }

        public UnityEvent<ExposeMachine> OnOpenMachineInfoScreen;
        
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void OpenMachineInfoScreen(ExposeMachine exposedMachine)
        {
            OnOpenMachineInfoScreen.Invoke(exposedMachine);
        }
        
    }
}