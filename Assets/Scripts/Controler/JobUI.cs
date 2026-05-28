using TMPro;
using UnityEngine;

public class JobUI : MonoBehaviour
{
    [SerializeField] private CharacterName character;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private AIEvents aiMonitor;

    void Awake()
    {
        if (label == null)     Debug.LogWarning($"{name}: label is not assigned.", this);
        if (aiMonitor == null) Debug.LogWarning($"{name}: aiMonitor is not assigned.", this);

        if (aiMonitor == null) return;
        aiMonitor.OnInitialization += HandleInit;
        aiMonitor.OnJobSwitched += HandleJobSwitch;
    }

    void OnDestroy()
    {
        if (aiMonitor == null) return;

        aiMonitor.OnInitialization -= HandleInit;
        aiMonitor.OnJobSwitched -= HandleJobSwitch;
    }

    void HandleInit(CharacterName who, LocName loc, BehaviourName behaviour, JobName job)
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
