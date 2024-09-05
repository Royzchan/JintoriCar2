using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tereport : MonoBehaviour
{
    [SerializeField, Header("�I�_")]
    public GameObject TP2;
    [SerializeField, Header("�Ȃ���Ƃ�")]
    public GameObject CurvePoint;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.root.gameObject.transform.
               DOLocalMove(new Vector3(CurvePoint.transform.position.x,
                                       CurvePoint.transform.position.y,
                                       CurvePoint.transform.position.z), 1f);

            other.gameObject.transform.root.gameObject.transform.
                DOLocalMove(new Vector3(TP2.transform.position.x,
                                        TP2.transform.position.y, 
                                        TP2.transform.position.z), 1f).SetDelay(1f);
        }

        Rigidbody hitRb = other.transform.root.gameObject.GetComponent<Rigidbody>();
        if (hitRb != null)
        {
            hitRb.velocity = Vector3.zero;
        }
    }
}
