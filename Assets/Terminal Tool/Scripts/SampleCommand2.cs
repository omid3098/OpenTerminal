namespace Q2
{
    using UnityEngine;

    public class SampleCommand2
    {
        [Command("help")]
        public string Help()
        {
            const string help_string = "This is help!";
            Debug.Log(help_string);
            return help_string;
        }
    }
}