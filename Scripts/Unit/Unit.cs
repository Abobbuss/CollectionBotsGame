using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Tower _tower;

    private UnitStateMachine _stateMachineState;
    private Resource _currentResource;

    public bool IsFree => _stateMachineState.IsFree();
    public Tower Tower => _tower;

    private void Start()
    {
        _stateMachineState = new UnitStateMachine();
    }

    private void Update()
    {
        _stateMachineState.Update();
    }

    public void DeliveredResource()
    {
        _tower.DelivereResource(_currentResource);
        _currentResource = null;
    }

    public void SetState(IUnitState state)
    {
        _stateMachineState.SetState(state);
    }

    public void SetTargetResource(Resource resource)
    {
        _currentResource = resource;
        _stateMachineState.SetState(new MoveToResourceState(this, resource));
    }
}
