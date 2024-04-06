#nullable enable
using Core.Utils.StateMachine;

namespace Core.Enemy.States
{
    public class MovementState : StateBase
    {
        private readonly EnemyController _controller;
        private readonly DefaultStateMachine _stateMachine;
        private readonly SurroundingManager _surroundingManager;
        
        public MovementState(EnemyController controller, DefaultStateMachine stateMachine)
        {
            _surroundingManager = Main.Instance.SurroundingManager;
            _controller = controller;
            _stateMachine = stateMachine;

            UpdateMovementPoint();
        }
        
        public override void Enter()
        {
            _controller.PlayerController.OnPlayerMovement += UpdateMovementPoint;
        }

        public override void LogicUpdate()
        {
            _controller.TryMove();
            if (_surroundingManager.CanJoinRadius(_controller))
            {
                _stateMachine.ChangeState(_controller.MovementInCircle);
            }
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            _controller.PlayerController.OnPlayerMovement -= UpdateMovementPoint;
        }

        private void UpdateMovementPoint()
        {
            _controller.SetPointForMovement(_controller.PlayerController.Transform.position);
        }
    }
}