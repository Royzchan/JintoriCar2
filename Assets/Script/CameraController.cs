using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("�Y�[�����̃X�s�[�h")]
    private float _zoomSpeed = 0.5f;
    [SerializeField, Header("���̋����ɖ߂�X�s�[�h")]
    private float _returnSpeed = 1f;
    [SerializeField, Header("�����ʒu�ɏ��X�ɖ߂�X�s�[�h")]
    private float _initPosReturnSpeed = 0.05f;

    //�����l
    private Vector3 _localPos;
    Quaternion _rot;
    private float _distance;
    string _distanceFormatted;
    private float _zoomDistance;

    //Ray�ƕǂ����������|�W�V����
    private Vector3 _wallHitPosition;

    //�Y�[����������
    private bool _isZoom = false;

    //�Y�[���㌳�̈ʒu�ɖ߂��Ă�������
    private bool _isZoomReturn = true;

    //��ʗh�ꒆ������
    private bool _isCameraShake = false;

    // Start is called before the first frame update
    void Start()
    {
        //�����l
        _localPos = transform.localPosition;
        _rot = transform.localRotation;
        //�v���C���[�ƃJ�����̋������v�Z
        _distance = Vector3.Distance(transform.parent.position, transform.position);
        //�����_��2�ʎl�̌ܓ�
        _distanceFormatted = _distance.ToString("F1");
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
        ////Debug�p����
        //if (Input.GetKey(KeyCode.Alpha1))
        //{
        //    CameraOperation(2f);
        //}
        //if (Input.GetKey(KeyCode.Alpha2))
        //{
        //    CameraOperation(-2f);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    CameraRotate();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    StartCoroutine(CameraShake(0.25f, 0.1f));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    StartCoroutine(CameraZoom(0.1f));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    ZoomFinish();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    Reset();
        //}
    }

    //�v���C���[�ŌĂԂ悤��
    public void Shake()
    {
        StartCoroutine(CameraShake(0.25f, 0.1f));
    }

    public void Zoom()
    {
        StartCoroutine(CameraZoom(0.1f));
    }

    //�J��������i�v���C���[�̎������]�j
    //�v���X�}�C�i�X�ŕ������ς��
    public void CameraOperation(float rotateSpeed)
    {
        transform.RotateAround(transform.parent.position, Vector3.up, rotateSpeed);
    }

    //�J������]�i180�x�j
    //������x�ĂԂƂ��Ƃɖ߂�
    public void CameraRotate()
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0f, 0f, 180f);
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

    //�J�����Y�[���i�ڈ� : 0.15f�j
    public IEnumerator CameraZoom(float value)
    {
        //�Y�[�����ɂ���ɃY�[�����Ȃ�
        if (_isZoom) yield break;

        float _zoomCount = 0;
        _isZoom = true;
        _isZoomReturn = false;

        while (_zoomCount < value)
        {
            _zoomCount += Time.deltaTime * _zoomSpeed;
            //�v���C���[�̕������v�Z
            Vector3 _dir = transform.parent.position - transform.position;
            _dir = Vector3.Normalize(_dir);
            //�Y�[������
            transform.position = transform.position + _dir * _zoomCount;
            //�v���C���[�ƃJ�����̋������v�Z
            _zoomDistance = Vector3.Distance(transform.parent.position, transform.position);
            yield return null;
        }
    }

    //�Y�[���I��
    public void ZoomFinish()
    {
        _isZoomReturn = true;
    }

    //�J�����ʒu���Z�b�g
    public void Reset()
    {
        transform.localPosition = _localPos;
        transform.localRotation = _rot;
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
        if (_isZoomReturn)
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
            }
            else if (float.Parse(_distanceFormatted) < float.Parse(_disFormatted))
            {
                transform.position += _dir * Time.deltaTime * _returnSpeed;
            }
            else
            {
                _isZoom = false;
            }
        }
        else
        {
            //�v���C���[�ƃJ�����̋������v�Z
            float _dis = Vector3.Distance(transform.parent.position, transform.position);
            //�����_��2�ʎl�̌ܓ�
            string _disFormatted = _dis.ToString("F1");
            string _zoomDisFormatted = _zoomDistance.ToString("F1");

            Vector3 _dir = transform.parent.position - transform.position;
            _dir = Vector3.Normalize(_dir);
                        
            //�ŏ��̋����ƈ������߂�
            if (float.Parse(_zoomDisFormatted) > float.Parse(_disFormatted))
            {
                transform.position -= _dir * Time.deltaTime * _returnSpeed;
            }
            else if (float.Parse(_zoomDisFormatted) < float.Parse(_disFormatted))
            {
                transform.position += _dir * Time.deltaTime * _returnSpeed;
            }
        }
    }

    //�����ʒu�ɏ��X�ɖ߂�
    //�O�i�����������ɓ����
    //Update�ɏ���
    public void InitPosReturn(bool isFront)
    {
        if (!isFront) return;

        //�����_��2�ʎl�̌ܓ�
        string _localPosFormatted = _localPos.x.ToString("F1");
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
}
