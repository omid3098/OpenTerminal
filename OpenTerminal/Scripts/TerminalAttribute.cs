using System;
using UnityEngine;
[AttributeUsage(AttributeTargets.Method)]
public class TerminalCommandAttribute : Attribute
{
    public string commandName;
    public string commandDesc;

    public TerminalCommandAttribute(string name)
    {
        commandName = name;
    }
    public TerminalCommandAttribute(string name, string desc)
    {
        commandName = name;
        commandDesc = desc;
    }
}