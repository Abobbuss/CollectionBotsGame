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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            _findedResources.Add(resource);
    }

    private IEnumerator ScanForResources()
    {
        while (true)
        {
            while (transform.localScale.x < _maxRadius)
            {
                float growing = _speed * Time.deltaTime;
                transform.localScale += new Vector3(growing, growing, growing);

                yield return null;
            }

            if (_findedResources.Count > 0)
                Finded?.Invoke(_findedResources.Where(findedResource => findedResource.gameObject.activeSelf).ToList());

            yield return ResetScanner();
        }
    }

    private IEnumerator ResetScanner()
    {
        _findedResources.Clear();
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(_resetDelay);
    }
}
