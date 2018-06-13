using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerModePage : UIPage
{
    [SerializeField]
    protected Button _p1Button;
    [SerializeField]
    protected Button _p2Button;

    protected override void Awake()
    {
        base.Awake();
        Hide();
    }

    protected void Start()
    {
        _p1Button.onClick.AddListener(() =>
        {
            GameManager.instance.SetGameMode(GameMode.P1);
            GetComponentInParent<UIScreenMenu>().GoToNext();
        });

        _p2Button.onClick.AddListener(() =>
        {
            GameManager.instance.SetGameMode(GameMode.P2);
            GetComponentInParent<UIScreenMenu>().GoToNext();
        });

    }

    public override void Hide()
    {
        base.Hide();
        _p1Button.GetComponent<Animator>().SetTrigger("Normal");
        _p2Button.GetComponent<Animator>().SetTrigger("Normal");
    }

    public override void Show()
    {
        base.Show();
        _p1Button.GetComponent<Animator>().SetTrigger("Normal");
        _p2Button.GetComponent<Animator>().SetTrigger("Normal");
    }
}
