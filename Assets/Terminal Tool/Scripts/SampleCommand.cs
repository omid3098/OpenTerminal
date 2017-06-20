using UnityEngine;

public class SampleCommand
{
    [Command("debug")]
    public void SampleDebug()
    {
        Debug.Log("This is sample command");
    }
}
