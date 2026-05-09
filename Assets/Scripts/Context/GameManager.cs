using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;

    public void TriggerGameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene("GameOverScene");

    }

    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene("GameScene");
    }
}
