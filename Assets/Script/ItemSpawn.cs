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

    //�A�C�e���R���g���[���[
    private ItemController _controller;

    //�A�C�e�������݂��Ă��邩
    private bool _onItem = false;

    private float _counter = 0f;

    private void Start()
    {
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        //�A�C�e�������݂��Ă����瑁�����^�[��
        if (_onItem) return;

        //�A�C�e�����X�|�[���ʒu�ɖ����ꍇ��
        //�J�E���g��ǉ����Ă���
        _counter += Time.deltaTime;
        //�A�C�e�������̃N�[���^�C���𒴂�����
        if(_counter > _gm.ItemSpawnTime) 
        {
            //�A�C�e���𐶐�
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
                //�A�C�e����1�����o�^����Ă��Ȃ������ꍇ�͂�����Œ�ŏo��
                if (ItemList.Count == 1)
                {
                    _controller =
                        Instantiate(ItemList[0], this.transform.position, Quaternion.identity)
                        .GetComponent<ItemController>();
                }
                //����ȊO�͊m��
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
            //�R���g���[���[�Ɏ��g���Z�b�g
            _controller.SetItemSpown(this);
            //�A�C�e������������Ă���悤��
            _onItem = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    //�A�C�e�������ꂽ���ɌĂ�
    public void GetItem()
    {
        //�A�C�e�����X�e�[�W�ɂ��锻���false��
        _onItem = false;
    }
}
