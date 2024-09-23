using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsDelivering = false;

    public event Action<Resource> Delivered;
    
    public void Release()
    {
        Delivered?.Invoke(this);
    }
}
