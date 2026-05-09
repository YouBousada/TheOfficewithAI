using System.Collections.Generic;
using UnityEngine;

public class ViewData : MonoBehaviour
{
    public View viewName;
    public List<GameObject> objectsInView = new();
    public List<GameObject> hideUponJumpscare = new();
    public Sprite thisViewBackground;

    [Header("For Non Door Office Views Only")]
    public JumpscareData backgroundJumpscareData;

    [Header("For Door Office Views Only")]
    public DoorViewJumpscareData doorViewJumpscareData;
}