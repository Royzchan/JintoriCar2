using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImageChanger : MonoBehaviour
{

    [SerializeField, Header("�����ȉ摜�����Ă���")]
    private Sprite _clearImage;

    [SerializeField, Header("�A�C�e���̉摜���Z�b�g")]
    private Sprite[] _itemImage;

    //�摜�ύX�p
    private Image _image;

    [SerializeField, Header("��ڂ̃A�C�e���̉摜")]
    private Image _image2;

    [SerializeField, Header("�A�C�e���̎c��g�p�񐔂̃e�L�X�g")]
    private Text _itemUseNum;

    //�v���C���[
    private CarController _carController;

    //�摜�̍X�V�����邩
    private bool _isUpdate = false;



    void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        //�X�V���Ȃ��悤�Ȃ瑁�����^�[��
        if (!_isUpdate) return;

        if (!_carController.Have1StItem())
        {
            _itemUseNum.gameObject.SetActive(false);
        }
        else
        {
            _itemUseNum.gameObject.SetActive(true);
        }

        if (_itemUseNum != null)
        {
            _itemUseNum.text = _carController.UseItemNum.ToString();
        }
        switch (_carController.Get1stItem())
        {
            case ItemType.None:
                _image.sprite = _clearImage;
                break;

            case ItemType.Attack:
                _image.sprite = _itemImage[(int)ItemType.Attack];
                break;

            case ItemType.Bomb:
                _image.sprite = _itemImage[1];
                break;

            case ItemType.Camera_Rotate:
                _image.sprite = _itemImage[2];
                break;

            case ItemType.Stun:
                _image.sprite = _itemImage[3];
                break;
        }

        switch (_carController.Get2ndItem())
        {
            case ItemType.None:
                _image2.sprite = _clearImage;
                break;

            case ItemType.Attack:
                _image2.sprite = _itemImage[(int)ItemType.Attack];
                break;

            case ItemType.Bomb:
                _image2.sprite = _itemImage[1];
                break;

            case ItemType.Camera_Rotate:
                _image2.sprite = _itemImage[2];
                break;

            case ItemType.Stun:
                _image.sprite = _itemImage[3];
                break;
        }

    }

    public void SetPlayer(CarController car)
    {
        _carController = car;
        _isUpdate = true;
    }
}
