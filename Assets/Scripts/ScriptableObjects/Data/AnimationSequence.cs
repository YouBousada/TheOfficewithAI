using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSequence", menuName = "Scriptable Objects/AnimationSequence")]
public class AnimationSequence : ScriptableObject
{
    public Sprite[] frames;
}
