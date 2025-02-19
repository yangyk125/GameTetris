using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepeatEventFilter
{
    private float _RepeatDelay = 0.4f;
    private float _RepeatInterval = 0.04f;

    private bool _isPressDown = false;
    private float _timePressDown = 0f;
    private float _timeLastRepeat = 0f;

    public RepeatEventFilter(float delay, float interval)
    {
        _RepeatDelay = delay;
        _RepeatInterval = interval;
    }

    public bool RepeatFilter()
    {
        if (!_isPressDown)
        {
            _isPressDown = true;
            _timePressDown = Time.time;
            _timeLastRepeat = -1.0f;
            return true;
        }

        if (Time.time - _timePressDown > _RepeatDelay)
        {
            if (_timeLastRepeat < 0)
            {
                _timeLastRepeat = Time.time;
                return true;
            }
            else if (Time.time - _timeLastRepeat > _RepeatInterval)
            {
                _timeLastRepeat = Time.time;
                return true;
            }
        }

        return false;
    }

    public void FinishFilter()
    {
        _isPressDown = false;
        _timePressDown = -1.0f;
        _timeLastRepeat = -1.0f;
    }
}
