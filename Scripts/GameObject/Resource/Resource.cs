using UnityEngine;

public class Resource : MonoBehaviour
{
    private ResourceGenerator _generator;

    public void Init(ResourceGenerator resourceGenerator)
    {
        _generator = resourceGenerator;
    }

    public void Release()
    {
        _generator.OnRelease(this);
    }
}
