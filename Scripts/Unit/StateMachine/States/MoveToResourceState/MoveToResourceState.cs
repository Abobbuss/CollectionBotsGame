using UnityEngine;

public class MoveToResourceState : IUnitState
{
    private Unit _unit;
    private Resource _resource;
    private float _distance = 0.3f;
    private MoverBase _move;

    public MoveToResourceState(Unit unit, Resource target) 
    {
        _resource = target;
        _unit = unit;
    }

    public void Enter()
    {
        _move = new MoverBase(_unit, _resource.transform.position);
    }

    public void Exit()
    {}

    public void Update()
    {
        _move.Update();

        float distanceSqr = (_unit.transform.position - _resource.transform.position).sqrMagnitude;
        float distanceThresholdSqr = _distance * _distance;

        if (distanceSqr < distanceThresholdSqr)
        {
            _unit.SetState(new TakeState(_unit, _resource));
        }
    }
}
