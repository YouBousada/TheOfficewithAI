using UnityEngine;
using System;

public class Office : MonoBehaviour
{
    public static Office Instance { get; private set; }

    public float Power { get; private set; } = 1000f;
    public bool LeftDoorIsOpen { get; private set; }
    public bool RightDoorIsOpen { get; private set; }

    private float doorDrainagePerSecond = 1f;
    private bool invokedPowerOutage = false;

    public event Action<bool, bool> OnDoorStateChanged;
    public event Action OnLeftDoorClosed;
    public event Action OnLeftDoorOpened;
    public event Action OnRightDoorClosed;
    public event Action OnRightDoorOpened;
    public event Action OnPowerOutage;

    void Awake()
    {
        Instance = this;
        LeftDoorIsOpen = true;
        RightDoorIsOpen = true;
    }

    void Update()
    {
        if (Power < 0 && !invokedPowerOutage)
        {
            OnPowerOutage?.Invoke();
            invokedPowerOutage = true;
        }

        if (!LeftDoorIsOpen) Power -= Time.deltaTime * doorDrainagePerSecond;
        if (!RightDoorIsOpen) Power -= Time.deltaTime * doorDrainagePerSecond;
    }

    public void OpenLeftDoor()
    {
        if (LeftDoorIsOpen) return;
        LeftDoorIsOpen = true;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnLeftDoorOpened?.Invoke();
    }

    public void CloseLeftDoor()
    {
        if (!LeftDoorIsOpen) return;
        LeftDoorIsOpen = false;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnLeftDoorClosed?.Invoke();
    }

    public void OpenRightDoor()
    {
        if (RightDoorIsOpen) return;
        RightDoorIsOpen = true;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnRightDoorOpened?.Invoke();
    }

    public void CloseRightDoor()
    {
        if (!RightDoorIsOpen) return;
        RightDoorIsOpen = false;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnRightDoorClosed?.Invoke();
    }

    void OnGUI()
    {
        GUIStyle style = new(GUI.skin.label) { fontSize = 20 };
        style.normal.textColor = Color.green;
        GUI.Label(new Rect(10, 10, 300, 40), $"Power: {Power}", style);
        GUI.Label(new Rect(10, 50, 300, 40), $"Left Door: {LeftDoorIsOpen}", style);
        GUI.Label(new Rect(10, 90, 300, 40), $"Right Door: {RightDoorIsOpen}", style);
    }
}
