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
    [Header("～スポーンする車の設定～")]
    [SerializeField, Header("車の最大HP")]
    private int _cartHP = 100;

    [SerializeField, Header("カートをどのタイプでスポーンさせるか")]
    private CartType _cartType;

    [SerializeField, Header("カートのタイルの色をどうするか")]
    private CartColor _cartColor;

    [Header("～システム関連の設定～")]

    [SerializeField, Header("カートのPrefab")]
    private GameObject _cart;

    private CarController _cartController;

    [SerializeField, Header("対応するHPのゲージをセット")]
    private HealthGauge _healthGauge;

    [SerializeField,Header("所持アイテム変更")]
    private ItemImageChanger _itemImageChanger;


    private void Awake()
    {
        Spawn();
    }

    //プレイヤーをスポーンさせる
    public void Spawn()
    {
        //クローンを生成して生成下オブジェクトのプレイヤーを登録
        _cartController = Instantiate(_cart, this.transform.position, this.transform.rotation).GetComponent<CarController>();
        //int型に変換して引数に登録
        _cartController.SetFirst(_cartHP, _cartType, _cartColor);
        //HPゲージの更新を設定
        _healthGauge.SetCar(_cartController);
        //画像の変更をするスクリプトカートをセット
        _itemImageChanger.SetPlayer(_cartController);
    }
}
