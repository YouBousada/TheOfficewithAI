using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TopPaper : OverLayObject
{
    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetVisible(bool visible)
    {
        if (image != null)
            image.enabled = visible;
    }

    protected override void EnterActiveView() { }
    protected override void ExitActiveView() => SetVisible(false);
    protected override void HideForJumpscare(CharacterName who, LocName from) => SetVisible(false);
}
