﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 移動用スクリプト
    private CharController _move = default;
    private CharAction _action;

    [SerializeField, Tooltip("イベント用コライダー")]
    private EventCollider _event = default;

    // キャラクターの状態
    public enum CharState
    {
        NOMAL,  // 通常
        IVENT,   // 会話中
        MAX
    }
    private CharState _state;
   public CharState state
    {
        get { return _state; }
        set { _state = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.TryGetComponent(out EventCollider eventcollider))
            {
                _event = eventcollider;
                break;
            }
        }
        _move = gameObject.GetComponent<CharController>();
        _action = GetComponent<CharAction>();
        _state = CharState.NOMAL;
        _action.Play();
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case CharState.NOMAL:
                // 移動
                _move.Move();
                // アクション関係
                CheckAction();
                break;
            case CharState.IVENT:
                // イベント関係
                NowEvent();

                break;
            default:
                break;
        }
    }

    // イベントがあるか確認
    void CheckAction()
    {
        // アイテムの選択
        if (Input.GetAxis("ItemSelectKey") != 0)
        {
            ItemManager.Instans.SelectItem();
        }
        // アイテムの使用
        if (Input.GetButtonDown("Fire3"))
        {
            ItemManager.Instans.UseItem();
        }

        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.M))
        {
            _action.Play();
        }

        // スペースキーでイベント開始
        if (Input.GetButtonDown("Jump"))
        {
            // イベントがあるか確認しあればデータのセットをしtrueになる
            if (_event.CheckEvent())
            {
                // イベント中
                _state = CharState.IVENT;
            }
        }
    }

    void NowEvent()
    {
        // イベントを終了時にtrueを返しStateを戻す
        if(_event.Event.EventUpData())           
        {
            // イベントオブジェクトのアイコンの設定
            _event.SetEventObjIcon();

            _state = CharState.NOMAL;
        }  
    }
}
