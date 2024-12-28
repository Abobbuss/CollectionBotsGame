using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Released;
    
    public bool IsDeatected { get; private set; }

    public void Release()
    {
        Released?.Invoke(this);
    }
    
    public void Find()
        => IsDeatected = true;
    
    public void UnFind()
        => IsDeatected = false;
}
