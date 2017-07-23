using System;
using UnityEngine;

public class TerminalInputHandler
{
    private Terminal terminal;
    private int touchCounter = 0;
    private float touchDelay = 1.5f;
    private float ellapsedTime = 0f;
    public TerminalInputHandler(Terminal t)
    {
        terminal = t;
    }
    public void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (MobileInput())
            {
                terminal.ToggleTerminal();
                return;
            }
        }
        if (Input.GetKeyDown("`"))
        {
            terminal.ToggleTerminal();
            return;
        }
        if (!terminal.displayTerminal) return;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            terminal.OnBackSpacePressed();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            terminal.OnTabPressed();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            terminal.OnEnterPressed();
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
            terminal.OnDownArrowPressed();
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            terminal.OnUpArrowPressed();
        else
        {
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
                terminal.UpdateInputText(Input.inputString);
        }
    }

    private bool MobileInput()
    {
        if (terminal.touchScreenKeyboard != null)
        {
            if (terminal.touchScreenKeyboard.done)
            {
                terminal.SetInputText(terminal.touchScreenKeyboard.text);
                terminal.OnEnterPressed();
                terminal.touchScreenKeyboard = null;
            }
        }
        if (Input.touchCount == terminal.mobileTouchCount)
        {
            ellapsedTime += Time.deltaTime;
        }
        if (ellapsedTime > touchDelay)
        {
            ellapsedTime = 0;
            if (terminal.displayTerminal)
            {
                terminal.DisplayTouchScreenKeyboard();
                return false;
            }
            return true;
        }
        else return false;
    }
}
