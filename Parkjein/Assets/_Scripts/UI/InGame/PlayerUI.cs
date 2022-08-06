using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Image playerFace; // ��ǻ� ���°�
    [SerializeField]
    private Image[] winImg;

    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private List<Image> iconList;

    [SerializeField]
    private Sprite defaultWinSprite;
    [SerializeField]
    private Sprite winSprite;

    public bool isLeft;

    public void Init(Sprite playerSprite = null)
    {
        if(playerSprite != null)
        {
            this.playerFace.sprite = playerSprite;
        }

        for (int i = 0; i < winImg.Length; i++)
        {
            winImg[i].sprite = defaultWinSprite;
        }

        for (int i = 0; i < iconList.Count; i++)
        {
            iconList[i].gameObject.SetActive(false);
        }

        SetHp(1f);
    }

    public void NewLoop() 
    {
        for (int i = 0; i < winImg.Length; i++)
        {
            winImg[i].sprite = defaultWinSprite;
        }

        SetHp(1f);
    }

    public void SetHp(float hpAmount,bool isDead = false)
    {
        if(isDead)
        {
            hpAmount = 1f;
            return;
        }

        hpSlider.value = hpAmount;
        Debug.LogError($"REQUEST: {hpAmount}, CURVALUE {hpSlider.value}");
    }

    public void SetWinImg(int win)
    {
        for (int i = 0; i < win; i++)
        {
            winImg[i].sprite = winSprite;
        }
        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.killSfx);
    }

    public void SetIcon(Sprite icon)
    {
        Image img = iconList.Find(x => !x.gameObject.activeSelf);

        img.sprite = icon;
        img.gameObject.SetActive(true);
    }
}
