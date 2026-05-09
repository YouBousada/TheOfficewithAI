using UnityEngine;

public class TimeGenerator
{
    public float WeibullTime(int AiLevel, CharacterProfile profile = null, float additionalMultiplier = 1f)
    {
        float u = Mathf.Clamp01(AiLevel / 20f);

        float k = Mathf.Lerp(1.2f, 3.0f, u);

        float median = Mathf.Lerp(30f, 5f, u);

        float scale = median / Mathf.Pow(Mathf.Log(2f), 1f / k);

        float x = Random.value;
        float t = scale * Mathf.Pow(-Mathf.Log(1f - x), 1f / k);
        return t * additionalMultiplier;
    }

    public virtual float ExponentialTime(int AiLevel, CharacterProfile profile = null, float additionalMultiplier = 1f)
    {
        float x = Random.value;

        float lambda = 0.00376f * AiLevel + 0.0676f;

        float t = -Mathf.Log(1f - x) / lambda;

        return t;

    }
}
