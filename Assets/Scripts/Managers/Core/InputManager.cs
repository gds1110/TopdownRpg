using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    public UnityEvent<bool> PressdEvent;
    bool _lPressed = false;
    float _lPressedTime = 0;  
    bool _rPressed = false;
    float _rPressedTime = 0;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
				KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_lPressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.LPointerDown);
                    _lPressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.LPress);
                _lPressed = true;
            }
            else
            {
                if (_lPressed)
                {
                    if (Time.time < _lPressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.LClick);
                    MouseAction.Invoke(Define.MouseEvent.LPointerUp);
                }
                _lPressed = false;
                _lPressedTime = 0;
            }
             if (Input.GetMouseButton(1))
            {
                if (!_rPressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.RPointerDown);
                    _rPressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.RPress);
                _rPressed = true;
            }
            else
            {
                if (_rPressed)
                {
                    if (Time.time < _rPressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.RClick);
                    MouseAction.Invoke(Define.MouseEvent.RPointerUp);
                }
                _rPressed = false;
                PressdEvent?.Invoke(_rPressed);
                _rPressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }

    public void Init()
    {
        PressdEvent = new UnityEvent<bool>();

    }
}
