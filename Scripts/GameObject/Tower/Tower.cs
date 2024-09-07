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

    public event UnityAction<int> ChangedCountFindedResources;
    public event UnityAction<int> ChangedCountDiscoverdResources;

    private void Start()
    {
        _findedResources = new List<Resource>();
        _deliverdResources = new List<Resource>();
    }

    private void OnEnable()
    {
        _scanner.Finded += FindedResource;
    }
    private void OnDisable()
    {
        _scanner.Finded -= FindedResource;
    }

    public void DelivereResource(Resource resource)
    {
        _deliverdResources.Add(resource);
        ChangedCountDiscoverdResources?.Invoke(_deliverdResources.Count);
        resource.Release();
    }

    private void FindedResource(List<Resource> resources)
    {
        foreach (Resource resource in resources)
        {
            _findedResources.Add(resource);
            AssignUnitToResource(resource);
        }

        ChangedCountFindedResources?.Invoke(_findedResources.Count);
    }
    private void AssignUnitToResource(Resource resource)
    {
        Unit freeUnit = _units.FirstOrDefault(unit => unit.IsFree);

        if (freeUnit != null)
            freeUnit.HasTarge(resource);
    }
}
