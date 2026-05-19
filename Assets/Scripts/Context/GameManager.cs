using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private float gameOverDelay = 3f;
    public void Start()
    {
        world.aiMonitor.OnDeath += HandleDeath;
    }
    private void TriggerGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void HandleDeath(CharacterName who, LocName from)
    {
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        TriggerGameOver();
    }

    void OnDestroy()
    {
        world.aiMonitor.OnDeath -= HandleDeath;
    }
}
