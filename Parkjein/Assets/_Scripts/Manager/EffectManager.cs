using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect
{
    public string name;
    public ParticleSystem effectPrefab;
}


public class EffectManager : MonoSingleton<EffectManager>
{
    [SerializeField]
    private Effect[] effectList;

    public void PlayEffect(string effectName, Vector2 pos, Vector2 normal, Transform parent = null)
    {
        ParticleSystem effectPrefab = GetEffectPrefab(effectName);

        if (effectPrefab == null) return;

        ParticleSystem effect = Instantiate(effectPrefab, pos, Quaternion.LookRotation(normal));

        if (parent != null) effect.transform.SetParent(parent);

        effect.Play();
    }

    private ParticleSystem GetEffectPrefab(string effectName)
    {
        for (int i = 0; i < effectList.Length; i++)
        {
            if (effectList[i].name.Equals(effectName)) return effectList[i].effectPrefab;
        }

        return null;
    }
}
