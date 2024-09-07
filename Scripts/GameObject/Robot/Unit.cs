using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Tower _tower;

    private UnitStateMachine _machineState;
    private Resource _currentResource;

    public bool IsFree => _machineState.IsFree();
    public Tower Tower => _tower;

    public void DeliveredResource()
    {
        _tower.DelivereResource(_currentResource);
        _currentResource = null;
    }

    private void Start()
    {
        _machineState = new UnitStateMachine(this);
    }

    public void SetState(IUnitState state)
    {
        _machineState.SetState(state);
    }

    public void HasTarge(Resource resource)
    {
        _currentResource = resource;
        _machineState.SetState(new MovingToResourceState(this, resource));
    }

    private void Update()
    {
        _machineState.Update();
    }
}
