using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [Header("Collectible Settings")]
    [SerializeField] private int totalCollectibles = 60;

    [Header("UI References")]
    [SerializeField] private TMP_Text collectibleText;

    private int collectedCount = 0;
    public bool CanLeave { get; private set; }

private void Start()
    {
        // Always start fresh
        collectedCount = 0;
        UpdateUI();

        Debug.Log($"Game started with {collectedCount}/{totalCollectibles} collectibles");
    }

    public void CollectItem(string itemId)
    {
        collectedCount++;
        UpdateUI();

        if (collectedCount >= totalCollectibles)
        {
            OnAllCollectiblesCollected();
        }

        Debug.Log($"Collected {itemId}. Total: {collectedCount}/{totalCollectibles}");
    }

    private void UpdateUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"{collectedCount}/{totalCollectibles}";
        }
    }

    public void OnAllCollectiblesCollected()
    {
        Debug.Log("All collectibles collected!");
        CanLeave = true;
        
    }

    // Method to reset all collectibles
    public void ResetCollectibles()
    {
        collectedCount = 0;
        UpdateUI();

        var collectibles = Object.FindObjectsByType<CollectibleItem>(FindObjectsSortMode.None);
        foreach (var collectible in collectibles)
        {
            collectible.gameObject.SetActive(true);
        }

        Debug.Log("All collectibles reset!");
    }

    public int CollectedCount => collectedCount;
    public int TotalCollectibles => totalCollectibles;
}