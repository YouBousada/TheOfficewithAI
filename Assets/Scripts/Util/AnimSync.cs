using UnityEngine;
using System;

public class AnimSync : MonoBehaviour
{
    public static AnimSync Instance { get; private set; }

    float timer = 0f;

    void Awake()
    {
        Instance = this;
        timer = 1f / 12f;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 1f / 12f;
            UpdateFrame();
        }
    }

    public void UpdateFrame()
    {
        UpdateFPS?.Invoke(this, EventArgs.Empty);
        // Debug.Log("Emulated Frame update Invoked");
    }

    public event EventHandler UpdateFPS;
}