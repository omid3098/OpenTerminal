using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    private const string COMMAND_NOT_FOUND = "Command not found! type \"help\" for list of available commands!";
    [SerializeField] private TerminalConfig config;
    [SerializeField] private bool displayTerminal = false;
    private string inputText = "";
    private string history = "";
    Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();
    private GUIStyle terminalStyle;
    public static Terminal instance;
    void Awake()
    {
        instance = this;
        if (config == null) config = Resources.Load<TerminalConfig>("Config/ZSH");
        terminalStyle = new GUIStyle();
        terminalStyle.font = config.font;
        terminalStyle.fontSize = 16;
        terminalStyle.normal.textColor = config.commandColor;
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
        // GUI.color = Color.white;
        // GUI.contentColor = Color.red;
        GUILayout.Label(history + consoleLine() + inputText, terminalStyle);
    }

    private string consoleLine()
    {
        return (config.console + config.backslash + config.arrow + " ");
    }

    void Update()
    {
        InputHandler();
    }

    private void InputHandler()
    {
        if (Input.GetKeyDown("`"))
        {
            displayTerminal = !displayTerminal;
            return;
        }
        if (!displayTerminal) return;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (inputText.Length >= 1) inputText = inputText.Substring(0, inputText.Length - 1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inputText += "    ";
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string result = ExecuteCommand(inputText);
            history += consoleLine() + inputText + "\n" + (!string.IsNullOrEmpty(result) ? (result + "\n") : "");
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
                        if (type.IsSubclassOf(typeof(UnityEngine.Object)))
                        {
                            var instance_classes = GameObject.FindObjectsOfType(type);
                            if (instance_classes != null)
                            {
                                foreach (var instance_class in instance_classes)
                                {
                                    object[] obj = new object[] { "hello" };
                                    result = (string)methodInfo.Invoke(instance_class, null);
                                }
                            }
                        }
                        else
                        {
                            var instance_class = Activator.CreateInstance(type);
                            object[] obj = new object[] { "hello" };
                            result = (string)methodInfo.Invoke(instance_class, null);
                        }
                        registered = true;
                        break;
                    }
                }
        }
        if (!string.IsNullOrEmpty(result)) return result;
        if (registered) return null;
        return COMMAND_NOT_FOUND;
    }
    public void Clear()
    {
        StartCoroutine(ClearTerminalCoroutine());
    }

    private IEnumerator ClearTerminalCoroutine()
    {
        yield return new WaitForEndOfFrame();
        history = "";
    }

    public MethodInfo[] GetMethods()
    {
        return methods.Values.ToArray();
    }
}
