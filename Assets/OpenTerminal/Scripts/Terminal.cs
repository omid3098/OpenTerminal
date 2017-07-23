using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
public class Terminal : MonoBehaviour
{
    public static Terminal instance;
    [SerializeField] public TerminalConfig config;
    public bool displayTerminal { get; private set; }
    public string inputText { get; private set; }
    public string history { get; private set; }
    public List<string> autoCompList { get; private set; }
    public int autoCompIndex { get; private set; }
    public string consoleLine { get { return (config.console + config.backslash + config.arrow + " "); } }
    public TerminalMethods terminalMethods;
    private TerminalInputHandler inputHandler;
    private TerminalGUI terminalGui;
    public TouchScreenKeyboard touchScreenKeyboard;
    public int mobileTouchCount = 4;

    void Awake()
    {
        instance = this;
        if (config == null) config = Resources.Load<TerminalConfig>("Config/ZSH");
        if (mobileTouchCount <= 0) mobileTouchCount = 4;
        autoCompIndex = 0;
        autoCompList = new List<string>();
        terminalMethods = new TerminalMethods();
        inputHandler = new TerminalInputHandler(this);
        terminalGui = new TerminalGUI(this);
    }

    internal void Hide()
    {
        displayTerminal = false;
    }

    void OnGUI()
    {
        if (!displayTerminal) return;
        terminalGui.OnGUI();
    }

    void Update()
    {
        inputHandler.Update();
    }

    /// <summary>
    /// For Android mobile keyboard input
    /// </summary>
    /// <param name="inputString"></param>
    internal void SetInputText(string inputString)
    {
        inputText = inputString;
    }

    /// <summary>
    /// For PC keyboard input
    /// </summary>
    /// <param name="input"></param>
    public void UpdateInputText(string input)
    {
        inputText += input;
    }

    public void OnUpArrowPressed()
    {
        if (autoCompList.Count > 0)
            autoCompIndex = (int)Mathf.Repeat(autoCompIndex - 1, autoCompList.Count);
    }

    internal void ChangeInput(string input)
    {
        inputText = input;
    }

    public void OnDownArrowPressed()
    {
        if (autoCompList.Count > 0)
            autoCompIndex = (int)Mathf.Repeat(autoCompIndex + 1, autoCompList.Count);
    }

    public void PreExecute()
    {
        string result = ExecuteCommand(inputText);
        history += consoleLine + inputText + "\n" + (!string.IsNullOrEmpty(result) ? (result + "\n") : "");
        inputText = "";
    }

    public void OnTabPressed()
    {
        if (autoCompList.Count != 0) { OnEnterPressed(); return; }
        autoCompIndex = 0;
        autoCompList.Clear();
        autoCompList.AddRange(terminalMethods.GetCommandsContaining(inputText));
    }

    private string ExecuteCommand(string inputText)
    {
        autoCompList.Clear();
        bool registered = false;
        string result = null;
        string insideParentheses = Regex.Match(inputText, @"\(([^)]*)\)").Groups[1].Value;
        List<string> args = new List<string>();
        string command;
        if (!string.IsNullOrEmpty(insideParentheses))
        {
            args = insideParentheses.Split(new char[] { ',' }).ToList();
            command = inputText.Replace(insideParentheses, "").Replace("(", "").Replace(")", "").Replace(";", "");
        }
        else command = inputText.Replace("(", "").Replace(")", "").Replace(";", "");
        foreach (var method in terminalMethods.methods)
        {
            foreach (object attribute in method.GetCustomAttributes(true)) // Returns all 3 of my attributes.
                if (attribute is CommandAttribute)
                {
                    CommandAttribute attr = (CommandAttribute)attribute;
                    if (attr.commandName == command)
                    {
                        if (registered) Debug.LogError(TerminalStrings.MULTIPLE_COMMAND_NAMES + command);
                        Type type = (method.DeclaringType);
                        ParameterInfo[] methodParameters = method.GetParameters();
                        List<object> argList = new List<object>();
                        // Cast Arguments if there is any
                        if (args.Count != 0)
                        {
                            if (methodParameters.Length != args.Count)
                            {
                                result = string.Format(TerminalStrings.ARGUMENT_COUNT_MISSMATCH, command, methodParameters.Length, args.Count);
                                Debug.Log(result);
                                return result;
                            }
                            else
                            {
                                // Cast string arguments to input objects types
                                for (int i = 0; i < methodParameters.Length; i++)
                                {
                                    try
                                    {
                                        var a = Convert.ChangeType(args[i], methodParameters[i].ParameterType);
                                        argList.Add(a);
                                    }
                                    catch
                                    {
                                        result = string.Format("Counld not convert {0} to Type {1}", args[i], methodParameters[i].ParameterType);
                                        Debug.LogError(result);
                                        return result;
                                    }
                                }
                            }
                        }
                        if (type.IsSubclassOf(typeof(UnityEngine.Object)))
                        {
                            var instance_classes = GameObject.FindObjectsOfType(type);
                            if (instance_classes != null)
                            {
                                foreach (var instance_class in instance_classes)
                                {
                                    result = (string)method.Invoke(instance_class, argList.ToArray());
                                }
                            }
                        }
                        else
                        {
                            var instance_class = Activator.CreateInstance(type);
                            result = (string)method.Invoke(instance_class, argList.ToArray());
                        }
                        registered = true;
                        break;
                    }
                }
        }
        if (!string.IsNullOrEmpty(result)) return result;
        if (registered) return null;
        return TerminalStrings.COMMAND_NOT_FOUND;
    }
    internal void Clear()
    {
        StartCoroutine(ClearTerminalCoroutine());
    }

    private IEnumerator ClearTerminalCoroutine()
    {
        yield return new WaitForEndOfFrame();
        history = "";
    }

    internal void ToggleTerminal()
    {
        displayTerminal = !displayTerminal;
        DisplayTouchScreenKeyboard();
    }

    public void DisplayTouchScreenKeyboard()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (displayTerminal)
            {
                touchScreenKeyboard = TouchScreenKeyboard.Open(inputText, TouchScreenKeyboardType.Default);
                TouchScreenKeyboard.hideInput = true;
            }
        }
    }

    internal void OnBackSpacePressed()
    {
        if (inputText.Length >= 1) inputText = inputText.Substring(0, inputText.Length - 1);
    }
    internal void OnEnterPressed()
    {
        if (autoCompList.Count > 0)
        {
            inputText = autoCompList[autoCompIndex];
            autoCompList.Clear();
        }
        else
            PreExecute();
    }
}