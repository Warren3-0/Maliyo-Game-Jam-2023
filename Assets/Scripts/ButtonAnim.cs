using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ButtonAnim : MonoBehaviour
{
    Sequence sequence;
    void Start()
    {
        sequence =DOTween.Sequence();
        sequence
            .Append(transform.DOScale(new Vector2(1.2f, 1.2f), 0.3f).SetDelay(0.3f))
            .Append(transform.DOScale(Vector2.one, 0.3f))
            .SetLoops(-1, LoopType.Restart);
    }
    public void StopAnimation()
    {
        sequence.Pause();
        }
}
