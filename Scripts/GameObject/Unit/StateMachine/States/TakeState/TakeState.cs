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

    public void Enter()
    {}

    public void Exit()
    {}

    public void Take()
    {
        _resource.transform.parent = _unit.transform;
    }

    public void Update()
    {
        _unit.SetState(new MoveToTowerState(_unit, _unit.Tower));
    }
}
