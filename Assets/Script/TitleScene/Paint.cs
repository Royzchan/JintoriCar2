using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        // �ԂP���
        if (collider.gameObject.tag == "Paint")
        {
            // �ԐF�ɕύX����
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        // �ԂQ���
        if (collider.gameObject.tag == "Paint2")
        {
            // �F�ɕύX����
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        // �ԂR���
        if (collider.gameObject.tag == "Paint3")
        {
            // �ΐF�ɕύX����
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

        // �ԂS���
        if (collider.gameObject.tag == "Paint4")
        {
            // ���F�ɕύX����
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
