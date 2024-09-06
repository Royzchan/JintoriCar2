using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour
{
    [SerializeField,Header("�O���[�X�P�[���p�̃}�e���A��")]
    Material _grayscaleMaterial;
    [SerializeField,Header("�X�^�����̉摜")]
    private Image _stunImage;
    [SerializeField,Header("�X�^�����̉摜��Y���W�ݒ�")]
    float _stunPosY;
    [SerializeField,Header("���b��ɃX�^�����邩")]
    private float _stunTime;

    //StunImage��z�u����Canvas
    private GameObject _canvas;
    //StunImage�̍��W�w��ŃJ�����̕`��͈͂���ɂ���
    Camera _camera;
    //��������rotateImage��ۑ�
    Image _image;

    Material _material;

    bool _enabled = false;

    private void Start()
    {
        StunImageInstantiate();
    }

    public void GreyScale()
    {
        if (_enabled)
        {
            _material = null;
            _enabled = !_enabled;
        }
        else
        {
            StartCoroutine(StartStun(_stunTime));
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_material != null)
        {
            //�}�e���A����K�p
            Graphics.Blit(src, dest, _material);
        }
        else
        {
            //�ʏ�̏��
            Graphics.Blit(src, dest);
        }
    }

    IEnumerator StartStun(float rotateTime)
    {
        //�X�^���摜��\��
        _image.gameObject.SetActive(true);

        //rotateTime�b�҂�
        yield return new WaitForSeconds(rotateTime);

        //�}�e���A����K�p
        _material = _grayscaleMaterial;
        _enabled = !_enabled;
        //�摜��\��
        _image.gameObject.SetActive(false);
    }

    //���߂ɃX�^�����̉摜�𐶐��A���W�w��
    void StunImageInstantiate()
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
        _image = Instantiate(_stunImage, _canvas.transform);
        // UI�v�f�̈ʒu��ݒ�
        _image.rectTransform.anchoredPosition = localPoint + new Vector2(0f, _stunPosY * 10);
        //�ŏ��͕\�����Ȃ���Ԃ���n�߂�
        _image.gameObject.SetActive(false);
    }
}
