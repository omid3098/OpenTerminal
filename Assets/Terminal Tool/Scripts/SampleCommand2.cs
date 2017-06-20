namespace Q2
{
    using UnityEngine;

    public class SampleCommand2
    {
        [Command("help")]
        public void MyDebug2()
        {
            Debug.Log("This is help!");
        }
    }
}