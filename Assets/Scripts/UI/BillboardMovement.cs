using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BillboardMovement : MonoBehaviour
{
    [SerializeField] public float MovementAmplitude;
    [SerializeField] public float MovementDuration;
    
    void Start()
    {
        Vector3 startPosition = transform.position + new Vector3(0f, -MovementAmplitude, 0f);

        DOTween.Sequence()
            .Append(transform.DOMove(startPosition, MovementDuration).SetEase(Ease.OutSine)).SetLoops(-1, LoopType.Yoyo);
    }
    
}
