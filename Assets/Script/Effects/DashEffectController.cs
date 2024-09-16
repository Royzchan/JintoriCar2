using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffectController : MonoBehaviour
{
    [SerializeField,Header("生成されてから何秒後に消すか")]
    private float _deletTime = 3.0f;

    void Update()
    {
        _deletTime -= Time.deltaTime;
        if (_deletTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
