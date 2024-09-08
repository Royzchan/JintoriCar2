using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("���̋����ɖ߂�X�s�[�h")]
    private float _returnSpeed = 1f;
    [SerializeField, Header("�����ʒu�ɏ��X�ɖ߂�X�s�[�h")]
    private float _initPosReturnSpeed = 0.05f;
    [SerializeField,Header("�J�����������x��Ă��鑬�x")]
    private float _smoothSpeed = 0.125f;
    [SerializeField,Header("�x���傫��")]
    private float _lateValue = 0.3f;
    [SerializeField,Header("�㉺���]���ɕ\������摜")]
    private Image _rotateImage;
    [SerializeField,Header("���b��ɔ��]���邩")]
    private float _rotateTime;
    [SerializeField,Header("�㉺���]���̉摜��Y���W�ݒ�")]
    float _rotatePosY;

    //RotateImage��z�u����Canvas
    private GameObject _canvas;
    //RotateImage�̍��W�w��ŃJ�����̕`��͈͂���ɂ���
    Camera _camera;
    //��������rotateImage��ۑ�
    Image _image;

    //�㉺���]���邩
    bool _isFlipped = false;

    //�����l
    private Vector3 _initLocalPos;
    Quaternion _rot;
    private float _distance;
    string _distanceFormatted;

    private Vector3 _offset;// �I�t�Z�b�g�l

    //�J�����̈ʒu���ς��Ƃ��Ɋ�ƂȂ�ʒu��ۑ�
    private Vector3 _localPos;

    //Ray�ƕǂ����������|�W�V����
    private Vector3 _wallHitPosition;

    //��ʗh�ꒆ������
    private bool _isCameraShake = false;

    //���E�ړ�����
    bool _isLate = false;

    //���^�[������
    bool _isRetrun = false;

    // Start is called before the first frame update
    void Start()
    {
        //�����l
        _initLocalPos = transform.localPosition;
        _rot = transform.localRotation;
        //�v���C���[�ƃJ�����̋������v�Z
        _distance = Vector3.Distance(transform.parent.position, transform.position);
        //�����_��2�ʎl�̌ܓ�
        _distanceFormatted = _distance.ToString("F1");
        _localPos = transform.localPosition;

        RotateImageInstantiate();
    }

    // Update is called once per frame
    void Update()
    {
        //�ǂ���������
        if (WallCheck())
        {
            //Ray�����������|�W�V�����ɃJ�������ړ�
            transform.position = _wallHitPosition;
        }
        else
        {
            ReturnDis();
        }

        LateUpdate();
    }

    //�v���C���[�ŌĂԂ悤��
    public void Shake()
    {
        StartCoroutine(CameraShake(0.25f, 0.1f));
    }

    //�J��������i�v���C���[�̎������]�j
    //�v���X�}�C�i�X�ŕ������ς��
    public void CameraOperation(float rotateSpeed)
    {
        transform.RotateAround(transform.parent.position, Vector3.up, rotateSpeed);
        _localPos = transform.localPosition;
    }

    //�J������]�i180�x�j
    //������x�ĂԂƂ��Ƃɖ߂�
    public void CameraRotate()
    {
        if (_isFlipped)
        {
            _isFlipped = !_isFlipped;
        }
        else
        {
            StartCoroutine(StartRotate(_rotateTime));
        }
    }

    IEnumerator StartRotate(float rotateTime)
    {
        //�㉺���]��Image��\��
        _image.gameObject.SetActive(true);

        //rotateTime�b�҂�
        yield return new WaitForSeconds(rotateTime);

        //�㉺���]
        _isFlipped = !_isFlipped;
        //Image���\����
        _image.gameObject.SetActive(false);
    }

    //�J�����h��i�f�t�H���g�l : 0.25f, 0.1f�j
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        //��ʗh�ꒆ������
        if (_isCameraShake) yield break;

        var _beforePos = transform.localPosition;

        var _beforeRot = transform.localRotation;

        var _elapsed = 0f;

        _isCameraShake = true;

        while (_elapsed < duration)
        {
            var x = _beforePos.x + Random.Range(-1f, 1f) * magnitude;
            var y = _beforePos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, _beforePos.z);
            _elapsed += Time.deltaTime;

            yield return null;
        }

        //��ʗh��O�̈ʒu�ɖ߂�
        transform.localPosition = _beforePos;
        transform.localRotation = _beforeRot;
        _isCameraShake = false;
    }

    //�J�����ʒu���Z�b�g
    public void Reset()
    {
        transform.localPosition = _initLocalPos;
        transform.localRotation = _rot;
        _localPos = transform.localPosition;
    }

    //�ǂɖ��܂�Ȃ��悤�Ƀ`�F�b�N
    bool WallCheck()
    {
        RaycastHit _wallHit;

        //Ray���΂��ăv���C���[�ƃJ�����̊ԂɃI�u�W�F�N�g�����邩�ǂ���
        if (Physics.Raycast(transform.parent.position, transform.position - transform.parent.position, out _wallHit, Vector3.Distance(transform.parent.position, transform.position)))
        {
            //Ray�����������I�u�W�F�N�g���ǂ�������
            if (_wallHit.collider.CompareTag("Wall"))
            {
                //���������|�W�V������ۑ�
                _wallHitPosition = _wallHit.point;
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    //�����������l�ƈႤ�Ƃ��ɒ���
    void ReturnDis()
    {
        //�v���C���[�ƃJ�����̋������v�Z
        float _dis = Vector3.Distance(transform.parent.position, transform.position);
        //�����_��2�ʎl�̌ܓ�
        string _disFormatted = _dis.ToString("F1");
        Vector3 _dir = transform.parent.position - transform.position;
        _dir = Vector3.Normalize(_dir);

        //�ŏ��̋����ƈ������߂�
        if (float.Parse(_distanceFormatted) > float.Parse(_disFormatted))
        {
            transform.position -= _dir * Time.deltaTime * _returnSpeed;
            _localPos = transform.localPosition;
            _isRetrun = true;
        }
        else if (float.Parse(_distanceFormatted) < float.Parse(_disFormatted))
        {
            transform.position += _dir * Time.deltaTime * _returnSpeed;
            _localPos = transform.localPosition;
            _isRetrun = true;
        }
        else
        {
            _isRetrun = false;
        }
    }

    //�����ʒu�ɏ��X�ɖ߂�
    //�O�i�����������ɓ����
    //Update�ɏ���
    public void InitPosReturn(bool isFront)
    {
        if (!isFront) return;
        if (_isLate) return;

        //�����_��2�ʎl�̌ܓ�
        string _localPosFormatted = _initLocalPos.x.ToString("F1");
        string _transformLocalPositionFormatted = transform.localPosition.x.ToString("F1");

        //float�ɒ����Ĕ�r
        if (float.Parse(_transformLocalPositionFormatted) > float.Parse(_localPosFormatted))
        {
            CameraOperation(_initPosReturnSpeed);
        }
        else if (float.Parse(_transformLocalPositionFormatted) < float.Parse(_localPosFormatted))
        {
            CameraOperation(-_initPosReturnSpeed);
        }
    }

    void LateUpdate()
    {
        if (_isCameraShake) return;
        if (_isRetrun) return;
        Vector3 _desiredPosition = _localPos + _offset;

        // �J�����̈ʒu���X���[�Y�ɕ��
        Vector3 _smoothedPosition = Vector3.Lerp(transform.localPosition, _desiredPosition, _smoothSpeed);

        // �J�����̈ʒu���X�V
        transform.localPosition = _smoothedPosition;

        // �^�[�Q�b�g�̕������������߂̕����x�N�g�����v�Z
        Vector3 direction = transform.parent.position - transform.position;

        // ��]���v�Z
        Quaternion rotation = Quaternion.LookRotation(direction);

        // �㉺���]�����邩
        if (_isFlipped)
        {
            // X����180�x��]���ď㉺���]
            rotation *= Quaternion.Euler(0f, 0f, 180f);
        }

        // �v�Z������]��K�p
        transform.rotation = rotation;
    }

    public void Smooth(float value)
    {
        if(value == 0)
        {
            _isLate = false;
            _offset.x = 0;
            _offset.z = 0;
        }
        else if(value > 0)
        {
            _isLate = true;
            // �p�x�����W�A���ɕϊ�
            float _radians = transform.localEulerAngles.y * Mathf.Deg2Rad;
            _offset.x = Mathf.Cos(_radians) * _lateValue;
            _offset.z = -Mathf.Sin(_radians) * _lateValue;
        }
        else if (value < 0)
        {
            _isLate = true;
            // �p�x�����W�A���ɕϊ�
            float _radians = transform.localEulerAngles.y * Mathf.Deg2Rad;
            _offset.x = -Mathf.Cos(_radians) * _lateValue;
            _offset.z = Mathf.Sin(_radians) * _lateValue;
        }
    }

    //���߂ɏ㉺���]���̉摜�𐶐��A���W�w��
    void RotateImageInstantiate()
    {
        //�e�ƂȂ�Canvas��ݒ�
        _canvas = GameObject.Find("2PVerCanvas");
        //�J������ݒ�
        _camera = GetComponent<Camera>();

        // �J�����̒��S�����[���h���W�Ŏ擾
        Vector3 cameraCenterWorld = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _camera.nearClipPlane));

        // ���[���h���W���X�N���[�����W�ɕϊ�
        Vector2 screenPoint = _camera.WorldToScreenPoint(cameraCenterWorld);

        // �X�N���[�����W��UI�L�����o�X�̃��[�J�����W�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPoint, _canvas.GetComponent<Canvas>().worldCamera, out Vector2 localPoint);
        //����
        _image = Instantiate(_rotateImage, _canvas.transform);
        // UI�v�f�̈ʒu��ݒ�
        _image.rectTransform.anchoredPosition = localPoint + new Vector2(0f, _rotatePosY * 10);
        //�ŏ��͕\�����Ȃ���Ԃ���n�߂�
        _image.gameObject.SetActive(false);
    }
}
