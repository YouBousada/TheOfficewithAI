using UnityEngine;

[CreateAssetMenu(menuName = "Events/AI Events")]
public class AiEvents : ScriptableObject
{
    [SerializeField] private InitializationEvent InitEvent;
    [SerializeField] private BehaviourChangedEvent BehaveChangeEvent;
    [SerializeField] private JobSwitchedEvent JobSwitchEvent;
    [SerializeField] private LocationChangedEvent LocChangeEvent;
    [SerializeField] private JumpscareEvent JumpscareEvent;

    public InitializationEvent OnAnimatronicInitialize => InitEvent;
    public BehaviourChangedEvent OnBehaviourChanged => BehaveChangeEvent;
    public JobSwitchedEvent OnJobSwitched => JobSwitchEvent;
    public LocationChangedEvent OnLocationChanged => LocChangeEvent;
    public JumpscareEvent OnJumpscare => JumpscareEvent;
}