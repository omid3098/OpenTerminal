using UnityEngine;

public class BasicCommands
{

    [TerminalCommand("clear", "clears the terminal screen")]
    public void ClearTerminal()
    {
        Terminal.instance.Clear();
    }

    [TerminalCommand("help", "Shows list of available commands")]
    public string Help()
    {
        string help_string = "List of available commands:";
        foreach (var method in Terminal.instance.terminalMethods.methods)
        {
            foreach (var attribute in method.GetCustomAttributes(true))
            {
                if (attribute is TerminalCommandAttribute) //Does not pass
                {
                    TerminalCommandAttribute attr = (TerminalCommandAttribute)attribute;
                    help_string += "\n      " + attr.commandName + " --> " + attr.commandDesc;
                }
            }
        }
        return help_string;
    }

    [TerminalCommand("hide", "Hides the terminal")]
    public void Hide()
    {
        Terminal.instance.Hide();
    }
}
