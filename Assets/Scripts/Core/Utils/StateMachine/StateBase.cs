namespace Core.Utils.StateMachine
{
    public abstract class StateBase
    {
        public abstract void Enter();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void Exit();
    }
}