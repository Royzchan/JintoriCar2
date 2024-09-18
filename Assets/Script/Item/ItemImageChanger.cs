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

    [SerializeField, Header("�����̃A�C�e���̎g�p�񐔂�UI")]
    private GameObject[] _itemUseNumImage;

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
        
        if (!_isUpdate) return;

        switch (_carController.Get1stItem())
        {
            case ItemType.None:
                _image.sprite = _clearImage;
                _itemUseNumImage[0].SetActive(false);
                _itemUseNumImage[1].SetActive(false);
                _itemUseNumImage[2].SetActive(false);
                break;

            case ItemType.Attack:
                _image.sprite = _itemImage[(int)ItemType.Attack];
                switch (_carController.UseItemNum)
                {
                    case 1:
                        _itemUseNumImage[0].SetActive(false);
                        _itemUseNumImage[1].SetActive(false);
                        _itemUseNumImage[2].SetActive(true);
                        break;
                    case 2:
                        _itemUseNumImage[0].SetActive(false);
                        _itemUseNumImage[1].SetActive(true);
                        _itemUseNumImage[2].SetActive(true);
                        break;
                    case 3:
                        _itemUseNumImage[0].SetActive(true);
                        _itemUseNumImage[1].SetActive(true);
                        _itemUseNumImage[2].SetActive(true);
                        break;
                }
                break;

            case ItemType.Bomb:
                _image.sprite = _itemImage[1];
                _itemUseNumImage[0].SetActive(false);
                _itemUseNumImage[1].SetActive(false);
                _itemUseNumImage[2].SetActive(false);
                break;

            case ItemType.Camera_Rotate:
                _image.sprite = _itemImage[2];
                _itemUseNumImage[0].SetActive(false);
                _itemUseNumImage[1].SetActive(false);
                _itemUseNumImage[2].SetActive(false);
                break;

            case ItemType.Stun:
                _image.sprite = _itemImage[3];
                _itemUseNumImage[0].SetActive(false);
                _itemUseNumImage[1].SetActive(false);
                _itemUseNumImage[2].SetActive(false);
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
                _image2.sprite = _itemImage[3];
                break;
        }

    }

    public void SetPlayer(CarController car)
    {
        _carController = car;
        _isUpdate = true;
    }
}
