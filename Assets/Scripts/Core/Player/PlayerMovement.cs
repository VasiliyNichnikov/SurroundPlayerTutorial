using UnityEngine;

namespace Core.Player
{
    public class PlayerMovement
    {
        private readonly CharacterController _controller;

        private const float Speed = 10.0f;
        
        public PlayerMovement(CharacterController controller)
        {
            _controller = controller;
        }

        public bool TryMove()
        {
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

        private static float GetHorizontalInput() => Input.GetAxis("Horizontal");
        private static float GetVerticalInput() => Input.GetAxis("Vertical");
    }
}