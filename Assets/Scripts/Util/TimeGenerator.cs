using UnityEngine;

public class TimeGenerator
{
    private const float WeibullMinShape = 1.2f;
    private const float WeibullMaxShape = 3.0f;
    private const float WeibullMaxMedian = 30f;
    private const float WeibullMinMedian = 5f;
    private const float ExponentialLambdaScale = 0.00376f;
    private const float ExponentialLambdaBase = 0.0676f;

    public float WeibullTime(int AiLevel, CharacterProfile profile = null, float additionalMultiplier = 1f)
    {
        float u = Mathf.Clamp01(AiLevel / 20f);

        float k = Mathf.Lerp(WeibullMinShape, WeibullMaxShape, u);

        float median = Mathf.Lerp(WeibullMaxMedian, WeibullMinMedian, u);

        float scale = median / Mathf.Pow(Mathf.Log(2f), 1f / k);

        float x = Mathf.Clamp(Random.value, 0f, 0.9999f);
        float t = scale * Mathf.Pow(-Mathf.Log(1f - x), 1f / k);
        return t * additionalMultiplier;
    }

    public virtual float ExponentialTime(int AiLevel, CharacterProfile profile = null, float additionalMultiplier = 1f)
    {
        float x = Mathf.Clamp(Random.value, 0f, 0.9999f);

        float lambda = ExponentialLambdaScale * AiLevel + ExponentialLambdaBase;

        float t = -Mathf.Log(1f - x) / lambda;

        return t;

    }
}
