using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSet : MonoBehaviour
{
    [SerializeField, Header("���l�Ńv���C���邩")]
    private int _number = 0;

    [Header("�����͎l�l�o�^���Ă���")]
    [SerializeField, Header("�v���C���[�}�e���A��������")]
    private Material[] _playerMaterials;

    [SerializeField, Header("�v���C���[�}�e���A���ɑΉ����閼�O������")]
    private string[] _playerNames;

    //�����L���O���Z�b�g����֐�
    public void RankingSet()
    {
        //�����L���O�f�[�^�̕��ɉ��l�v���C�����Z�b�g
        RankingData._playerNum = _number;

        //�v���C���[�����N���A
        RankingData.playerDatas.Clear();

        //�z���MapTail�����I�u�W�F�N�g������
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("MapTile");

        //���Ԗڂ̃v���C���[�����m�F
        int playerCount = 0;
        //�}�e���A���𐔂���
        foreach (Material mat in _playerMaterials)
        {
            //�}�e���A���̐����v���C���[�̃}�e���A�����ɐ�����
            int count = 0;

            foreach (GameObject obj in taggedObjects)
            {
                Material objMat = obj.GetComponent<MeshRenderer>().material;
                if (objMat.color == mat.color)
                {
                    count++;
                }
            }

            //�����ŃZ�b�g
            //RankingData._playerScore.Add(count, mat);
            //�v���C���[�̖��O���ݒ肳��Ă����ꍇ
            if (_playerNames[playerCount] != "")
            {
                //RankingData.playerDatas.Add(new PlayerData(_playerNames[playerCount], count, mat));
                playerCount++;
            }
            else
            {
                //RankingData.playerDatas.Add(new PlayerData("����������", count, mat));
                playerCount++;
            }
        }
    }
}