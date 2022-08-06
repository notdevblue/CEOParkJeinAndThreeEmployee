using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class DamageHue : MonoBehaviour
{
    private VolumeProfile _volume;
    private ColorAdjustments _colorAdj;

    public float effectDuration = 0.1f;
    public Color damagedColor;
    private Color _defaultColor;

    private float step;

    private void Awake()
    {
        _volume = GetComponent<VolumeProfile>();

        if (_volume.TryGet<ColorAdjustments>(out var adj)) {
            _colorAdj = adj;
            _defaultColor = _colorAdj.colorFilter.value;
        }
    }

    public void OnDamage(bool isme)
    {
        if (!isme) return;

        _colorAdj.colorFilter.value = damagedColor;

        Invoke(nameof(ReturnToDefault), effectDuration);
    }

    private void ReturnToDefault()
    {
        _colorAdj.colorFilter.value = _defaultColor;
    }
}