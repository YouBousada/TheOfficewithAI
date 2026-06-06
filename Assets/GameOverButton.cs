using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void OnClick()
    {
        gameManager.GoToNightScene();
    }
}
