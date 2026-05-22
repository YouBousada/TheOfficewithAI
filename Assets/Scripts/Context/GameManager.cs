using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameContext gameContext;
    [SerializeField] private float gameOverDelay;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameContext != null)
        {
            gameContext.AIMonitor.OnDeath -= HandleDeath;
            gameContext.OfficeStates.OnPowerOutage -= HandlePowerOut;
        }

        gameContext = GameContext.Instance;

        if (gameContext != null)
        {
            gameContext.AIMonitor.OnDeath += HandleDeath;
            gameContext.OfficeStates.OnPowerOutage += HandlePowerOut;
        }
    }

    private void GoToGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void GoToNightScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void HandleDeath(CharacterName who, LocName from)
    {
        StartCoroutine(DelayedGameOver());
    }

    private void HandlePowerOut()
    {
        GoToGameOverScene();
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        GoToGameOverScene();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (gameContext != null)
            gameContext.AIMonitor.OnDeath -= HandleDeath;
            gameContext.OfficeStates.OnPowerOutage -= HandlePowerOut;
    }
}
