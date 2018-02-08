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

    
    public int nRemain
    {
        get { return (int)_fCurrent; }
    }

    public string strTimeRemain
    {
        get
        {
            int h = nRemain / 3600;
            int m = nRemain / 60 - h * 60 ;
            int s = nRemain - m - m * h ;

            string t = null;

            if(h>0)
            {
                t = h + "h."+ m +"m";
            }
            else if (m > 0)
            {
                t = m + "m." + s + "s";
            }
            else if (s > 0)
            {
                t = s + "s";
            }

            return t;
        }
    }


    public float Duration
    {
        get { return _fDuration; }
    }

    public void SetTimer(float time, System.Action cb = null, float initVal = 0, float tarVal = 1)
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

		_fDuration = 0;

        if (imgTimer)
            imgTimer.DOKill(complete);
    }

    public float GetDecRatio()
    {
        return _fCurrent / _fDuration;
    }
}
