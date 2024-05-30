using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popup_Base : MonoBehaviour, IPopup
{
    private Button btn_Close;
    private Action act;

    protected virtual void Awake()
    {
        btn_Close = gameObject.Search<Button>(nameof(btn_Close));
        btn_Close?.onClick.AddListener(Close);
    }
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetData<T>(T t) where T : class
    {

    }

    public virtual void  SetAction(Action act)
    {
        this.act = act;
    }
}
