
using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Press Door Button")]
public class InteractWithEvironment : PlayerCommand
{
    [SerializeField] private EvironmentInteractionName interactionName;
    [SerializeField] private InteractWithEvironmentEvent interactWithOfficeEvent;
    public override void Execute()
    {
        interactWithOfficeEvent.Raise(interactionName);
    }
}