using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAction", menuName = "Scriptable Objects/PlayerAction")]
public class PlayerAction : ScriptableObject
{
    [SerializeField] private ActionTypeName actionName;
    [SerializeField] private View allowedView;
    [SerializeField] private float hotspotDisableDuration;

    public ActionTypeName ActionName => actionName;
    public View AllowedView => allowedView;
    public float HotspotDisableDuration => hotspotDisableDuration;
}
