#nullable enable
using Core.Enemy.States;
using Core.Player;
using Core.Utils.StateMachine;
using RadiusModule;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public class EnemyController : MonoBehaviour, ISurroundingObject
    {
        public float DistanceToPoint => _movement.GetDistanceToPoint();
        public StateBase MovementState { get; private set; } = null!;
        public StateBase MovementInCircle => _movementInCircle;
        public StateBase IdleState { get; private set; } = null!;
        public IPlayerController PlayerController { get; private set; } = null!;

        
        [SerializeField]
        private NavMeshAgent _agent = null!;

        private EnemyMovement _movement = null!;
        private DefaultStateMachine _stateMachine = null!;
        private MovementInCircle _movementInCircle;

        public void Init(IPlayerController playerController)
        {
            PlayerController = playerController;
            _movement = new EnemyMovement(transform, _agent);

            InitStates();
        }
        
        public void SetPointForMovement(Vector3 point)
        {
            if (_movement == null!)
            {
                return;
            }
            _movement.SetPoint(point);
        }
        
        public void TryMove() => _movement.TryMove();

        private void Update()
        {
            _stateMachine.CurrentState.LogicUpdate();
        }


        private void InitStates()
        {
            _stateMachine = new DefaultStateMachine();
            IdleState = new IdleState(this, _stateMachine);
            MovementState = new MovementState(this, _stateMachine);
            _movementInCircle = new MovementInCircle(this, _stateMachine);
            _stateMachine.Init(IdleState);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_movement != null!)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_movement.PointForMovement, 0.35f);

                Gizmos.color = Color.red;
                if (_movementInCircle.LastPoint != Vector3.zero)
                {
                    Gizmos.DrawSphere(_movementInCircle.LastPoint, 0.35f);
                }
            }
        }

        public Vector3 Position => transform.position;
        public float GetDistanceToCenter() => Vector3.Distance(PlayerController.Transform.position, transform.position);
    }
}