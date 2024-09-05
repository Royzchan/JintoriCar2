using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Rigidbody _rb;
    private BoxCollider _collider;

    [SerializeField, Header("������ꂽ���ɔ�ԗ�(��)")]
    private float _throwPower = 5.0f;

    [SerializeField, Header("������ꂽ���ɔ�ԗ�(��)")]
    private float _throwPowerUp = 3.0f;

    [SerializeField, Header("�����͈̔�")]
    private Vector3 _explosionRange = new Vector3(5f, 1.5f, 5);

    [SerializeField, Header("�����̃G�t�F�N�g")]
    private GameObject _explosionEffect;

    //�ŏ��ɒn�ʂɓ���������
    private bool _firstHit = false;

    private float _aliveTime = 0f;
    private float _lifeTime = 10.0f;

    private Material _playerColor;
    private CarController _carController;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        //���e��O�ɔ�΂�
        _rb.AddForce(transform.forward * _throwPower + transform.up * _throwPowerUp, ForceMode.Impulse);
    }

    private void Update()
    {
        _aliveTime += Time.deltaTime;
        if (_aliveTime >= _lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    //�������v���C���[�̐F���Z�b�g
    public void SetPlayerColor(CarController car, Material mat)
    {
        _carController = car;
        _playerColor = mat;
    }

    private void OnTriggerEnter(Collider other)
    {

        //�ǂɂԂ�������
        if (other.gameObject.CompareTag("Wall"))
        {
            _rb.velocity = new Vector3(_rb.velocity.x * -1f, _rb.velocity.y, _rb.velocity.z * -1f);
        }

        //�n�ʂɂԂ�������
        if (other.gameObject.CompareTag("MapTile"))
        {
            //����������(�����蔻����L����)
            _collider.size = _explosionRange;
            other.gameObject.GetComponent<Renderer>().material = _playerColor;
            if(!_firstHit)
            {
                Instantiate(_explosionEffect, transform.position, Quaternion.identity);
                _firstHit = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        //�n�ʂ��痣�ꂽ��
        if (other.gameObject.CompareTag("MapTile"))
        {
            other.gameObject.GetComponent<Renderer>().material = _playerColor;
            //����������
            Destroy(this.gameObject);
        }
    }
}
