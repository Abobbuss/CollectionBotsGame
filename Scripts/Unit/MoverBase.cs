using UnityEngine;

public class MoverBase
{
    private float _speed = 1.5f;
    private Unit _unit;
    private Vector3 _targetPosition;

    public MoverBase(Unit unit, Vector3 target) 
    { 
        _unit = unit;
        _targetPosition = target;
    }

    public void Update()
    {
        float step = _speed * Time.deltaTime;
        _unit.transform.position = Vector3.MoveTowards(_unit.transform.position, _targetPosition, step);
    }

    public bool IsCathTarget(float distance = 0.75f)
    {
        float distanceSqr = (_unit.transform.position - _targetPosition).sqrMagnitude;
        float distanceThresholdSqr = distance * distance;

        return distanceSqr < distanceThresholdSqr;
    }
}
