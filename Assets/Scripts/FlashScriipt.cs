using UnityEngine;

public class FlashScriipt : MonoBehaviour
{
    [Header("Flash Settings")]
    public Canvas flashCanvas;
    public float flashDuration = 0.2f;
    public AudioSource audioSource;

    private void Start()
    {
        // Ensure the canvas is disabled at start
        if (flashCanvas != null)
        {
            flashCanvas.enabled = false;
        }
    }

    // Alternative if using 2D colliders
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FlashCanvas());
        }
    }

    private System.Collections.IEnumerator FlashCanvas()
    {
        if (flashCanvas != null)
        {
            // Enable the canvas
            flashCanvas.enabled = true;
            audioSource.Play();
            // Wait for the specified duration
            yield return new WaitForSeconds(flashDuration);

            // Disable the canvas
            flashCanvas.enabled = false;
            Destroy(gameObject);
        }
    }
}