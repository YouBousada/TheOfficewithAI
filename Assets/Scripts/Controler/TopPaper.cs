using UnityEngine;
using UnityEngine.UI;

public class TopPaper : OverLayObject
{
    // private Image Image;
    // [SerializeField] private PuzzleMaster puzzleMaster;

    // void Awake()
    // {
    //     Image = GetComponent<Image>();

    //     puzzleMaster.OnRoomStateChanged += EnterActiveView;
    // }
    // protected override void EnterActiveView()
    // {
    //     if (!puzzleMaster.IsTopDrawerOpen())
    //     {
    //         SetVisible(false);
    //         return;
    //     }

    //     SetVisible(true);
    // }

    // protected override void ExitActiveView()
    // {
    //     SetVisible(false);
    // }

    // protected override void HideForJumpscare(CharacterName who, LocName from)
    // {
    //     SetVisible(!hideUponJumpscare);
    // }

    // private void SetVisible(bool visible)
    // {
    //     if (Image != null)
    //         Image.enabled = visible;
    // }
    protected override void EnterActiveView()
    {
        throw new System.NotImplementedException();
    }

    protected override void ExitActiveView()
    {
        throw new System.NotImplementedException();
    }

    protected override void HideForJumpscare(CharacterName who, LocName from)
    {
        throw new System.NotImplementedException();
    }
}
