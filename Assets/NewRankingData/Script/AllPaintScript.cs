using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AllPaintScript : RankingProcess
{
    //全てのタイル
    private GameObject[] _tiles = null;
    private float[] _distances = null;
    //車の位置
    [SerializeField,Header("車の位置")]
    public Transform _carPos;
    //現在の塗られている範囲
    private float _paintDis;
    [SerializeField, Header("色")]
    public Color _paintColor;
    [SerializeField,Header("1秒ごとに塗る範囲の速度")]
    private float _paintSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //タイルを取得
        Paint[] paints = FindObjectsOfType<Paint>();
        //GameObject型の配列に保存
        _tiles = paints.Select(x => x.gameObject).ToArray();
        //範囲を更新
        _distances = _tiles.Select(x => Vector3.Distance(x.transform.position,_carPos.position)).ToArray();
        
    }

    // Update is called once per frame
    public override bool UpdateProcess()
    {
        if(!_startRun)
        {
            if (FindObjectOfType<RankingManager>()._carObjs.Count == 0) return true;
            //1位の車の位置を取得
            _carPos = FindObjectOfType<RankingManager>()._carObjs[0].transform;
            //色の取得
            _paintColor = RankingData.GetRankData(0)._playerMat.color;
            _startRun = true;
        }
        _distances = _tiles.Select(x => Vector3.Distance(x.transform.position, _carPos.position)).ToArray();
        if (_tiles.Length != 0)
        {
            _paintDis += _paintSpeed * Time.deltaTime;
        }
        PaintTile();
        if (_distances.Max() < _paintDis)
        {
            return true;
        }
        return false;

    }

    private void PaintTile()
    {
        //_paintDisより
        var query = _distances.Select(x => x <= _paintDis).ToArray();
        for(int i = 0;i < query.Length;++i)
        {
            if (query[i])
            {
                _tiles[i].GetComponent<Renderer>().material.color = _paintColor;
            }
        }

    }
}
