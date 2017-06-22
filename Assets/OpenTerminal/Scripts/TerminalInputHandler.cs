using UnityEngine;

public class TerminalInputHandler
{
    private Terminal terminal;
    public TerminalInputHandler(Terminal t)
    {
        terminal = t;
    }
    public void Update()
    {
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
            terminal.UpdateInputText(Input.inputString);
    }
}
