using UnityEngine;
using System;

public class Office : MonoBehaviour
{
    public static Office Instance { get; private set; }

    public float Power { get; private set; } = 100f;
    public bool LeftDoorIsOpen { get; private set; }
    public bool RightDoorIsOpen { get; private set; }

    private float doorDrainagePerSecond = 1f;

    public event Action<bool, bool> OnDoorStateChanged;
    public event System.Action OnPowerOutage;

    private bool invokedPowerOutage = false;

    void OnGUI()
    {
        GUIStyle style = new(GUI.skin.label) { fontSize =  20 };
        style.normal.textColor = Color.green;

        GUI.Label(new Rect(10, 10, 300, 40), $"Power: {Power}", style);
        GUI.Label(new Rect(10, 50, 300, 40), $"Left Door is Open : {LeftDoorIsOpen}", style);
        GUI.Label(new Rect(10, 90, 300, 40), $"Right Door is Open: {RightDoorIsOpen}", style);
    }

    void Awake()
    {
        Instance = this;
        LeftDoorIsOpen = true;
        RightDoorIsOpen = true;
    }

    void Start()
    {
        Player.Instance.OnActionPerformed += HandleAction;
    }

    void Update()
    {
        if(Power < 0 && invokedPowerOutage == false)
        {
            OnPowerOutage?.Invoke();
            invokedPowerOutage = true;
        }

        if (!LeftDoorIsOpen)
        {
            Power -= Time.deltaTime * doorDrainagePerSecond;
        }
        if (!RightDoorIsOpen)
        {
            Power -= Time.deltaTime * doorDrainagePerSecond;
        }
    }

    void HandleAction(Action action)
    {
        if (action == Action.OpenLeftDoor) SetLeftDoor(true);
        else if (action == Action.CloseLeftDoor) SetLeftDoor(false);
        else if (action == Action.OpenRightDoor) SetRightDoor(true);
        else if (action == Action.CloseRightDoor) SetRightDoor(false);
    }

    public void SetLeftDoor(bool isOpen)
    {
        if (LeftDoorIsOpen == isOpen) return;
        LeftDoorIsOpen = isOpen;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
    }

    public void SetRightDoor(bool isOpen)
    {
        if (RightDoorIsOpen == isOpen) return;
        RightDoorIsOpen = isOpen;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
    }


    void OnDestroy()
    {
        Player.Instance.OnActionPerformed -= HandleAction;
    }
}
