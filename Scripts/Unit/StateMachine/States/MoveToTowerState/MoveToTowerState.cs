
public class MoveToTowerState : IUnitState
{
    private Unit _unit;
    private Tower _tower;
    private MoverBase _move;

    public MoveToTowerState(Unit unit, Tower tower)
    {
        _unit = unit;
        _tower = tower;
    }

    public void Enter()
    {
        _move = new MoverBase(_unit, _tower.transform.position);
    }

    public void Exit()
    {
        _unit.DeliveredResource();
    }

    public void Update()
    {
        _move.Update();
        
        if(_move.IsCathTarget())
            _unit.SetState(new WaiteState());
    }
}
