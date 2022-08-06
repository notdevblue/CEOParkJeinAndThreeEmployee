using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    [SerializeField]
    private CinemachineVirtualCamera vCam;

    private float timer = 0f;

    private void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;

            if(timer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cmPerlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cmPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    #region Cinemachine

    public void SetFollow(Transform trm)
    {
        vCam.Follow = trm;
    }

    public void ShakeCamera(float power = 0.5f, float duration = 0.2f)
    {
        CinemachineBasicMultiChannelPerlin cmPerlin = vCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        cmPerlin.m_AmplitudeGain = power;
        timer = duration;
    }

    #endregion

    public void PlayEffect(string effectName, Vector2 pos, Vector2 normal,bool isEnd = false, float time = 0.2f, Transform parent = null)
    {
        ParticleSystem effectPrefab = GetEffectPrefab(effectName);

        if (effectPrefab == null) return;

        ParticleSystem effect = Instantiate(effectPrefab, pos, Quaternion.LookRotation(normal));

        if (parent != null) effect.transform.SetParent(parent);

        effect.Play();

        if(isEnd)
        {
            StartCoroutine(EffectOff(effect,time));
        }
    }

    public void PlayEffect(string effectName, Vector2 pos, Vector2 normal, Transform parent = null)
    {
        ParticleSystem effectPrefab = GetEffectPrefab(effectName);

        if (effectPrefab == null) return;

        ParticleSystem effect = Instantiate(effectPrefab, pos, Quaternion.LookRotation(normal));

        if (parent != null) effect.transform.SetParent(parent);

        effect.Play();
    }

    IEnumerator EffectOff(ParticleSystem effect, float time)
    {
        yield return new WaitForSeconds(time);

        effect.Stop();
        effect.gameObject.SetActive(false);
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
