using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerModePage : UIPage<PlayerModeSettings>
{
    [SerializeField]
    private Button _p1Button = null;
    [SerializeField]
    private Button _p2Button = null;
    [SerializeField]
    private RawImage _p1Picto = null;
    [SerializeField]
    private RawImage _p2Picto = null;

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

    public override void Init(PlayerModeSettings playerModeSettings)
    {
        base.Init(playerModeSettings);
        if (!String.IsNullOrEmpty(playerModeSettings.p1PictoPath.key) && TextureManager.instance.HasTexture(playerModeSettings.p1PictoPath.key))
            _p1Picto.texture = TextureManager.instance.GetTexture(playerModeSettings.p1PictoPath);
        if (!String.IsNullOrEmpty(playerModeSettings.p2PictoPath.key) && TextureManager.instance.HasTexture(playerModeSettings.p2PictoPath.key))
            _p2Picto.texture = TextureManager.instance.GetTexture(playerModeSettings.p2PictoPath);
    }
}
