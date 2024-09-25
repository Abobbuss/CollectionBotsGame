using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Tower : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<Unit> _units;

    private List<Resource> _findedResources;
    private List<Resource> _deliverdResources;
    private HashSet<int> _foundResourceIds = new HashSet<int>();

    public event UnityAction<int> ChangedCountDiscoverdResources;

    private void OnEnable()
    {
        _scanner.Finded += OnFindResource;
    }

    private void OnDisable()
    {
        _scanner.Finded -= OnFindResource;
    }
    
    private void Start()
    {
        _findedResources = new List<Resource>();
        _deliverdResources = new List<Resource>();
    }

    private void Update()
    {
        AssignUnitToResource();
    }

    public void DelivereResource(Resource resource)
    {
        _deliverdResources.Add(resource);
        ChangedCountDiscoverdResources?.Invoke(_deliverdResources.Count);
        resource.Release();
    }

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
        foreach (Resource resource in _findedResources.ToList())
        {
            Unit closestUnit = _units
                .Where(unit => unit.IsFree)
                .OrderBy(unit => (unit.transform.position - resource.transform.position).sqrMagnitude)
                .FirstOrDefault();

            if (closestUnit == null)
                break;

            closestUnit.SetTargetResource(resource);
            _findedResources.Remove(resource); 
        }
    }
    
    private void ResourceOnOnReleased(Resource resource)
    {
        resource.OnReleased -= ResourceOnOnReleased;
        _foundResourceIds.Remove(resource.GetInstanceID());
    }
}
