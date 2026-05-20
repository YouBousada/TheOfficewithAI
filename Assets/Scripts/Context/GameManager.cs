using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private World world;
    [SerializeField] private float gameOverDelay;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        world.AIMonitor.OnDeath += HandleDeath;
        world.OfficeStates.OnPowerOutage += HandlePowerOut;
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
        world.AIMonitor.OnDeath -= HandleDeath;
    }
}
