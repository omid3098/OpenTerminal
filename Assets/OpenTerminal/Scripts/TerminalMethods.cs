using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class TerminalMethods
{
    public List<MethodInfo> methods { get; private set; }
    private List<string> methodNames = new List<string>();
    public TerminalMethods()
    {
        methods = new List<MethodInfo>();
        var assembly = System.AppDomain.CurrentDomain.Load("Assembly-CSharp");
        methods = assembly
            .GetTypes()
            .SelectMany(x => x.GetMethods())
            .Where(y => y.GetCustomAttributes(true).OfType<CommandAttribute>().Any()).ToList();
        foreach (var method in methods)
        {
            foreach (var attribute in method.GetCustomAttributes(true))
            {
                if (attribute is CommandAttribute) //Does not pass
                {
                    CommandAttribute attr = (CommandAttribute)attribute;
                    methodNames.Add(attr.commandName);
                }
            }
        }
    }

    public string[] GetCommandsContaining(string input)
    {
        return methodNames.Where(k => k.Contains(input)).ToArray();
    }
}