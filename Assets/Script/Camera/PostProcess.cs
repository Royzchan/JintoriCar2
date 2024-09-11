using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    [SerializeField]
    GameObject[] _postProcessVolumes;
    [SerializeField,Header("�_�ŃX�s�[�h")] private float _speed = 1;
    [SerializeField,Header("�_�ł��n�܂�c��")]
    float _startNum = 0;
    [SerializeField,Header("�ő�l")]
    float _maxIntensity = 0.6f;
    [SerializeField,Header("�ŏ��l")]
    float _minIntensity = 0.1f;

    //�v���C���[�A�J�����擾�p
    List<GameObject> _cars = new List<GameObject>();

    CarController[] _ccs;

    List<PostProcessLayer> _carCamera = new List<PostProcessLayer>();

    private Vignette[] _vignettes;

    //�_�Ŏ���
    private float[] _time;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        //������
        //�v���C���[�擾
        _ccs = FindObjectsOfType<CarController>();
        _vignettes = new Vignette[_postProcessVolumes.Length];
        _time = new float[_ccs.Length];
        foreach (CarController carController in _ccs)
        {
            _cars.Add(carController.gameObject);
        }
        //�v���C���[�̎q�̃J�������擾
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
                // �����������o�߂�����
                _time[i] += Time.deltaTime * _speed;

                // ����cycle�ŌJ��Ԃ��g�̃A���t�@�l�v�Z
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
        // Mathf.Cos(time) �� -1 ���� 1 �͈̔͂ŕω�����̂ŁA�܂����͈̔͂� 0�`1 �ɕϊ�
        float cosValue = (Mathf.Cos(time) + 1) / 2;

        // 0�`1 �͈̔͂� min�`max �͈̔͂ɃX�P�[��
        return Mathf.Lerp(min, max, cosValue);
    }
}
