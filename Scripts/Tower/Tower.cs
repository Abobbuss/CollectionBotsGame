using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TowerFlagManager))]
public class Tower : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private Unit _unitPrefab;

    private List<Resource> _findedResources;
    private List<Resource> _deliverdResources;
    private HashSet<int> _foundResourceIds = new HashSet<int>();
    private int _needResourcesToCreateUnit = 3;
    private int _needResourcesToCreatenewTower = 5;
    private int _maxCountUnits = 3;
    private TowerFlagManager _towerFlagManager;
    private bool _toCreateNewTower = false;
    private Flag _flag;
    private int _needUnitsCanCreateTower = 1;
    
    public event UnityAction<int> ChangedCountDiscoverdResources;

    private void Awake()
    {
        _towerFlagManager = GetComponent<TowerFlagManager>();
    }

    private void OnEnable()
    {
        _scanner.Finded += OnFindResource;
        _towerFlagManager.OnPlacedFlag += OnPlacedFlag;
    }

    private void OnPlacedFlag(Flag flag)
    {
        _toCreateNewTower = true;
        _flag = flag;
    }

    private void OnDisable()
    {
        _scanner.Finded -= OnFindResource;
        _towerFlagManager.OnPlacedFlag += OnPlacedFlag;
    }
    
    private void Start()
    {
        _findedResources = new List<Resource>();
        _deliverdResources = new List<Resource>();
    }

    private void Update()
    {
        if (_toCreateNewTower == false || _units.Count <= _needUnitsCanCreateTower)
        {
            if (_deliverdResources.Count >= _needResourcesToCreateUnit && _units.Count <= _maxCountUnits)
            {
                _deliverdResources.RemoveRange(0, _needResourcesToCreateUnit);
                CreateUnit();
                ChangedCountDiscoverdResources?.Invoke(_deliverdResources.Count);
            }
        }
        else
        {
            if (_deliverdResources.Count >= _needResourcesToCreatenewTower)
            {
                Unit unitToNewTower = _units.Where(unit => unit.IsFree).FirstOrDefault();
                unitToNewTower.SetState(new MoveToFlagState(unitToNewTower, _flag));
                _toCreateNewTower = false;
                _units.Remove(unitToNewTower);
                
                _deliverdResources.RemoveRange(0, _needResourcesToCreatenewTower);
                ChangedCountDiscoverdResources?.Invoke(_deliverdResources.Count);
            }
        }
        
        AssignUnitToResource();
    }

    public void DelivereResource(Resource resource)
    {
        _deliverdResources.Add(resource);
        ChangedCountDiscoverdResources?.Invoke(_deliverdResources.Count);
        resource.Release();
    }

    public void AddUnit(Unit unit)
        => _units.Add(unit);

    private void OnFindResource(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            if (_foundResourceIds.Contains(resource.GetInstanceID()) == false)
            {
                int resourceId = resource.GetInstanceID();
                _foundResourceIds.Add(resourceId);
                _findedResources.Add(resource);
                resource.OnReleased += ResourceOnOnReleased;
            }
        }
    }

    private void AssignUnitToResource()
    {
        foreach (Resource resource in _findedResources.OrderBy(resource => (resource.transform.position - gameObject.transform.position).sqrMagnitude).ToList())
        {
            if (resource.IsDeatected)
                continue;
            
            Unit closestUnit = _units
                .Where(unit => unit.IsFree)
                .OrderBy(unit => (unit.transform.position - resource.transform.position).sqrMagnitude)
                .FirstOrDefault();

            if (closestUnit == null)
                break;
            
            resource.Finded(true);
            closestUnit.SetTargetResource(resource);
            _findedResources.Remove(resource); 
        }
    }
    
    private void ResourceOnOnReleased(Resource resource)
    {
        resource.OnReleased -= ResourceOnOnReleased;
        _foundResourceIds.Remove(resource.GetInstanceID());
    }

    private void CreateUnit()
    {
        Unit unitObject = Instantiate(_unitPrefab);
        Vector3 spawnPosition = GetUnitSpawnPosition();
        unitObject.transform.position = spawnPosition;
        
        Unit unit = unitObject.GetComponent<Unit>();
        unit.Initialize(this);

        AddUnit(unit);
    }
    
    private Vector3 GetUnitSpawnPosition()
    {
        float randomValueToSpawn = 1f;
        Vector3 towerPosition = transform.position;
        float offsetX = Random.Range(-randomValueToSpawn, randomValueToSpawn);
        float offsetZ = Random.Range(-randomValueToSpawn, randomValueToSpawn);
        return towerPosition + new Vector3(offsetX, 0, offsetZ);
    }
}
