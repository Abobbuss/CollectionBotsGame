public class UnitStateMachine
{
    private IUnitState _currentState;

    public UnitStateMachine()
    {
        SetState(new WaiteState());
    }

    public bool IsFree()
        => _currentState is WaiteState;

    public void Update()
    {
        _currentState?.Update();
    }

    public void SetState(IUnitState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
