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

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var method in assembly.GetTypes().SelectMany(x => x.GetMethods()).Where(y => y.GetCustomAttributes(true).OfType<TerminalCommandAttribute>().Any()).ToList())
            {
                methods.Add(method)
                foreach (var attribute in method.GetCustomAttributes(true))
                {
                    if (attribute is TerminalCommandAttribute) //Does not pass
                    {
                        TerminalCommandAttribute attr = (TerminalCommandAttribute)attribute;
                        methodNames.Add(attr.commandName);
                    }
                }
            }
        }
    }

    public string[] GetCommandsContaining(string input)
    {
        return methodNames.Where(k => k.Contains(input)).ToArray();
    }
}
