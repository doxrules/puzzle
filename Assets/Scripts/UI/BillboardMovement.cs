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
        Vector3 endPosition = transform.position + new Vector3(0f, +MovementAmplitude, 0f);
        
        DOTween.Sequence()
            .Append(transform.DOMove(startPosition, MovementDuration).SetEase(Ease.OutSine)).SetLoops(-1, LoopType.Yoyo);
            //.Append(transform.DOMove(endPosition, MovementDuration).SetEase(Ease.OutBack)).SetLoops(-1, LoopType.Yoyo)
            //.SetAutoKill(false);
        //transform.DOLocalMove(new Vector3(0f, MovementAmplitude, 0f), MovementAmplitude ).SetLoops(-1, LoopType.Yoyo);
        //transform.DOJump(new Vector3(0f, MovementAmplitude, 0f), MovementDuration, -1, MovementDuration);
    }
    
}
