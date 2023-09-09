#nullable enable
using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public Transform Transform => transform;
        
        [SerializeField] private CharacterController _characterController = null!;

        private PlayerMovement _movement = null!;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _movement = new PlayerMovement(_characterController);
        }

        private void Update()
        {
            _movement.TryMove();
        }
    }
}