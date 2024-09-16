using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffectController : MonoBehaviour
{
    [SerializeField,Header("ê∂ê¨Ç≥ÇÍÇƒÇ©ÇÁâΩïbå„Ç…è¡Ç∑Ç©")]
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
