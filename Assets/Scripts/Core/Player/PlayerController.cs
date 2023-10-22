#nullable enable
using System;
using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public event Action? OnPlayerMovement;
        public Transform Transform => transform;

        public PlayerMovement PlayerMovement { get; private set; } = null!;
        public ControlBlocker ControlBlocker { get; private set; } = null!;
        
        [SerializeField] private CharacterController _characterController = null!;
        [SerializeField] private LayerMask _layerMaskWalls;
        [SerializeField] private Camera _camera = null!;
        [SerializeField] private AnimationCurve _dashCurve = null!; 
        
        private PlayerDashLogic _dashLogic = null!;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            ControlBlocker = new ControlBlocker();
            PlayerMovement = new PlayerMovement(_characterController, ControlBlocker);
            _dashLogic = new PlayerDashLogic(this, _layerMaskWalls, _dashCurve);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _dashLogic.Dash();
            }
            
            var condition = PlayerMovement.TryMove();
            if (condition)
            {
                OnPlayerMovement?.Invoke();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            var spherePosition = transform.position;
            spherePosition.y = .0f;
            Gizmos.DrawSphere(spherePosition, 0.25f);

            var points = CircleSurroundings.CalculatePointsInCircle(transform.position, 2.0f);
            foreach (var point in points)
            {
                var pointReady = new Vector3(point.x, 0.0f, point.z);
                Gizmos.color = Color.black;
                Gizmos.DrawLine(spherePosition, pointReady);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(pointReady, 0.15f);
            }

            if (!Application.isPlaying)
            {
                return;
            }

            points = CircleSurroundings.CalculatePointsInCircleWithWalls(transform.position, 2.0f, _layerMaskWalls);
            foreach (var point in points)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(spherePosition, point);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(point, 0.15f);
            }
        }
    }
}