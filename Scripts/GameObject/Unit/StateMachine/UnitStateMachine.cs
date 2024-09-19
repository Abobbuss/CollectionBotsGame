public class UnitStateMachine
{
    private IUnitState _currentState;
    private Unit _unit;

    public UnitStateMachine(Unit unit)
    {
        _unit = unit;
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
