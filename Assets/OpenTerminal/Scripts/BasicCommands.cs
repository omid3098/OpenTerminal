using UnityEngine;

public class BasicCommands
{

    [Command("clear", "clears the terminal screen")]
    public void ClearTerminal()
    {
        Terminal.instance.Clear();
    }

    [Command("help", "Shows list of available commands")]
    public string Help()
    {
        string help_string = "List of available commands:";
        foreach (var method in Terminal.instance.terminalMethods.methods)
        {
            foreach (var attribute in method.GetCustomAttributes(true))
            {
                if (attribute is CommandAttribute) //Does not pass
                {
                    CommandAttribute attr = (CommandAttribute)attribute;
                    help_string += "\n      " + attr.commandName + " --> " + attr.commandDesc;
                }
            }
        }
        return help_string;
    }

    [Command("hide", "Hides the terminal")]
    public void Hide()
    {
        Terminal.instance.Hide();
    }
}
