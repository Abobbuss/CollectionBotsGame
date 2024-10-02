using System;
using Unity.VisualScripting;
using UnityEngine;

public class TowerFlagManager : MonoBehaviour, IClickable
{
    [SerializeField] private LayerMask _towerLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Flag _flagPrefab;

    private Camera _mainCamera;
    private Flag _flag;
    private bool _isFlagActive;

    public event Action<Flag> OnPlacedFlag;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _flag = Instantiate(_flagPrefab);
    }

    private void Start()
    {
        if (_flag != null)
        {
            _flag.Initialize(_mainCamera, _groundLayer);
            _flag.DeactivateFlag();
        }
    }

    private void Update()
    {
        if (_isFlagActive && _flag.gameObject.activeSelf)
        {
            _flag.FollowMouse();
            
            if (Input.GetMouseButtonDown(0))
            {
                TryMoveFlag();
                _isFlagActive = false;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            DetectTowerClick();
        }
    }

    private void DetectTowerClick()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _towerLayer))
        {
            if (hit.collider.TryGetComponent<TowerFlagManager>(out var towerManager) && towerManager == this)
            {
                OnClick();
            }
        }
    }

    public void OnClick()
    {
        if (_flag != null && _flag.gameObject.activeSelf)
        {
            _isFlagActive = true;
        }
        else
        {
            TryPlaceFlag();
        }
    }

    private void TryPlaceFlag()
    {
        Vector3 clickPosition;

        if (GetMouseClickPosition(out clickPosition, _groundLayer))
        {
            Vector3 adjustedPosition = clickPosition + new Vector3(0, _flag.Height, 0);

            _flag.ActivateFlag(adjustedPosition);
            _isFlagActive = true;
        }
    }

    private void TryMoveFlag()
    {
        Vector3 clickPosition;

        if (GetMouseClickPosition(out clickPosition, _groundLayer))
        {
            _flag.PlaceFlag(clickPosition + new Vector3(0, _flag.Height, 0));
            OnPlacedFlag?.Invoke(_flag);
        }
    }

    private bool GetMouseClickPosition(out Vector3 clickPosition, LayerMask layerMask)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            clickPosition = hit.point;
            
            return true;
        }

        clickPosition = Vector3.zero;
        
        return false;
    }
}
