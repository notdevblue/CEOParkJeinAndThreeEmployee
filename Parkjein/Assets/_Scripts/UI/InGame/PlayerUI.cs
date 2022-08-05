using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Image playerFace;
    [SerializeField]
    private Image[] winImg;

    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private List<Image> iconList;


    public void Init(Sprite playerSprite,Sprite defaultWinImg)
    {
        this.playerFace.sprite = playerSprite;

        for (int i = 0; i < winImg.Length; i++)
        {
            winImg[i].sprite = defaultWinImg;
        }

        for (int i = 0; i < iconList.Count; i++)
        {
            iconList[i].gameObject.SetActive(false);
        }

        SetHp(1f);
    }

    public void SetHp(float hpAmount)
    {
        hpSlider.value = hpAmount;
    }

    public void SetWinImg(Sprite winSprite, int win)
    {
        for (int i = 0; i < win; i++)
        {
            winImg[i].sprite = winSprite;
        }
    }

    public void SetIcon(Sprite icon)
    {
        Image img = iconList.Find(x => !x.gameObject.activeSelf);

        img.sprite = icon;
        img.gameObject.SetActive(true);
    }
}
