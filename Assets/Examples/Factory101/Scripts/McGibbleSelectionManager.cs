using UnityEngine;
using UnityEngine.Events;

namespace Examples.Factory101.Scripts
{
    public class McGibbleSelectionManager:MonoBehaviour
    {
        [Header("Description")]
        public UnityEvent<string> OnMcGibbleHoverDescription;
        public UnityEvent OnMcGibbleHoverOut;

        public void HoverGameObject(GameObject go)
        {
            var mcGibble = go.GetComponent<McGibble>();
            if (mcGibble == null) return;
            OnMcGibbleHoverDescription.Invoke(mcGibble.GetFullDescription());
        }

        public void HoverOutGameObject()
        {
            OnMcGibbleHoverOut.Invoke();
        }
    }
}