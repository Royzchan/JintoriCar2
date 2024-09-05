using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Header("ズーム時のスピード")]
    private float _zoomSpeed = 0.5f;
    [SerializeField, Header("元の距離に戻るスピード")]
    private float _returnSpeed = 1f;
    [SerializeField, Header("初期位置に徐々に戻るスピード")]
    private float _initPosReturnSpeed = 0.05f;

    //初期値
    private Vector3 _localPos;
    Quaternion _rot;
    private float _distance;
    string _distanceFormatted;
    private float _zoomDistance;

    //Rayと壁が当たったポジション
    private Vector3 _wallHitPosition;

    //ズーム中か判定
    private bool _isZoom = false;

    //ズーム後元の位置に戻してもいいか
    private bool _isZoomReturn = true;

    //画面揺れ中か判定
    private bool _isCameraShake = false;

    // Start is called before the first frame update
    void Start()
    {
        //初期値
        _localPos = transform.localPosition;
        _rot = transform.localRotation;
        //プレイヤーとカメラの距離を計算
        _distance = Vector3.Distance(transform.parent.position, transform.position);
        //小数点第2位四捨五入
        _distanceFormatted = _distance.ToString("F1");
    }

    // Update is called once per frame
    void Update()
    {
        //壁があったら
        if (WallCheck())
        {
            //Rayが当たったポジションにカメラを移動
            transform.position = _wallHitPosition;
        }
        else
        {
            ReturnDis();
        }
        ////Debug用入力
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

    //プレイヤーで呼ぶように
    public void Shake()
    {
        StartCoroutine(CameraShake(0.25f, 0.1f));
    }

    public void Zoom()
    {
        StartCoroutine(CameraZoom(0.1f));
    }

    //カメラ操作（プレイヤーの周りを回転）
    //プラスマイナスで方向が変わる
    public void CameraOperation(float rotateSpeed)
    {
        transform.RotateAround(transform.parent.position, Vector3.up, rotateSpeed);
    }

    //カメラ回転（180度）
    //もう一度呼ぶともとに戻る
    public void CameraRotate()
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0f, 0f, 180f);
    }

    //カメラ揺れ（デフォルト値 : 0.25f, 0.1f）
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        //画面揺れ中か判定
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

        //画面揺れ前の位置に戻す
        transform.localPosition = _beforePos;
        transform.localRotation = _beforeRot;
        _isCameraShake = false;
    }

    //カメラズーム（目安 : 0.15f）
    public IEnumerator CameraZoom(float value)
    {
        //ズーム中にさらにズームしない
        if (_isZoom) yield break;

        float _zoomCount = 0;
        _isZoom = true;
        _isZoomReturn = false;

        while (_zoomCount < value)
        {
            _zoomCount += Time.deltaTime * _zoomSpeed;
            //プレイヤーの方向を計算
            Vector3 _dir = transform.parent.position - transform.position;
            _dir = Vector3.Normalize(_dir);
            //ズームする
            transform.position = transform.position + _dir * _zoomCount;
            //プレイヤーとカメラの距離を計算
            _zoomDistance = Vector3.Distance(transform.parent.position, transform.position);
            yield return null;
        }
    }

    //ズーム終了
    public void ZoomFinish()
    {
        _isZoomReturn = true;
    }

    //カメラ位置リセット
    public void Reset()
    {
        transform.localPosition = _localPos;
        transform.localRotation = _rot;
    }

    //壁に埋まらないようにチェック
    bool WallCheck()
    {
        RaycastHit _wallHit;

        //Rayを飛ばしてプレイヤーとカメラの間にオブジェクトがあるかどうか
        if (Physics.Raycast(transform.parent.position, transform.position - transform.parent.position, out _wallHit, Vector3.Distance(transform.parent.position, transform.position)))
        {
            //Rayが当たったオブジェクトが壁だったら
            if (_wallHit.collider.CompareTag("Wall"))
            {
                //当たったポジションを保存
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

    //距離が初期値と違うときに調整
    void ReturnDis()
    {
        if (_isZoomReturn)
        {
            //プレイヤーとカメラの距離を計算
            float _dis = Vector3.Distance(transform.parent.position, transform.position);
            //小数点第2位四捨五入
            string _disFormatted = _dis.ToString("F1");
            Vector3 _dir = transform.parent.position - transform.position;
            _dir = Vector3.Normalize(_dir);

            //最初の距離と違ったら戻す
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
            //プレイヤーとカメラの距離を計算
            float _dis = Vector3.Distance(transform.parent.position, transform.position);
            //小数点第2位四捨五入
            string _disFormatted = _dis.ToString("F1");
            string _zoomDisFormatted = _zoomDistance.ToString("F1");

            Vector3 _dir = transform.parent.position - transform.position;
            _dir = Vector3.Normalize(_dir);
                        
            //最初の距離と違ったら戻す
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

    //初期位置に徐々に戻る
    //前進中かを引数に入れる
    //Updateに書く
    public void InitPosReturn(bool isFront)
    {
        if (!isFront) return;

        //小数点第2位四捨五入
        string _localPosFormatted = _localPos.x.ToString("F1");
        string _transformLocalPositionFormatted = transform.localPosition.x.ToString("F1");

        //floatに直して比較
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
