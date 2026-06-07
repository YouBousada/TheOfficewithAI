using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TopPaper : OverLayObject
{

    protected override void HideForJumpscare(CharacterName who, RoomName from) => SetVisible(false);

}
