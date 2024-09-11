using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    [SerializeField]
    GameObject[] _postProcessVolumes;
    [SerializeField,Header("点滅スピード")] private float _speed = 1;
    [SerializeField,Header("点滅が始まる残量")]
    float _startNum = 0;
    [SerializeField,Header("最大値")]
    float _maxIntensity = 0.6f;
    [SerializeField,Header("最小値")]
    float _minIntensity = 0.1f;

    //プレイヤー、カメラ取得用
    List<GameObject> _cars = new List<GameObject>();

    CarController[] _ccs;

    List<PostProcessLayer> _carCamera = new List<PostProcessLayer>();

    private Vignette[] _vignettes;

    //点滅時間
    private float[] _time;

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
        _time = new float[_ccs.Length];
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
            if (_ccs[i].Ink < _startNum)
            {
                // 内部時刻を経過させる
                _time[i] += Time.deltaTime * _speed;

                // 周期cycleで繰り返す波のアルファ値計算
                float _intensity = CosBetweenMinMax(_time[i] + Mathf.PI, _minIntensity, _maxIntensity);
                _vignettes[i].intensity.value = _intensity;
            }
            else
            {
                _vignettes[i].intensity.value = 0;
                _time[i] = 0;
            }
        }
    }

    float CosBetweenMinMax(float time, float min, float max)
    {
        // Mathf.Cos(time) は -1 から 1 の範囲で変化するので、まずその範囲を 0～1 に変換
        float cosValue = (Mathf.Cos(time) + 1) / 2;

        // 0～1 の範囲を min～max の範囲にスケール
        return Mathf.Lerp(min, max, cosValue);
    }
}
