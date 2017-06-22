using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
	List<string> commandNames = new List<string> ();
	List<string> autoCompList = new List<string> ();
	int autoCompIndex = 0;
	void Awake()
	{
		instance = this;
		if (config == null) config = Resources.Load<TerminalConfig>("Config/ZSH");
		terminalStyle = new GUIStyle();
		terminalStyle.font = config.font;
		terminalStyle.fontSize = 16;
		terminalStyle.normal.textColor = config.commandColor;
		terminalStyle.hover.textColor = Color.red;
		terminalStyle.active.textColor = Color.red;
		terminalStyle.onHover.textColor = Color.red;
		terminalStyle.onActive.textColor = Color.red;
		var assembly = System.AppDomain.CurrentDomain.Load("Assembly-CSharp");
		methods = assembly
			.GetTypes()
			.SelectMany(x => x.GetMethods())
			.Where(y => y.GetCustomAttributes(true).OfType<CommandAttribute>().Any())
			.ToDictionary(z => z.Name);
		GetCommandNames ();
	}

	void OnGUI()
	{
		if (!displayTerminal) return;
		GUILayout.Label(history + consoleLine() + inputText, terminalStyle);
		if (autoCompList.Count > 0) {
			for (int i = 0; i < autoCompList.Count; i++) {
				GUI.SetNextControlName (autoCompList [i]);
				GUIStyle t = terminalStyle;
				if (i == autoCompIndex)
					GUI.color = Color.red;
				else
					GUI.color = Color.white;
				if (GUILayout.Button (autoCompList [i], t)) {
					inputText = autoCompList [i];
					ShowResult (autoCompList [i]);
				}
			}
		} else
			GUI.color = Color.white;
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
			AutoComplete (inputText);
            return;

		}

		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if (autoCompList.Count > 0){
				inputText = autoCompList [autoCompIndex];
				autoCompList.Clear();
			}
			else 
				ShowResult (inputText);
			return;
		}
		if (autoCompList.Count > 0) {
			if (Input.GetKeyDown (KeyCode.DownArrow))
				autoCompIndex = (int)Mathf.Repeat(autoCompIndex+1, autoCompList.Count);
			else if (Input.GetKeyDown (KeyCode.UpArrow))
				autoCompIndex = (int)Mathf.Repeat(autoCompIndex-1, autoCompList.Count);
		}else 
			inputText += Input.inputString;
	}

	private void ShowResult(string input){
		string result = ExecuteCommand(input);
		history += consoleLine() + input + "\n" + (!string.IsNullOrEmpty(result) ? (result + "\n") : "");
		inputText = "";
	}

	private void GetCommandNames(){
		commandNames.Clear ();
		foreach (var method in methods.Values)
		{
			foreach (var attribute in method.GetCustomAttributes(true))
			{
				if (attribute is CommandAttribute) //Does not pass
				{
					CommandAttribute attr = (CommandAttribute)attribute;
					commandNames.Add (attr.commandName);
				}
			}
		}
	}
	private void AutoComplete(string input){
		autoCompIndex = 0;
		autoCompList.Clear ();
		autoCompList.AddRange (commandNames.Where(k => k.Contains(input)));
	}

	private string ExecuteCommand(string inputText)
	{
		autoCompList.Clear ();
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
		Debug.Log("command : " + command);
		Debug.Log("argument : " + insideParentheses);
		foreach (var method in methods)
		{
			var methodName = method.Key;
			var methodInfo = method.Value;
			foreach (object attribute in methodInfo.GetCustomAttributes(true)) // Returns all 3 of my attributes.
				if (attribute is CommandAttribute)
				{
					CommandAttribute attr = (CommandAttribute)attribute;
					if (attr.commandName == command)
					{
						if (registered) Debug.LogError("Multiple commands are defined with: " + command);
						Type type = (methodInfo.DeclaringType);
						ParameterInfo[] methodParameters = methodInfo.GetParameters();
						List<object> argList = new List<object>();
						// Cast Arguments if there is any
						if (args.Count != 0)
						{
							if (methodParameters.Length != args.Count)
							{
								result = string.Format("Method {0} needs {1} arguments, passed {2}", methodName, methodParameters.Length, args.Count);
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
									result = (string)methodInfo.Invoke(instance_class, argList.ToArray());
								}
							}
						}
						else
						{
							var instance_class = Activator.CreateInstance(type);
							result = (string)methodInfo.Invoke(instance_class, argList.ToArray());
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
