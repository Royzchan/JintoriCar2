using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CartType
{
    truck,
    old
}

public enum CartColor
{
    red,
    blue,
    green,
    purple
}

public class PlayerSpawn : MonoBehaviour
{
    [Header("�`�X�|�[������Ԃ̐ݒ�`")]
    [SerializeField, Header("�Ԃ̍ő�HP")]
    private int _cartHP = 100;

    [SerializeField, Header("�J�[�g���ǂ̃^�C�v�ŃX�|�[�������邩")]
    private CartType _cartType;

    [SerializeField, Header("�J�[�g�̃^�C���̐F���ǂ����邩")]
    private CartColor _cartColor;

    [Header("�`�V�X�e���֘A�̐ݒ�`")]

    [SerializeField, Header("�J�[�g��Prefab")]
    private GameObject _cart;

    private CarController _cartController;

    [SerializeField, Header("�Ή�����HP�̃Q�[�W���Z�b�g")]
    private HealthGauge _healthGauge;

    [SerializeField,Header("�����A�C�e���ύX")]
    private ItemImageChanger _itemImageChanger;


    private void Awake()
    {
        Spawn();
    }

    //�v���C���[���X�|�[��������
    public void Spawn()
    {
        //�N���[���𐶐����Đ������I�u�W�F�N�g�̃v���C���[��o�^
        _cartController = Instantiate(_cart, this.transform.position, this.transform.rotation).GetComponent<CarController>();
        //int�^�ɕϊ����Ĉ����ɓo�^
        _cartController.SetFirst(_cartHP, _cartType, _cartColor);
        //HP�Q�[�W�̍X�V��ݒ�
        _healthGauge.SetCar(_cartController);
        //�摜�̕ύX������X�N���v�g�J�[�g���Z�b�g
        _itemImageChanger.SetPlayer(_cartController);
    }
}
