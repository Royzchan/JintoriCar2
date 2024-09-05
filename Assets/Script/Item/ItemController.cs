using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アイテムのタイプ
public enum ItemType
{
    Attack,//攻撃(減速)
    Bomb,//爆弾
    Camera_Rotate,//カメラ(回転)
    Stun,//スタン
    None
}

public class ItemController : MonoBehaviour
{
    [SerializeField, Header("アイテムのタイプ")]
    private ItemType _itemType;

    private ItemSpawn _itemSpown;

    void Update()
    {
        //常に回転
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //当たった相手がプレイヤーだった場合
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.transform.parent.GetComponent<CarController>();
            //プレイヤーにアイテムを持たす
            player.GetItem(_itemType);
            player.InkHeal();
            _itemSpown.GetItem();
            Destroy(this.gameObject);
        }
    }

    //アイテムスポーンをセット
    public void SetItemSpown(ItemSpawn itemSpawn)
    {
        _itemSpown = itemSpawn;
    }
}
