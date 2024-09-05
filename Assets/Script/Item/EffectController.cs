using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    private float _deleteTime = 3.0f;

    void Update()
    {
        _deleteTime -= Time.deltaTime;

        if(_deleteTime<=0)
        {
            Destroy(this.gameObject);
        }
    }
}
