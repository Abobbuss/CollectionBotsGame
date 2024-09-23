using UnityEngine;

public class MoveToTowerState : IUnitState
{
    private Unit _unit;
    private Tower _tower;
    private float _distance = 0.75f;

    public MoveToTowerState(Unit unit, Tower tower)
    {
        _unit = unit;
        _tower = tower;
    }

    public void Enter()
    {}

    public void Exit()
    {
        _unit.DeliveredResource();
    }

    public void Update()
    {
        MoverBase move = new MoverBase(_unit, _tower.transform.position);
        move.Update();

        if (Vector3.Distance(_unit.transform.position, _tower.transform.position) < _distance)
        {
            _unit.SetState(new WaiteState());
        }
    }
}
