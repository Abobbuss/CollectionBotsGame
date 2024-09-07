public class TakeState : IUnitState
{
    private Unit _unit;
    private Resource _resource;

    public TakeState(Unit unit, Resource resource)
    {
        _unit = unit;
        _resource = resource;
        Take();
    }

    public void EnterState()
    {}

    public void ExitState()
    {}

    public void Take()
    {
        _resource.transform.parent = _unit.transform;
    }

    public void UpdateState()
    {
        _unit.SetState(new MovingToTowerState(_unit, _unit.Tower));
    }
}
