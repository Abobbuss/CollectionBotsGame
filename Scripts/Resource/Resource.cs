using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _isDeatected;
    public bool IsDeatected => _isDeatected;
    
    public event Action<Resource> OnReleased;
    
    public void Release()
    {
        OnReleased?.Invoke(this);
    }
    
    public void Finded(bool isFinded)
        => _isDeatected = isFinded;
}
