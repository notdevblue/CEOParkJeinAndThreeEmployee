using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    protected CanvasGroup cvs;

    protected virtual void Awake()
    {
        cvs = GetComponent<CanvasGroup>();

        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }

    protected virtual void Start()
    {
        
    }

    public virtual void Open()
    {
        cvs.alpha = 1f;
        cvs.blocksRaycasts = true;
        cvs.interactable = true;
    }

    public virtual void Close()
    {
        cvs.alpha = 0f;
        cvs.blocksRaycasts = false;
        cvs.interactable = false;
    }
}
