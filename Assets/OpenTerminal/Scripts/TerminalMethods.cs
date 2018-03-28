using System;
using UnityEngine;
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

        MonoBehaviour[] sceneActive = GameObject.FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour mono in sceneActive)
        {
            Type monoType = mono.GetType();

            // Retreive the fields from the mono instance
            MethodInfo[] methodFields = monoType.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            // search all fields and find the attribute
            for (int i = 0; i < methodFields.Length; i++)
            {
                TerminalCommandAttribute attribute = Attribute.GetCustomAttribute(methodFields[i], typeof(TerminalCommandAttribute)) as TerminalCommandAttribute;

                // if we detect any attribute print out the data.
                if (attribute != null)
                {
                    methodNames.Add(attribute.commandName);
                    methods.Add(methodFields[i]);
                }
            }
        }

    }

    public string[] GetCommandsContaining(string input)
    {
        return methodNames.Where(k => k.Contains(input)).ToArray();
    }
}
