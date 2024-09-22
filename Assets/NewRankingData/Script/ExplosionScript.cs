using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : RankingProcess
{
    [SerializeField,Header("爆発エフェクト")]
    GameObject _explosionObj;

    [SerializeField, Header("紙吹雪のエフェクト")]
    private GameObject _confetti;

    [SerializeField, Header("紙吹雪のエフェクトを出す場所")]
    private GameObject _confettiPos;

    [Header("爆発させる車")]
    public List<GameObject> _targetCars;
    public override bool UpdateProcess()
    {
        if(!_startRun)
        {
            if (FindObjectOfType<RankingManager>()._carObjs.Count == 0) return true;
            _targetCars.Add(FindObjectOfType<RankingManager>()._carObjs[1]);
            foreach(var car in _targetCars)
            {
                GameObject obj = Instantiate(_explosionObj, car.transform);
                Instantiate(_confetti, _confettiPos.transform);
                obj.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
                MeshRenderer[] meshRenderers = car.GetComponentsInChildren<MeshRenderer>();
                foreach(MeshRenderer mr in meshRenderers) 
                {
                    mr.enabled = false;
                }
            }
            _startRun = true;
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
