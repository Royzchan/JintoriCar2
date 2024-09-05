using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�A�C�e���̃^�C�v
public enum ItemType
{
    Attack,//�U��(����)
    Bomb,//���e
    Camera_Rotate,//�J����(��])
    Stun,//�X�^��
    None
}

public class ItemController : MonoBehaviour
{
    [SerializeField, Header("�A�C�e���̃^�C�v")]
    private ItemType _itemType;

    private ItemSpawn _itemSpown;

    void Update()
    {
        //��ɉ�]
        transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //�����������肪�v���C���[�������ꍇ
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.transform.parent.GetComponent<CarController>();
            //�v���C���[�ɃA�C�e����������
            player.GetItem(_itemType);
            player.InkHeal();
            _itemSpown.GetItem();
            Destroy(this.gameObject);
        }
    }

    //�A�C�e���X�|�[�����Z�b�g
    public void SetItemSpown(ItemSpawn itemSpawn)
    {
        _itemSpown = itemSpawn;
    }
}
