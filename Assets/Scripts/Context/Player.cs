using UnityEngine;

public class Player : MonoBehaviour
{
   public View currentView;
   public View startingView;
   public PlayerMonitor playerMonitor;
   public bool isDead = false;

    private void Awake()
    {
        SetNewView(startingView);
    }

    public void SetNewView(View newView)
    {
        currentView = newView;
        playerMonitor.HandleNewView(newView);
    }

    public void PerformAction(Action action)
    {
        playerMonitor.HandleAction(action);
    }
}
