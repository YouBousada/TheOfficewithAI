using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAction", menuName = "Scriptable Objects/PlayerAction")]
public class ActionType : ScriptableObject
{
    [SerializeField] private ActionTypeName actionTypeName;

    [SerializeField] private float hotspotDisableDuration;

    public ActionTypeName ActionTypeName => actionTypeName;
    public float HotspotDisableDuration => hotspotDisableDuration;
}
