using System.Collections.Concurrent;
using HanSocket.Data;
using HanSocket.VO.InGame;
using Managers;
using UnityEngine;

namespace HanSocket.Handlers.InGame
{
    public class DamageHandler : HandlerBase
    {
        protected override string Type => "damage";

        ConcurrentQueue<DamageVO> vos
            = new ConcurrentQueue<DamageVO>();

        protected override void OnArrived(string payload)
        {
            vos.Enqueue(JsonUtility.FromJson<DamageVO>(payload));
        }

        protected override void OnFlag()
        {
        }

        private void Update()
        {
            while (vos.Count > 0) 
            {
                if (vos.TryDequeue(out var vo))
                {
                    GameObject obj = UserData.Instance.users[vo.id];
                    PlayerData data = obj.GetComponent<PlayerData>();
                    DamageText text = TextPool.Instance.GetDamageText();
                    bool isCritical = false;

                    data.MyUI.SetHp((float)vo.hp / vo.maxhp);
                    obj.GetComponent<PlayerAnimation>().SetHurt();
                    EffectManager.Instance.PlayEffect("hit", vo.point, Vector2.zero,null);

                    if(vo.id == WebSocketClient.Instance.id)
                    {
                        EffectManager.Instance.ShakeCamera(0.9f);
                    }

                    Debug.LogWarning($"AtkHP: {vo.atkhp}/{vo.atkmaxhp}, Damaged: {vo.id}, HP: {vo.hp}/{vo.maxhp}");
                    vo.specialCommands?.ForEach(x => {
                        switch (x.command)
                        {
                            case "knockout":
                                data.Knockout(x.param);
                                break;
                            case "skinofsteel":
                                SoundManager.Instance.PlaySfxSound(SoundManager.Instance.skinOfSteelSfx);
                                break;
                            case "critical":
                                isCritical = true;
                                SoundManager.Instance.PlaySfxSound(SoundManager.Instance.criticalSfx);
                                break;
                            default:
                                break;
                        }

                        Debug.LogWarning($"{x.command}:{x.param}");
                    });

                    text.Init($"{vo.damage}", isCritical ? Color.red : Color.white, vo.point);
                    text.SetActive(true);
                    SoundManager.Instance.PlayHit(!data.CanMove);
                }
            }
        }
    }
}