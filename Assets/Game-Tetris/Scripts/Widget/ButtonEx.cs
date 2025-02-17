using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class ButtonEx : Selectable, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }


    [SerializeField]
    public float RepeatDelay = 0.3f;
    public float RepeatInterval = 0.03f;


    [FormerlySerializedAs("onRepeatClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnRepeatClick = new ButtonClickedEvent();
    public ButtonClickedEvent onRepeatClick
    {
        get { return m_OnRepeatClick; }
        set { m_OnRepeatClick = value; }
    }


    private float _timePressDown = 0f;
    private float _timeLastRepeat = 0f;

    private void FixedUpdate()
    {
        if (!_isPressDown)
            return;

        if (Time.time - _timePressDown > RepeatDelay)
        {
            if (_timeLastRepeat < 0)
            {
                _timeLastRepeat = Time.time;
                m_OnRepeatClick.Invoke();
            }
            else if (Time.time- _timeLastRepeat > RepeatInterval)
            {
                _timeLastRepeat = Time.time;
                m_OnRepeatClick.Invoke();
            }
        }
    }

    private bool _isPressDown = false;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        m_OnClick.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        DoStateTransition(SelectionState.Pressed, false);

        _isPressDown = true;
        _timePressDown = Time.time;

        m_OnRepeatClick.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        _isPressDown = false;

        _timePressDown = -1.0f;
        _timeLastRepeat = -1.0f;

        DoStateTransition(SelectionState.Normal, false);
    }
}
