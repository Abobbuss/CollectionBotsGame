using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    private ResourceGenerator _generator;

    public event UnityAction<Resource> Delivered;

    public void Init(ResourceGenerator resourceGenerator)
    {
        _generator = resourceGenerator;
    }

    public void Release()
    {
        Delivered?.Invoke(this);
        _generator.OnRelease(this);
    }
}
