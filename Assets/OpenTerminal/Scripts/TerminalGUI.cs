using System;
using UnityEngine;

public class TerminalGUI {
    private Terminal terminal;
    private TerminalConfig config;
    private Texture2D texture2D;

    public GUIStyle terminalStyle { get; private set; }

    public TerminalGUI (Terminal terminal) {
        this.terminal = terminal;
        this.config = terminal.config;
        terminalStyle = new GUIStyle ();
        terminalStyle.font = config.font;
        terminalStyle.fontSize = 16;
        terminalStyle.richText = true;
        terminalStyle.normal.textColor = config.commandColor;
        terminalStyle.hover.textColor = config.autoCompleteHoverColor;
        terminalStyle.active.textColor = config.autoCompleteHoverColor;
        terminalStyle.onHover.textColor = config.autoCompleteHoverColor;
        terminalStyle.onActive.textColor = config.autoCompleteHoverColor;

        texture2D = new Texture2D (2, 2);
        texture2D.SetPixels (0, 0, texture2D.width, texture2D.height, new Color[] { config.backgroundColor, config.backgroundColor, config.backgroundColor, config.backgroundColor });
        texture2D.wrapMode = TextureWrapMode.Repeat;
        texture2D.Apply();

        terminalStyle.normal.background = texture2D;
    }

    internal void OnGUI () {
        GUILayout.Label (terminal.history + terminal.consoleLine + terminal.inputText, terminalStyle);
        if (terminal.autoCompList.Count > 0) {
            for (int i = 0; i < terminal.autoCompList.Count; i++) {
                GUI.SetNextControlName (terminal.autoCompList[i]);
                GUIStyle t = terminalStyle;
                if (i == terminal.autoCompIndex)
                    GUI.color = config.autoCompleteHoverColor;
                else
                    GUI.color = config.commandColor;
                if (GUILayout.Button (terminal.autoCompList[i], t)) {
                    terminal.ChangeInput (terminal.autoCompList[i]);
                    terminal.PreExecute ();
                }
            }
        } else
            GUI.color = config.commandColor;
    }

}