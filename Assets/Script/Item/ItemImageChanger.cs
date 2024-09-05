using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemImageChanger : MonoBehaviour
{

    [SerializeField, Header("透明な画像を入れておく")]
    private Sprite _clearImage;

    [SerializeField, Header("アイテムの画像をセット")]
    private Sprite[] _itemImage;

    //画像変更用
    private Image _image;

    [SerializeField, Header("二個目のアイテムの画像")]
    private Image _image2;

    [SerializeField, Header("アイテムの残り使用回数のテキスト")]
    private Text _itemUseNum;

    //プレイヤー
    private CarController _carController;

    //画像の更新をするか
    private bool _isUpdate = false;



    void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        //更新しないようなら早期リターン
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
