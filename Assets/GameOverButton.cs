using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.GoToNightScene();
    }
}
