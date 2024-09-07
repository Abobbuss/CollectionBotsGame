public class UnitStateMachine
{
    private IUnitState _currentState;
    private Unit _unit;

    public UnitStateMachine(Unit unit)
    {
        _unit = unit;
        SetState(new WaitingState());
    }

    public bool IsFree()
        => _currentState is WaitingState;

    public void Update()
    {
        _currentState?.UpdateState();
    }

    public void SetState(IUnitState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }
}
