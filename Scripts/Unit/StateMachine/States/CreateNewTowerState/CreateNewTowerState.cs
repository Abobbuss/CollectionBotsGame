using System;
using UnityEngine;

public class CreateNewTowerState : IUnitState
{
    private Unit _unit;
    private MoverBase _move;
    private Flag _flag;

    public CreateNewTowerState(Unit unit, Flag towerPosition)
    {
        _unit = unit;
        _flag = towerPosition;
    }

    public void Enter()
    {
        _move = new MoverBase(_unit, _flag.transform.position);
    }

    public void Exit()
    { }

    public void Update()
    {
        _move.Update();

        if (_move.IsCathTarget())
        {
            _flag.gameObject.SetActive(false);
            _unit.CreateNewTower(_flag.transform.position);
        }
    }
}