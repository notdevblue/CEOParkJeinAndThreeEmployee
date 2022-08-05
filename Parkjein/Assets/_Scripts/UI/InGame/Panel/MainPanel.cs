using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainPanel : Panel
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Sprite[] winImgs; // default - 0, win - 1

    public override void Open()
    {
        cvs.alpha = 1f;
    }

    public void SetScoreText(int left,int right)
    {
        scoreText.text = $"{left} : {right}";
    }
}
