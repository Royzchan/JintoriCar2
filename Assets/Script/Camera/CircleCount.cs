using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleCount : MonoBehaviour
{
    [SerializeField]
    Image _circleCountImage;

    [SerializeField]
    Text _circleCountText;

    [SerializeField, Header("�c�艽�b�ŊJ�n���邩")]
    int _actionTime;

    private GameManager _gm;

    bool _active = false;

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[���}�l�[�W���[���擾
        _gm = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[�����������琧�����Ԃ̃e�L�X�g���X�V
        if (_gm.IsPlaying)
        {
            if (_gm.TimeLimit > _actionTime) return;
            if (!_active)
            {
                _circleCountImage.gameObject.SetActive(true);
                _circleCountText.gameObject.SetActive(true);
                _active = true;
            }
            _circleCountText.text = Mathf.Ceil(_gm.TimeLimit).ToString();
            _circleCountImage.fillAmount = _gm.TimeLimit % 1.0f;
        }
    }
}
