using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    [SerializeField]
    GameObject[] _postProcessVolumes;
    [SerializeField,Header("点滅周期[s]")] private float _cycle = 1;
    [SerializeField,Header("点滅が始まる残量")]
    float startNum = 0;

    //プレイヤー、カメラ取得用
    List<GameObject> _cars = new List<GameObject>();

    CarController[] _ccs;

    List<PostProcessLayer> _carCamera = new List<PostProcessLayer>();

    private Vignette[] _vignettes;

    //点滅時間
    private double _time;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        //初期化
        //プレイヤー取得
        _ccs = FindObjectsOfType<CarController>();
        _vignettes = new Vignette[_postProcessVolumes.Length];
        foreach (CarController carController in _ccs)
        {
            _cars.Add(carController.gameObject);
        }
        //プレイヤーの子のカメラを取得
        for (int i = 0; i < _cars.Count; ++i)
        {
            _carCamera.Add(_cars[i].transform.Find("CarCamera").GetComponent<Camera>().GetComponent<PostProcessLayer>());
            _carCamera[i].volumeLayer.value = LayerMask.GetMask(LayerMask.LayerToName(_postProcessVolumes[i].layer));
            _postProcessVolumes[i].GetComponent<PostProcessVolume>().profile.TryGetSettings<Vignette>(out _vignettes[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HpGaugeALittle();
    }

    void HpGaugeALittle()
    {
        for (int i = 0; i < _ccs.Length; ++i)
        {
            if (_ccs[i].Ink < startNum)
            {
                // 内部時刻を経過させる
                _time += Time.deltaTime;

                // 周期cycleで繰り返す波のアルファ値計算
                float _color = Mathf.Cos((float)(2 * Mathf.PI * _time / _cycle)) * 0.5f + 0.5f;
                _vignettes[i].color.value = new Color(1f, _color, _color,1f);
            }
            else
            {
                _vignettes[i].color.value = new Color(1f, 1f, 1f, 1f);
            }
        }
    }
}
