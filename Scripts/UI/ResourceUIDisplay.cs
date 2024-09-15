using TMPro;
using UnityEngine;

public class ResourceUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _haveResources;
    [SerializeField] private Tower _tower;

    private void OnEnable()
    {
        _tower.ChangedCountDiscoverdResources += ChangeDeliveredResource;
    }

    private void OnDisable()
    {
        _tower.ChangedCountDiscoverdResources -= ChangeDeliveredResource;
    }

    private void ChangeDeliveredResource(int count)
    {
        _haveResources.text = $"{count}";
    }
}
