using System.Collections.Generic;
using UnityEngine;

public class DebugGUIDisplay : MonoBehaviour
{
    [SerializeField] private Office office;
    [SerializeField] private List<Animatronic> animatronics = new();
    [SerializeField] private ActiveCameraVariable activeCameraVariable;

    void OnGUI()
    {
        GUIStyle style = new(GUI.skin.label) { fontSize = 20 };
        style.normal.textColor = Color.green;

        if (office != null)
        {
            GUI.Label(new Rect(10, 10,  300, 40), $"Power: {office.Power}",                       style);
            GUI.Label(new Rect(10, 50,  300, 40), $"LeftDoorIsOpen: {office.LeftDoorIsOpen}",     style);
            GUI.Label(new Rect(10, 90,  300, 40), $"RightDoorIsOpen: {office.RightDoorIsOpen}",   style);
        }
        
        GUI.Label(new Rect(1000, 10,  300, 40), $"Active Camera: {activeCameraVariable.room}",                       style);

        for (int i = 0; i < animatronics.Count; i++)
        {
            Animatronic anim = animatronics[i];
            GUI.Label(
                new Rect(300, 10 + i * 30, 1000, 40),
                $"{anim.GetCharacterName()}: Job: {anim.GetCurrentJobName()} Location: {anim.GetCurrentLocation()}",
                style
            );
        }
    }
}
