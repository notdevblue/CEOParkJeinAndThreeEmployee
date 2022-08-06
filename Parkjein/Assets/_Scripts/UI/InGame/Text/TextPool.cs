using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPool : MonoSingleton<TextPool>
{
    private List<DamageText> damageList = new List<DamageText>();

    [SerializeField]
    private DamageText damagePrefab;

    [SerializeField]
    private int initCount = 20;

    private void Start()
    {
        for (int i = 0; i < initCount; i++)
        {
            DamageText t = InstantiateDamageText();
            t.SetActive(false);
        }
    }

    private DamageText InstantiateDamageText()
    {
        DamageText t = Instantiate(damagePrefab, transform);
        damageList.Add(t);

        return t;
    }

    public DamageText GetDamageText()
    {
        DamageText text = damageList.Find(x => !x.gameObject.activeSelf);

        if(text == null)
        {
            text = InstantiateDamageText();
            text.SetActive(false);
        }

        return text;
    }
}
