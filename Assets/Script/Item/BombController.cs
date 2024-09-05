using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    private Rigidbody _rb;
    private BoxCollider _collider;

    [SerializeField, Header("投げられた時に飛ぶ力(横)")]
    private float _throwPower = 5.0f;

    [SerializeField, Header("投げられた時に飛ぶ力(上)")]
    private float _throwPowerUp = 3.0f;

    [SerializeField, Header("爆発の範囲")]
    private Vector3 _explosionRange = new Vector3(5f, 1.5f, 5);

    [SerializeField, Header("爆発のエフェクト")]
    private GameObject _explosionEffect;

    //最初に地面に当たったか
    private bool _firstHit = false;

    private float _aliveTime = 0f;
    private float _lifeTime = 10.0f;

    private Material _playerColor;
    private CarController _carController;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        //爆弾を前に飛ばす
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

    //投げたプレイヤーの色をセット
    public void SetPlayerColor(CarController car, Material mat)
    {
        _carController = car;
        _playerColor = mat;
    }

    private void OnTriggerEnter(Collider other)
    {

        //壁にぶつかったら
        if (other.gameObject.CompareTag("Wall"))
        {
            _rb.velocity = new Vector3(_rb.velocity.x * -1f, _rb.velocity.y, _rb.velocity.z * -1f);
        }

        //地面にぶつかったら
        if (other.gameObject.CompareTag("MapTile"))
        {
            //爆発させる(あたり判定を広げる)
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

        //地面から離れたら
        if (other.gameObject.CompareTag("MapTile"))
        {
            other.gameObject.GetComponent<Renderer>().material = _playerColor;
            //自分を消す
            Destroy(this.gameObject);
        }
    }
}
