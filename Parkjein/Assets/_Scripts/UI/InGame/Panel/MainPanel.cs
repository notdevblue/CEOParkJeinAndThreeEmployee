using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Managers;

public class MainPanel : Panel
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public override void Open()
    {
        cvs.alpha = 1f;
    }

    public void SetScoreText(int left,int right)
    {
        scoreText.text = $"{left} : {right}";
        SoundManager.Instance.PlaySfxSound(SoundManager.Instance.killSfx);
    }
}
