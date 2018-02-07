using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITimerCtrl : MonoBehaviour
{
    protected Tweener _tweener;
    protected float _fDuration;
    protected float _fTargetVal;
    protected float _fCurrent;

    public Image imgTimer;

    public System.Action cbOver;

    public float Duration
    {
        get { return _fDuration; }
    }

    public void SetTimer(float time, System.Action cb = null, float initVal = 0, float tarVal = 0)
    {
        _fCurrent = _fDuration = time;
        cbOver = cb;
        if (imgTimer)
        {
            _fTargetVal = tarVal;
            imgTimer.fillAmount = initVal;
        }
    }

    public void StartTimer()
    {
        _tweener = DOTween.To(() => _fCurrent, p => _fCurrent = p, 0, _fDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (cbOver != null)
                cbOver.Invoke();
        });
        if (imgTimer)
            imgTimer.DOFillAmount(_fTargetVal, _fDuration).SetEase(Ease.Linear);
    }

    public void StopTimer(bool complete)
    {
        _tweener.OnComplete(() =>
        {
            if (cbOver != null)
                cbOver.Invoke();
        }).Kill(complete);
        if (imgTimer)
            imgTimer.DOKill(complete);
    }

    public float GetDecRatio()
    {
        return _fCurrent / _fDuration;
    }
}
