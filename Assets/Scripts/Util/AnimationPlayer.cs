using UnityEngine;
using UnityEngine.UI;
using System;

public class AnimationPlayer : MonoBehaviour
{
    bool isPlaying = false;
    AnimationSequence sequence;
    GameObject targetObject;
    int currentFrame = 0;
    Image targetImage;
    Sprite endSprite;

    private bool hasStarted = false;

    void Start()
    {
        if (AnimSync.Instance != null)
        {
            AnimSync.Instance.UpdateFPS += AdvanceToNextFrame;
            hasStarted = true;
        }
    }
    void OnEnable()
    {
        if (AnimSync.Instance != null && hasStarted)
            AnimSync.Instance.UpdateFPS += AdvanceToNextFrame;
    }
    void Update()
    {
        HandleCurrentAnimation();
    }
    public void PlayAnimation(AnimationSequence sequence, GameObject target, Sprite endSprite = null, bool unSkippable = false)
    {
        this.endSprite = endSprite;
        currentFrame = 0;
        this.sequence = sequence;
        targetObject = target;
        targetImage = target.GetComponent<Image>();
        isPlaying = true;
        Debug.Log(gameObject.name + " is now playing an animation");
    }
    void StopAnimation()
    {
        if (endSprite != null)
        {
            targetImage.sprite = endSprite;
        }

        isPlaying = false;
        sequence = null;
        targetObject = null;
        targetImage = null;
        currentFrame = 0;
        Debug.Log(gameObject.name + " has stopped animating");
    }
    void HandleCurrentAnimation()

    {
        if (NotPlayingAnimation()) { return; }

        if (currentFrame >= sequence.frames.Length)
        {
            StopAnimation();
        }

    }
    private bool NotPlayingAnimation()
    {
        return !isPlaying || sequence == null || targetObject == null;
    }
    private void AdvanceToNextFrame(object sender, EventArgs e)
    {
        if (NotPlayingAnimation()) { return; }
        if (currentFrame >= sequence.frames.Length) { return; }

        targetImage.sprite = sequence.frames[currentFrame];
        currentFrame++;
    }

    void OnDestroy()
    {
        if (AnimSync.Instance != null)
        {
            AnimSync.Instance.UpdateFPS -= AdvanceToNextFrame;
        }
    }

}

