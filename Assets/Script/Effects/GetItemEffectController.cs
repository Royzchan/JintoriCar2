using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEffectController : MonoBehaviour
{
    GameManager _gm;

    GameObject _itemSub;

    void Start()
    {
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();

        _itemSub = transform.GetChild(0).gameObject;
    }

    //�A�C�e�����擾�������ɌĂ�
    public void PlayEffect()
    {
        Instantiate(_gm.GetItemEffect, this.transform.position, Quaternion.identity);
    }

    public void PlaySubEffect()
    {
        Instantiate(_gm.GetItemEffect, _itemSub.transform.position, Quaternion.identity);
    }
}
