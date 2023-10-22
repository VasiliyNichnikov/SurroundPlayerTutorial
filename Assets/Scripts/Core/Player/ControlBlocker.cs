#nullable enable
using UnityEngine;

namespace Core.Player
{
    public class ControlBlocker
    {
        public bool IsLocked { get; private set; }

        public void Lock()
        {
            if (IsLocked)
            {
                Debug.LogWarning("ControlBlocked.Lock: isLocked already true");
                return;
            }

            IsLocked = true;
        }

        public void Unlock()
        {
            if (!IsLocked)
            {
                Debug.LogWarning("ControlBlocked.Lock: isLocked is not locked");
                return;
            }

            IsLocked = false;
        }
    }
}