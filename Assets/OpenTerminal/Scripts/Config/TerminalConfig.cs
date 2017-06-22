using System;
using UnityEngine;
[CreateAssetMenu()]
public class TerminalConfig : ScriptableObject
{
    public string arrow;
    public string backslash;
    public string console;
    public Font font;
    public Color commandColor;
    public Color autoCompleteHoverColor;
}
