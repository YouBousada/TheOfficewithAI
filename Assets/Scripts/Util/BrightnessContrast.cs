using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BrightnessContrast : MonoBehaviour
{
    [Range(-1f, 1f)] public float brightness = 0f;
    [Range(0f, 3f)]  public float contrast   = 1f;

    RawImage _image;
    Material _mat;

    void Awake()
    {
        _image = GetComponent<RawImage>();
        _mat = new Material(Shader.Find("Custom/BrightnessContrast"));
        _image.material = _mat;
    }

    void Update()
    {
        _mat.SetFloat("_Brightness", brightness);
        _mat.SetFloat("_Contrast",   contrast);
    }

    void OnDestroy() => Destroy(_mat);
}
