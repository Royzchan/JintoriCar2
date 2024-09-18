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

    [SerializeField, Header("�J�[�g���ǂ̃^�C�v�ŃX�|�[�������邩")]
    private CartType _cartType;

    [SerializeField, Header("�J�[�g�̃^�C���̐F���ǂ����邩")]
    private CartColor _cartColor;

    [SerializeField,Header("�A�C�e���擾���̃G�t�F�N�g����������")]
    private GetItemEffectController _itemEffectController;

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
        _cartController.SetFirst(_cartType, _cartColor,_itemEffectController);
        //HP�Q�[�W�̍X�V��ݒ�
        _healthGauge.SetCar(_cartController);
        //�摜�̕ύX������X�N���v�g�J�[�g���Z�b�g
        _itemImageChanger.SetPlayer(_cartController);
    }
}
