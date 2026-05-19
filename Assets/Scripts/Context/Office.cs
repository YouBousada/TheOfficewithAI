using UnityEngine;
using System;

public class Office : MonoBehaviour
{
    public float Power { get; private set; } = 100f;
    public bool LeftDoorIsOpen { get; private set; }
    public bool RightDoorIsOpen { get; private set; }

    private float doorDrainagePerSecond = 0.1f;

    public event Action<bool, bool> OnDoorStateChanged;

    void Awake()
    {
        LeftDoorIsOpen = true;
        RightDoorIsOpen = true;
    }

    void Start()
    {
        Player.Instance.OnActionPerformed += HandleAction;
    }

    void Update()
    {
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
