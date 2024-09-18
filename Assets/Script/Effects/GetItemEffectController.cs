using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEffectController : MonoBehaviour
{
    GameManager _gm;

    void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
    }

    //アイテムを取得した時に呼ぶ
    public void PlayEffect()
    {
        Instantiate(_gm.GetItemEffect, this.transform.position, Quaternion.identity);
    }
}
