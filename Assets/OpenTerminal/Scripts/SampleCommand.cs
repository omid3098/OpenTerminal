using UnityEngine;

public class SampleCommand
{
    [TerminalCommand("debug", "Debugs a sample line in Unity Console")]
    public void SampleDebug(string input)
    {
        Debug.Log(input);
    }
}
