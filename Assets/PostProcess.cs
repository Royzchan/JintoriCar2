using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcess : MonoBehaviour
{
    [SerializeField]
    GameObject[] _postProcessVolumes;
    [SerializeField,Header("�_�Ŏ���[s]")] private float _cycle = 1;
    [SerializeField,Header("�_�ł��n�܂�c��")]
    float startNum = 0;

    //�v���C���[�A�J�����擾�p
    List<GameObject> _cars = new List<GameObject>();

    CarController[] _ccs;

    List<PostProcessLayer> _carCamera = new List<PostProcessLayer>();

    private Vignette[] _vignettes;

    //�_�Ŏ���
    private double _time;

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
            if (_ccs[i].Ink < startNum)
            {
                // �����������o�߂�����
                _time += Time.deltaTime;

                // ����cycle�ŌJ��Ԃ��g�̃A���t�@�l�v�Z
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
