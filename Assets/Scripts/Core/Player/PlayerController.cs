#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using RadiusModule;
using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController, ICenterComponent
    {
        public event Action? OnPositionChanged
        {
            add => OnPlayerMovement += value;
            remove => OnPlayerMovement -= value;
        }
        public Vector3 Position => transform.position;
        public event Action? OnPlayerMovement;
        public Transform Transform => transform;

        public PlayerMovement PlayerMovement { get; private set; } = null!;
        public ControlBlocker ControlBlocker { get; private set; } = null!;
        public CharacterController CharacterController => _characterController;

        [SerializeField] private CharacterController _characterController = null!;
        [SerializeField] private LayerMask _layerMaskWalls;
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
    }
}