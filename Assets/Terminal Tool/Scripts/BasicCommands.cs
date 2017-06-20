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
        string help_string = "  List of available commands:";
        var methods = Terminal.instance.GetMethods();
        foreach (var method in methods)
        {
            foreach (var attribute in method.GetCustomAttributes(true))
            {
                if (attribute is CommandAttribute) //Does not pass
                {
                    CommandAttribute attr = (CommandAttribute)attribute;
                    help_string += "\n" + attr.commandName + " --> " + attr.commandDesc;
                }
            }
        }
        Debug.Log(help_string);
        return help_string;
    }
}
