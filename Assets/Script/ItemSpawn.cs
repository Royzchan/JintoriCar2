using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    private GameManager _gm;

    [SerializeField]
    List<GameObject> ItemList = new List<GameObject>();

    [SerializeField]
    List<GameObject> R_ItemList = new List<GameObject>();

    int ItemNum = 0;

    //アイテムコントローラー
    private ItemController _controller;

    //アイテムが存在しているか
    private bool _onItem = false;

    private float _counter = 0f;

    private void Start()
    {
        //ゲームマネージャーを取得
        _gm = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        //アイテムが存在していたら早期リターン
        if (_onItem) return;

        //アイテムがスポーン位置に無い場合は
        //カウントを追加していき
        _counter += Time.deltaTime;
        //アイテム生成のクールタイムを超えたら
        if(_counter > _gm.ItemSpawnTime) 
        {
            //アイテムを生成
            ItemRandomSpawn();
            _counter = 0f;
        }
    }

    public bool ItemRandomSpawn()
    {
        ItemNum = Random.Range(0, 120);

        int SpawnNum = Random.Range(0, 1);
        if (SpawnNum == 0)
        {
            if (ItemNum < 100)
            {
                //アイテムが1個しか登録されていなかった場合はそれを固定で出す
                if (ItemList.Count == 1)
                {
                    _controller =
                        Instantiate(ItemList[0], this.transform.position, Quaternion.identity)
                        .GetComponent<ItemController>();
                }
                //それ以外は確率
                else
                {
                    _controller =
                        Instantiate(ItemList[ItemNum % ItemList.Count], this.transform.position, Quaternion.identity)
                        .GetComponent<ItemController>();
                }
            }
            else
            {
                if (R_ItemList.Count == 1)
                {
                    _controller =
                        Instantiate(R_ItemList[0], this.transform.position, Quaternion.identity)
                        .GetComponent<ItemController>();
                }
                else
                {
                    _controller =
                        Instantiate(R_ItemList[ItemNum % R_ItemList.Count], this.transform.position, Quaternion.identity)
                        .GetComponent<ItemController>();
                }
            }
            //コントローラーに自身をセット
            _controller.SetItemSpown(this);
            //アイテムが生成されているように
            _onItem = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    //アイテムが取られた時に呼ぶ
    public void GetItem()
    {
        //アイテムがステージにある判定をfalseに
        _onItem = false;
    }
}
