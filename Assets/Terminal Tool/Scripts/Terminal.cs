using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    private const string COMMAND_NOT_FOUND = "Command not found! type Help for list of available commands!";
    private TerminalConfig config;
    private bool displayTerminal = false;
    private string inputText = "";
    private string history = "";
    Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

    void Awake()
    {
        if (config == null) config = new CMD_Terminal();

        var assembly = System.AppDomain.CurrentDomain.Load("Assembly-CSharp");
        methods = assembly
            .GetTypes()
            .SelectMany(x => x.GetMethods())
            .Where(y => y.GetCustomAttributes(true).OfType<CommandAttribute>().Any())
            .ToDictionary(z => z.Name);
    }

    void OnGUI()
    {
        if (!displayTerminal) return;
        GUILayout.TextArea(history + consoleLine() + inputText);
    }

    private string consoleLine()
    {
        return (config.console + config.backslash + config.arrow + " ");
    }

    void Update()
    {
        CheckShowConsole();
        CheckBackSpace();
        InputHandler();
    }

    private void CheckBackSpace()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (inputText.Length > 1)
                inputText = inputText.Substring(0, inputText.Length - 1);
        }
    }

    private void CheckShowConsole()
    {
        if (Input.GetKeyDown("`")) displayTerminal = !displayTerminal;
    }

    private void InputHandler()
    {
        if (Input.GetKeyDown("`")) return;
        if (Input.GetKeyDown(KeyCode.Backspace)) return;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string result = ExecuteCommand(inputText);
            history += consoleLine() + inputText + "\n" + result + "\n";
            inputText = "";
            return;
        }
        inputText += Input.inputString;
    }

    private string ExecuteCommand(string inputText)
    {
        bool registered = false;
        string result = null;
        // Debug.Log(inputText);
        foreach (var method in methods)
        {
            var methodName = method.Key;
            var methodInfo = method.Value;
            foreach (object attribute in methodInfo.GetCustomAttributes(true)) // Returns all 3 of my attributes.
                if (attribute is CommandAttribute) //Does not pass
                {
                    CommandAttribute attribute1 = (CommandAttribute)attribute;
                    if (attribute1.commandName == inputText)
                    {
                        if (registered) Debug.LogError("Multiple commands are defined with: " + inputText);
                        Type type = (methodInfo.DeclaringType);
                        var instance_class = Activator.CreateInstance(type);
                        // Type instance_method = instance_class.GetType();
                        object[] obj = new object[] { "hello" };
                        result = (string)methodInfo.Invoke(instance_class, null);
                        registered = true;
                        break;
                    }
                }
        }
        if (!string.IsNullOrEmpty(result)) return result;
        if (registered) return null;
        return COMMAND_NOT_FOUND;
    }
}
