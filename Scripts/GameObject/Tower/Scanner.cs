using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _resetDelay = 5f;
    [SerializeField] private int _maxRadius = 50;

    private List<Resource> _findedResources = new List<Resource>();

    public event UnityAction<List<Resource>> Finded;

    private void Start()
    {
        StartCoroutine(ScanForResources());
    }

    private IEnumerator ScanForResources()
    {
        while (true)
        {
            DetectResources(_maxRadius);

            if (_findedResources.Count > 0)
                Finded?.Invoke(_findedResources.Where(findedResource => findedResource.gameObject.activeSelf).ToList());

            yield return ResetScanner();
        }
    }

    private void DetectResources(float radius)
    {
        _findedResources.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Resource resource))
                _findedResources.Add(resource);
        }
    }

    private IEnumerator ResetScanner()
    {
        transform.localScale = Vector3.zero;
        var delay = new WaitForSeconds(_resetDelay);

        yield return delay;
    }
}
