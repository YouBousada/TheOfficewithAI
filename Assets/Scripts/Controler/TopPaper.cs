using UnityEngine;
using UnityEngine.UI;

public class TopPaper : OverLayObject
{
    private Image Image;
    void Awake()
    {
        Image = GetComponent<Image>();
        gameContext.puzzleMaster.OnRoomStateChanged += EnterActiveView;
    }
    protected override void EnterActiveView()
    {
        if (!gameContext.puzzleMaster.IsTopDrawerOpen())
        {
            SetVisible(false);
            return;
        }

        SetVisible(true);
    }

    protected override void ExitActiveView()
    {
        SetVisible(false);
    }

    protected override void HideForJumpscare(CharacterName who, LocName from)
    {
        SetVisible(!hideUponJumpscare);
    }

    public void SetVisible(bool visible)
    {
        if (Image != null)
            Image.enabled = visible;
    }
}
