using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEffectController : MonoBehaviour
{
    GameManager _gm;

    GameObject _itemSub;

    void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();

        _itemSub = transform.GetChild(0).gameObject;
    }

    //アイテムを取得した時に呼ぶ
    public void PlayEffect()
    {
        Instantiate(_gm.GetItemEffect, this.transform.position, Quaternion.identity);
    }

    public void PlaySubEffect()
    {
        Instantiate(_gm.GetItemEffect, _itemSub.transform.position, Quaternion.identity);
    }
}
