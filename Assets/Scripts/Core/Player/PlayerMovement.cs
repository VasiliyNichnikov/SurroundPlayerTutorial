using UnityEngine;

namespace Core.Player
{
    public class PlayerMovement
    {
        private readonly CharacterController _controller;
        private readonly ControlBlocker _blocker;
        
        private const float Speed = 7.0f;
        
        public PlayerMovement(CharacterController controller, ControlBlocker blocker)
        {
            _blocker = blocker;
            _controller = controller;
        }

        public bool TryMove()
        {
            if (_blocker.IsLocked)
            {
                return false;
            }
            
            var x = GetHorizontalInput();
            var z = GetVerticalInput();

            if (x == 0 && z == 0)
            {
                return false;
            }

            var direction = new Vector3(x, 0.0f, z) * Speed * Time.deltaTime;
            _controller.Move(direction);
            return true;
        }

        public static float GetHorizontalInput() => Input.GetAxis("Horizontal");
        public static float GetVerticalInput() => Input.GetAxis("Vertical");
    }
}