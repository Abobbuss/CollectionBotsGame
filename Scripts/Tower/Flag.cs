using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Flag : MonoBehaviour
{
    private bool _isActive;
    private Camera _mainCamera;
    private LayerMask _groundLayer;
    
    public float Height => GetComponent<Renderer>().bounds.size.y / 2;

    public void Initialize(Camera camera, LayerMask groundLayer)
    {
        _mainCamera = camera;
        _groundLayer = groundLayer;
    }

    public void ActivateFlag(Vector3 initialPosition)
    {
        gameObject.SetActive(true);
        PlaceFlag(initialPosition);
        _isActive = true;
    }

    public void DeactivateFlag()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }

    public void FollowMouse()
    {
        if (!_isActive) return;

        Vector3 mousePosition;
        
        if (GetMouseClickPositionOnGround(out mousePosition))
        {
            transform.position = mousePosition + new Vector3(0, Height, 0);;
        }
    }

    public void PlaceFlag(Vector3 position)
    {
        transform.position = position;
    }

    private bool GetMouseClickPositionOnGround(out Vector3 groundPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            groundPosition = hit.point;
            
            return true;
        }

        groundPosition = Vector3.zero;
        
        return false;
    }
}