using System;
using UnityEngine;
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string commandName;
    public string commandDesc;

    public CommandAttribute(string name)
    {
        commandName = name;
    }
    public CommandAttribute(string name, string desc)
    {
        commandName = name;
        commandDesc = desc;
    }
}