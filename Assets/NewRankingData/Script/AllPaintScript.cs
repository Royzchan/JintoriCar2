using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AllPaintScript : RankingProcess
{
    //�S�Ẵ^�C��
    private GameObject[] _tiles = null;
    private float[] _distances = null;
    //�Ԃ̈ʒu
    [SerializeField,Header("�Ԃ̈ʒu")]
    public Transform _carPos;
    //���݂̓h���Ă���͈�
    private float _paintDis;
    [SerializeField, Header("�F")]
    public Color _paintColor;
    [SerializeField,Header("1�b���Ƃɓh��͈͂̑��x")]
    private float _paintSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //�^�C�����擾
        Paint[] paints = FindObjectsOfType<Paint>();
        //GameObject�^�̔z��ɕۑ�
        _tiles = paints.Select(x => x.gameObject).ToArray();
        //�͈͂��X�V
        _distances = _tiles.Select(x => Vector3.Distance(x.transform.position,_carPos.position)).ToArray();
        
    }

    // Update is called once per frame
    public override bool UpdateProcess()
    {
        if(!_startRun)
        {
            if (FindObjectOfType<RankingManager>()._carObjs.Count == 0) return true;
            //1�ʂ̎Ԃ̈ʒu���擾
            _carPos = FindObjectOfType<RankingManager>()._carObjs[0].transform;
            //�F�̎擾
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
        //_paintDis���
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
