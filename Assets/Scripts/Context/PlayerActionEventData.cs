using System.Collections.Generic;

public struct PlayerActionEventData<T>
{
    public ActionType actionType;
    public T actionPayload;
}