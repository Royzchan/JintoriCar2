using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSet : MonoBehaviour
{
    [SerializeField, Header("何人でプレイするか")]
    private int _number = 0;

    [Header("ここは四人登録しておく")]
    [SerializeField, Header("プレイヤーマテリアルを入れる")]
    private Material[] _playerMaterials;

    [SerializeField, Header("プレイヤーマテリアルに対応する名前を入れる")]
    private string[] _playerNames;

    //ランキングをセットする関数
    public void RankingSet()
    {
        //ランキングデータの方に何人プレイかをセット
        RankingData._playerNum = _number;

        //プレイヤー情報をクリア
        RankingData.playerDatas.Clear();

        //配列にMapTailを持つオブジェクトを入れる
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("MapTile");

        //何番目のプレイヤーかを確認
        int playerCount = 0;
        //マテリアルを数える
        foreach (Material mat in _playerMaterials)
        {
            //マテリアルの数をプレイヤーのマテリアル事に数える
            int count = 0;

            foreach (GameObject obj in taggedObjects)
            {
                Material objMat = obj.GetComponent<MeshRenderer>().material;
                if (objMat.color == mat.color)
                {
                    count++;
                }
            }

            //ここでセット
            //RankingData._playerScore.Add(count, mat);
            //プレイヤーの名前が設定されていた場合
            if (_playerNames[playerCount] != "")
            {
                //RankingData.playerDatas.Add(new PlayerData(_playerNames[playerCount], count, mat));
                playerCount++;
            }
            else
            {
                //RankingData.playerDatas.Add(new PlayerData("名無しさん", count, mat));
                playerCount++;
            }
        }
    }
}