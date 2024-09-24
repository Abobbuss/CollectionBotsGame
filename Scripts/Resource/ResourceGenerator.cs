using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private Collider _zoneSpawn;
    [SerializeField] private float _timeCreate = 8f;

    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: Create,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
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
        
        float halfSizeFactor = 0.5f;
        float halfObjectHeight = renderer.bounds.size.y * halfSizeFactor;

        Bounds spawnBounds = _zoneSpawn.bounds;
        Vector3 boundsCenter = spawnBounds.center;
        Vector3 boundsSize = spawnBounds.size;
        
        float randomXPosition = Random.Range(boundsCenter.x - boundsSize.x * halfSizeFactor, boundsCenter.x + boundsSize.x * halfSizeFactor);
        float randomZPosition = Random.Range(boundsCenter.z - boundsSize.z * halfSizeFactor, boundsCenter.z + boundsSize.z * halfSizeFactor);
        
        return new Vector3(randomXPosition, halfObjectHeight, randomZPosition);
    }

    private void OnGet(Resource resource)
    {
        resource.Delivered += Deliverd;
        resource.transform.position = GetCreatingPosition();
        resource.transform.rotation = Quaternion.identity;
        resource.gameObject.SetActive(true);
    }

    private void Deliverd(Resource resource)
    {
        resource.Delivered -= Deliverd;
        _pool.Release(resource);
    }

    private Resource Create()
    {
        return Instantiate(_prefab);
    }
}
