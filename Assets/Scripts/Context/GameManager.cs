using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Office officeStates;
    [SerializeField] private float gameOverDelay;

    void Awake()
    {
        if (officeStates != null) officeStates.OnPowerOutage += HandlePowerOut;
    }

    public void GoToNightScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HandleDeath(JumpscareEventData data)
    {
        StartCoroutine(DelayedGameOver());
    }

    private void HandlePowerOut()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("GameOverScene");
    }

    void OnDestroy()
    {
        if (officeStates != null) officeStates.OnPowerOutage -= HandlePowerOut;
    }
}
