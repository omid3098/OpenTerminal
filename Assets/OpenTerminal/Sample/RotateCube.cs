using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    private bool stop;
    void Update()
    {
        if (!stop)
            transform.Rotate(new Vector3(1, 1, 1));
    }


    [TerminalCommand("stop-cube", "stops the cube from rotating")]
    public void StopCube()
    {
        stop = true;
    }


    [TerminalCommand("rotate-cube", "rotates the cube")]
    public void RotateTheCube()
    {
        stop = false;
    }

    [TerminalCommand("move-cube", "move-cube(x,y,z) Moves the cube")]
    public void Move(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }
}
