using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private AiMonitor aiMonitor;
    private Office officeStates;
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
        if (aiMonitor != null)    aiMonitor.OnDeath -= HandleDeath;
        if (officeStates != null) officeStates.OnPowerOutage -= HandlePowerOut;

        aiMonitor    = AiMonitor.Instance;
        officeStates = Office.Instance;

        if (aiMonitor != null)    aiMonitor.OnDeath += HandleDeath;
        if (officeStates != null) officeStates.OnPowerOutage += HandlePowerOut;
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
        if (aiMonitor != null)    aiMonitor.OnDeath -= HandleDeath;
        if (officeStates != null) officeStates.OnPowerOutage -= HandlePowerOut;
    }
}
