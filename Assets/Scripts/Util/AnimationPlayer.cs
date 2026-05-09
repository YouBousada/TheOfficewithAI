using UnityEngine;
using UnityEngine.UI;

public class AnimationPlayer : MonoBehaviour
{
    float timer = 0f;
    bool isPlaying = false;
    AnimationSequence sequence;
    GameObject targetObject;
    int currentFrame = 0;
    Image targetImage;
    Sprite endSprite;
    
    void Update()
    {
        HandleCurrentAnimation();
    }
    public void PlayAnimation(AnimationSequence sequence, GameObject target, Sprite endSprite = null)
    {
        this.endSprite = endSprite;
        currentFrame = 0;
        this.sequence = sequence;
        targetObject = target;
        targetImage = target.GetComponent<Image>();
        timer = 0f;
        isPlaying = true;
    }
    void StopAnimation()
    {
        if(endSprite != null)
        {
            targetImage.sprite = endSprite;
        }

        isPlaying = false;
        sequence = null;
        targetObject = null;
        targetImage = null;
        currentFrame = 0;
    }
    void HandleCurrentAnimation()
    {
        if (NotPlayingAnimation()) { return; }

        if (currentFrame >= sequence.frames.Length)
        {
            StopAnimation();
            return;
        }

        AdvanceToNextFrame();
    }

    private bool NotPlayingAnimation()
    {
        return !isPlaying || sequence == null || targetObject == null;
    }

    private void AdvanceToNextFrame()
    {
        targetImage.sprite = sequence.frames[currentFrame];

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = 1f / sequence.fps;
            currentFrame++;
        }
    }
}
