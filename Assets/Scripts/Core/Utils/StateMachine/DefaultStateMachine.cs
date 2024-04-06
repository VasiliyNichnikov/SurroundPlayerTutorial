namespace Core.Utils.StateMachine
{
    public class DefaultStateMachine
    {
        public StateBase CurrentState { get; private set; }

        public void Init(StateBase startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void ChangeState(StateBase newState)
        {
            CurrentState.Exit();

            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}