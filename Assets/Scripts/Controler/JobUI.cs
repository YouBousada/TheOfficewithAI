using TMPro;
using UnityEngine;

public class JobUI : MonoBehaviour
{
    [SerializeField] private CharacterName character;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private World world;

    void Awake()
    {
        world.aiMonitor.OnInitialization += HandleInit;
        world.aiMonitor.OnJobSwitched += HandleJobSwitch;
    }

    void OnDestroy()
    {
        if (world == null) return;

        world.aiMonitor.OnInitialization -= HandleInit;
        world.aiMonitor.OnJobSwitched -= HandleJobSwitch;
    }

    void HandleInit(CharacterName who, LocName loc, Behaviour behaviour, JobName job)
    {
        if (who != character) return;
        UpdateLabel(job);
    }

    void HandleJobSwitch(CharacterName who, JobName from, JobName to)
    {
        if (who != character) return;
        UpdateLabel(to);
    }

    void UpdateLabel(JobName job)
    {
        label.text = $"{character}'s: {job}";
    }
}
