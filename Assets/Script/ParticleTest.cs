using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    public GameObject _g;
    public GameObject _b;
    private void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_g,_b.transform.position,Quaternion.identity);
        }
    }
}
