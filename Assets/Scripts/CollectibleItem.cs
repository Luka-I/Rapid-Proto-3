using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemId; // Unique identifier for this collectible
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private ParticleSystem collectEffect;

    private CollectibleManager collectibleManager;
    private bool isCollected = false;

    private void Start()
    {
        // Find the collectible manager in the scene
        collectibleManager = Object.FindFirstObjectByType<CollectibleManager>();

        // Check if this item was already collected (for persistence)
        if (collectibleManager != null && collectibleManager.IsItemCollected(itemId))
        {
            gameObject.SetActive(false);
            isCollected = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        // Check if the collider is the player
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        isCollected = true;

        // Play collect sound if available
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Play particle effect if available
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        // Notify the collectible manager
        if (collectibleManager != null)
        {
            collectibleManager.CollectItem(itemId);
        }

        // Disable the game object
        gameObject.SetActive(false);
    }

    // Public property to access item ID
    public string ItemId => itemId;
}
