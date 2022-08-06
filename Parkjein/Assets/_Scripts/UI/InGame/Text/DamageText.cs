using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float alphaSpeed = 3f;
    [SerializeField]
    private float disableTime = 1f;

    [SerializeField]
    private TextMeshPro text;

    private Color alpha;

    private Coroutine co;

    private WaitForSeconds ws;

    private void Start()
    {
        alpha = text.color;

        ws = new WaitForSeconds(disableTime);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
    }

    private void OnDisable()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }
    }

    private void OnEnable()
    {
        if(co != null)
        {
            StopCoroutine(co);
        }
        StartCoroutine(DisableObj());
    }

    public void Init(string msg, Color color,Vector2 pos)
    {
        text.text = msg;
        text.color = color;
        transform.position = pos;
    }

    IEnumerator DisableObj()
    {
        yield return ws;

        SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
