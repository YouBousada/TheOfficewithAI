using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSequence", menuName = "Scriptable Objects/AnimationSequence")]
public class AnimationSequence : ScriptableObject
{
    public Sprite[] frames;
    public float fps = 12f;
}
