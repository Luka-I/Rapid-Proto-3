using UnityEngine;
using UnityEngine.SceneManagement;

public class YarisScript : MonoBehaviour
{
    [SerializeField] private CollectibleManager collectibleManager;
    public string EndAnimationScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectibleManager != null && collectibleManager.CanLeave)
            {
                SceneManager.LoadScene(EndAnimationScene);
            }
        }
    }
}
