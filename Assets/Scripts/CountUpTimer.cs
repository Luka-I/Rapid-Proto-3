using UnityEngine;
using TMPro;

public class CountUpTimer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText; // Assign in Inspector

    private float elapsedTime = 0f;

    void Update()
    {
        // Increase elapsed time
        elapsedTime += Time.deltaTime;

        // Convert to minutes, seconds, milliseconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        // Display in 00:00:000 format
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
