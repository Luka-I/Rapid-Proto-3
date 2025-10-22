using TMPro;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemId; // Unique ID for this collectible

    private CollectibleManager collectibleManager;

    private void Start()
    {
        collectibleManager = FindObjectOfType<CollectibleManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (collectibleManager != null)
        {
            collectibleManager.CollectItem(itemId);
        }

        gameObject.SetActive(false);
    }

    public string ItemId => itemId;
}
