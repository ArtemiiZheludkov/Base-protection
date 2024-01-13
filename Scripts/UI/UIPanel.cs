using UnityEngine;

namespace BaseProtection
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract void Activate();
        public abstract void UpdatePanel();
    }
}