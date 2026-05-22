using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameContext world;
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
        if (world != null)
        {
            world.AIMonitor.OnDeath -= HandleDeath;
            world.OfficeStates.OnPowerOutage -= HandlePowerOut;
        }

        world = GameContext.Instance;

        if (world != null)
        {
            world.AIMonitor.OnDeath += HandleDeath;
            world.OfficeStates.OnPowerOutage += HandlePowerOut;
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
        if (world != null)
            world.AIMonitor.OnDeath -= HandleDeath;
            world.OfficeStates.OnPowerOutage -= HandlePowerOut;
    }
}
