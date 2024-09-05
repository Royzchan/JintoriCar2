using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    public Transform carTransform;
    public Transform carTransform2;

    void Start()
    {
        // é‘Çèâä˙à íuÇ…ê›íË
        carTransform.position = new Vector3(30, -0.5f, 15);
        carTransform.rotation = Quaternion.Euler(0, 270, 0);
        // ÇQë‰ñ⁄
        carTransform2.position = new Vector3(-30, 0.2f, 5);
        carTransform2.rotation = Quaternion.Euler(0, 90, 0);

        var sequence = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();

        sequence.PrependInterval(2f)
            .Append(carTransform.DOMove(new Vector3(-30f, -0.5f, 5f), 4f))
            .Append(carTransform.DORotate(new Vector3(0,180),1f, RotateMode.WorldAxisAdd))
            .Join(carTransform.DOMove(new Vector3(-30f, -0.5f, 15f), 0.5f))
            .Append(carTransform.DOMove(new Vector3(30f, -0.5f, 5f), 4f)).SetLoops(-1, LoopType.Restart);

        sequence2.PrependInterval(4f)
            .Append(carTransform2.DOMove(new Vector3(30f, 0.2f, 15f), 4f))
            .Append(carTransform2.DORotate(new Vector3(0, 180), 1f, RotateMode.WorldAxisAdd))
            .Join(carTransform2.DOMove(new Vector3(30f, 0.2f, 5f), 0.5f))
            .Append(carTransform2.DOMove(new Vector3(-30f, 0.2f, 15f), 4f)).SetLoops(-1, LoopType.Restart);


    }
}
