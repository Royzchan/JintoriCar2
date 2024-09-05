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
        // 車１台目
        if (collider.gameObject.tag == "Paint")
        {
            // 赤色に変更する
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        // 車２台目
        if (collider.gameObject.tag == "Paint2")
        {
            // 青色に変更する
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }

        // 車３台目
        if (collider.gameObject.tag == "Paint3")
        {
            // 緑色に変更する
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

        // 車４台目
        if (collider.gameObject.tag == "Paint4")
        {
            // 黄色に変更する
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
