using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private Collider _zoneSpawn;

    private ObjectPool<Resource> _pool;
    private float _timeCreate = 1.5f;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: obj => OnRelease(obj),
            actionOnDestroy: obj => Destroy(obj.gameObject)
            );
    }

    private void Start()
    {
        StartCoroutine(StartGenerate());
    }

    public void OnRelease(Resource resource)
    {
        resource.transform.parent = transform;
        resource.gameObject.SetActive(false);
    }

    private IEnumerator StartGenerate()
    {
        var waitTime = new WaitForSeconds(_timeCreate);

        while (true)
        {
            yield return waitTime;

            _pool.Get();
        }
    }

    private Vector3 GetCreatingPosition()
    {
        Renderer renderer = _prefab.GetComponent<Renderer>() ?? throw new MissingComponentException("Prefab is missing a Renderer component.");
        float objectHeight = renderer.bounds.size.y / 2;
        Bounds bounds = _zoneSpawn.bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;

        Vector3 randomPosition = new Vector3(
            Random.Range(center.x - size.x / 2, center.x + size.x / 2),
            objectHeight,
            Random.Range(center.z - size.z / 2, center.z + size.z / 2)
        );

        return randomPosition;
    }

    private void OnGet(Resource resource)
    {
        resource.Delivered += Deliverd;
        resource.transform.position = GetCreatingPosition();
        resource.gameObject.SetActive(true);
    }

    private void Deliverd(Resource resource)
    {
        resource.Delivered -= Deliverd;
        OnRelease(resource);
    }

    private Resource Create()
    {
        return Instantiate(_prefab);
    }
}
