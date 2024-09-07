using UnityEngine;

public class MovingToTowerState : IUnitState
{
    private Unit _unit;
    private Tower _tower;
    private float _distance = 0.55f;

    public MovingToTowerState(Unit unit, Tower tower)
    {
        _unit = unit;
        _tower = tower;
    }

    public void EnterState()
    {}

    public void ExitState()
    {
        _unit.DeliveredResource();
    }

    public void UpdateState()
    {
        MovingBase move = new MovingBase(_unit, _tower.transform.position);
        move.Update();

        if (Vector3.Distance(_unit.transform.position, _tower.transform.position) < _distance)
        {
            _unit.SetState(new WaitingState());
        }
    }
}
