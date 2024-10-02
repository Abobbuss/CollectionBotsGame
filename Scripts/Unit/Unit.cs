using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Tower _tower;
    [SerializeField] private Tower _towerPrefab;
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

    public void Initialize(Tower tower)
    {
        _tower = tower;
    }

    public void CreateNewTower(Vector3 position)
    {
        Tower towerObject = Instantiate(_towerPrefab);
        towerObject.transform.position = position;
        
        Tower tower = towerObject.GetComponent<Tower>();
        Initialize(tower);
        tower.AddUnit(this);
        SetState(new WaiteState());
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
