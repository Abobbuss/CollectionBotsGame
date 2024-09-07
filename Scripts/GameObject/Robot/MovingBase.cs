using UnityEngine;

public class MovingBase
{
    [SerializeField] private float _speed = 1.5f;

    private Unit _unit;
    private Vector3 _targetPosition;

    public MovingBase(Unit unit, Vector3 target) 
    { 
        _unit = unit;
        _targetPosition = target;
    }

    public void Update()
    {
        float step = _speed * Time.deltaTime;
        _unit.transform.position = Vector3.MoveTowards(_unit.transform.position, _targetPosition, step);
    }
}
