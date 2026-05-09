using UnityEngine;

[CreateAssetMenu(fileName = "ScreenPosition", menuName = "Scriptable Objects/ScreenPosition")]
public class ScreenPosition : ScriptableObject
{
    [Header("Anchors")]
    public Vector2 anchorMin;
    public Vector2 anchorMax;

    [Header("Pivot")]
    public Vector2 pivot;

    [Header("Position")]
    public Vector3 anchoredPosition;   

    [Header("Size")]
    public Vector2 sizeDelta;           

    [Header("Transform")]
    public Vector3 rotation;            
    public Vector3 scale = Vector3.one;
}
