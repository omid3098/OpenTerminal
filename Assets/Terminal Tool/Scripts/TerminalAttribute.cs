using System;
using UnityEngine;
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string commandName;

    public CommandAttribute(string _cn)
    {
        commandName = _cn;
    }
}