using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HotSpot : MonoBehaviour
{
    [SerializeField] private GameContext gameContext;
    [SerializeField] private View to;
    [SerializeField] private Action action;
    [field: SerializeField] public float ThisActionTime { get; private set; }
    private bool started;

    public void OnEnable()
    {
        if (!started) { return; }

        Button button = GetComponent<Button>();
        // if (button == null) Debug.LogError($"{gameObject.name} is missing a Button component!", this);

        DoSubscriptions();
    }

    private void DoSubscriptions()
    {
        gameContext.Player.OnActionPerformed += HandlePlayerAction;
        gameContext.AIMonitor.OnDeath += HandleJumpscare;
    }

    public void Start()
    {
        started = true;
        DoSubscriptions();
    }

    public void InvokeViewChange(){
        gameContext.Player.SetView(to);
        // Debug.Log("Player set view to " + to);
    }
    public void InvokeAction(){
        gameContext.Player.PerformAction(action, ThisActionTime);
        // Debug.Log("action taken: " + action);
    }

    private void HandlePlayerAction(Action action, float time)
    {
        StartCoroutine(HandlePlayerActionCoroutine(action, time));
    }

    private IEnumerator HandlePlayerActionCoroutine(Action action, float time)
    {
        Button button = GetComponent<Button>();

        button.interactable = false;


        yield return new WaitForSeconds(time);

        button.interactable = true;
    }

    private void HandleJumpscare(CharacterName who, LocName from)
    {
        Button button = GetComponent<Button>();
        button.interactable = false;
    }
    

    public void OnDisable()
    {
        gameContext.Player.OnActionPerformed -= HandlePlayerAction;
        gameContext.AIMonitor.OnDeath -= HandleJumpscare;
    }

}
