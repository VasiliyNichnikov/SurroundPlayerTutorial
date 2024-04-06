using Core.Utils.StateMachine;

namespace Core.Enemy.States
{
    public class IdleState : StateBase
    {
        private readonly EnemyController _controller;
        private readonly DefaultStateMachine _stateMachine;
        
        public IdleState(EnemyController controller, DefaultStateMachine stateMachine)
        {
            _controller = controller;
            _stateMachine = stateMachine;
        }
        
        public override void Enter()
        {
            _stateMachine.ChangeState(_controller.MovementState);
        }

        public override void LogicUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
           
        }

        public override void Exit()
        {
        }
    }
}