using UnityEngine;

public class MoveToResourceState : IUnitState
{
    private Unit _unit;
    private Resource _resource;
    private float _distance = 0.3f;

    public MoveToResourceState(Unit unit, Resource target) 
    {
        _resource = target;
        _unit = unit;
    }

    public void EnterState()
    {}

    public void ExitState()
    {}

    public void UpdateState()
    {
        MoverBase move = new MoverBase(_unit, _resource.transform.position);
        move.Update();

        if (Vector3.Distance(_unit.transform.position, _resource.transform.position) < _distance)
            _unit.SetState(new TakeState(_unit, _resource));
    }
}
