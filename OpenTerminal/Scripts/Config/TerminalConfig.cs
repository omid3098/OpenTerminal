using UnityEngine;
[CreateAssetMenu()]
public class TerminalConfig : ScriptableObject
{
    public string console;
    public Font font;
    public int fontSize = 16;
    public Color commandColor;
    public Color backgroundColor;
    public Color autoCompleteHoverColor;

    [Header("LogReport")]
    public string supportEmail;
    public string EmailTitle;
    public int LogCount = 2000;
    public bool infoStacks = false;
    public bool warningStacks = false;
    public bool errorStacks = true;
    public bool assertStacks = true;
    public bool exceptionStacks = true;
}
