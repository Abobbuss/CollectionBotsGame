using UnityEngine;

public class MoveToFlagState : IUnitState
{
    private Unit _unit;
    private MoverBase _move;
    private Flag _flagPosition;

    public MoveToFlagState(Unit unit, Flag flagPosition)
    {
        _unit = unit;
        _flagPosition = flagPosition;
    }

    public void Enter()
    {
        _move = new MoverBase(_unit, _flagPosition.transform.position);
    }

    public void Exit()
    {

    }

    public void Update()
    {
        _move.Update();
        float dist = 1f;
        
        if (_move.IsCathTarget(dist))
            _unit.SetState(new CreateNewTowerState(_unit, _flagPosition));
    }
}
