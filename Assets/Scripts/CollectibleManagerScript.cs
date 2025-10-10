using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private int totalCollectibles = 60;

    [Header("UI References")]
    [SerializeField] private TMP_Text collectibleText;

    private int collectedCount = 0;
    private HashSet<string> collectedItems = new HashSet<string>();

    // Event for when a collectible is picked up
    public System.Action<int, int> OnCollectibleCollected;

    private void Start()
    {
        LoadCollectedItems();
        UpdateUI();
    }

    public void CollectItem(string itemId)
    {
        if (collectedItems.Contains(itemId))
            return;

        collectedItems.Add(itemId);
        collectedCount++;



        // Update UI
        UpdateUI();

        // Trigger event
        OnCollectibleCollected?.Invoke(collectedCount, totalCollectibles);

        // Check for completion
        if (collectedCount >= totalCollectibles)
        {
            OnAllCollectiblesCollected();
        }
    }

    public bool IsItemCollected(string itemId)
    {
        return collectedItems.Contains(itemId);
    }

    private void UpdateUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"{collectedCount}/{totalCollectibles}";
        }
    }

    private void OnAllCollectiblesCollected()
    {
        Debug.Log("All collectibles collected!");
        // You can add any completion logic here without a UI panel
        // For example: play a sound, show particle effect, unlock achievement, etc.
    }

    

    private void LoadCollectedItems()
    {
        collectedCount = PlayerPrefs.GetInt("TotalCollected", 0);

        // Note: This is a simple implementation. For better performance,
        // you might want to store all collectible IDs and check each one.
        // This implementation relies on the CollectibleItem checking on Start.
    }

    // Method to reset all collectibles (for testing or new game)
    public void ResetCollectibles()
    {
        collectedItems.Clear();
        collectedCount = 0;
        PlayerPrefs.DeleteAll();
        UpdateUI();

        var collectibles = Object.FindObjectsByType<CollectibleItem>(FindObjectsSortMode.None);
        foreach (var collectible in collectibles)
        {
            collectible.gameObject.SetActive(true);
        }
    }

    // Public properties to access collectible stats
    public int CollectedCount => collectedCount;
    public int TotalCollectibles => totalCollectibles;
    public float CollectionPercentage => totalCollectibles > 0 ? (float)collectedCount / totalCollectibles * 100f : 0f;

    //poistettu saveus
}