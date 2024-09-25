using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Tower _tower;

    private UnitStateMachine _stateMachine;
    private Resource _currentResource;

    public bool IsFree => _stateMachine.IsFree();
    public Tower Tower => _tower;

    private void Start()
    {
        _stateMachine = new UnitStateMachine();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void DeliveredResource()
    {
        _tower.DelivereResource(_currentResource);
        _currentResource = null;
    }

    public void SetState(IUnitState state)
    {
        _stateMachine.SetState(state);
    }

    public void SetTargetResource(Resource resource)
    {
        _currentResource = resource;
        _stateMachine.SetState(new MoveToResourceState(this, resource));
    }
}
