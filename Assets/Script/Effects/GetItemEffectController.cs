using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEffectController : MonoBehaviour
{
    GameManager _gm;

    void Start()
    {
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();
    }

    //�A�C�e�����擾�������ɌĂ�
    public void PlayEffect()
    {
        Instantiate(_gm.GetItemEffect, this.transform.position, Quaternion.identity);
    }
}
