using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetDelay = 3f;
    [SerializeField] private int _maxRadius = 50;
    [SerializeField] private LayerMask _resourceLayerMask;
    
    private HashSet<int> _foundResourceIds = new HashSet<int>();

    public event UnityAction<List<Resource>> Finded;

    private void Start()
    {
        StartCoroutine(ScanForResources());
    }

    private IEnumerator ScanForResources()
    {
        var delay = new WaitForSeconds(_resetDelay);
        
        while (true)
        {
            var resources = DetectResources(_maxRadius);

            if (resources.Count > 0)
                Finded?.Invoke(resources.Where(findedResource => findedResource.gameObject.activeSelf).ToList());

            transform.localScale = Vector3.zero;

            yield return delay;
        }
    }

    private List<Resource> DetectResources(float radius)
    {
        List<Resource> foundResources = new List<Resource>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, _resourceLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Resource resource) && _foundResourceIds.Contains(resource.GetInstanceID()) == false)
            {
                resource.Delivered += ResourceOnDelivered;
                int resourceId = resource.GetInstanceID();
                foundResources.Add(resource);
                _foundResourceIds.Add(resourceId);
            }
        }

        return foundResources;
    }

    private void ResourceOnDelivered(Resource resource)
    {
        resource.Delivered -= ResourceOnDelivered;
        _foundResourceIds.Remove(resource.GetInstanceID());
    }
        
}
