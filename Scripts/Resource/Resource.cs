using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> OnReleased;
    
    public void Release()
    {
        OnReleased?.Invoke(this);
    }
}
