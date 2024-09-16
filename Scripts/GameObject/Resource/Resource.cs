using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public event UnityAction<Resource> Delivered;

    public void Release()
    {
        Delivered?.Invoke(this);
    }
}
