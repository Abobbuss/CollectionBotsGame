using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetDelay = 3f;
    [SerializeField] private int _maxRadius = 70;
    [SerializeField] private LayerMask _resourceLayerMask;

    public event UnityAction<List<Resource>> Finded;

    private void Start()
    {
        StartCoroutine(ScanForResources());
    }

    private IEnumerator ScanForResources()
    {
        WaitForSeconds delay = new WaitForSeconds(_resetDelay);
        
        while (true)
        {
            List<Resource> resources = DetectResources(_maxRadius);

            if (resources.Count > 0)
                Finded?.Invoke(resources.Where(findedResource => findedResource.gameObject.activeSelf).ToList());

            yield return delay;
        }
    }

    private List<Resource> DetectResources(float radius)
    {
        List<Resource> foundResources = new List<Resource>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, _resourceLayerMask);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Resource resource))
            {
                foundResources.Add(resource);
            }
        }

        return foundResources;
    }
}
