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

    public event UnityAction<int> ChangedCountDiscoverdResources;

    private void Start()
    {
        _findedResources = new List<Resource>();
        _deliverdResources = new List<Resource>();
    }

    private void Update()
    {
        AssignUnitToResource();
    }

    private void OnEnable()
    {
        _scanner.Finded += OnFindResource;
    }

    private void OnDisable()
    {
        _scanner.Finded -= OnFindResource;
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
            if (!_findedResources.Contains(resource))
            {
                _findedResources.Add(resource);
            }
        }
    }

    private void AssignUnitToResource()
    {
        foreach (Resource resource in _findedResources.ToList())
        {
            Unit closestUnit = _units
                .Where(unit => unit.IsFree)
                .OrderBy(unit => Vector3.Distance(unit.transform.position, resource.transform.position))
                .FirstOrDefault();

            if (closestUnit == null)
                break;

            closestUnit.SetTargetResource(resource);
            _findedResources.Remove(resource); 
        }
    }
}
