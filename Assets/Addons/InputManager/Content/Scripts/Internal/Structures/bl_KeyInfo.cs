using UnityEngine;
using System;

[Serializable]
public class bl_KeyInfo
{
    public string Function = "None";
    public KeyCode Key;
    public string Description = "";

   [HideInInspector] public string Axis;
   [HideInInspector] public bool isAxis;
   [HideInInspector]public string AxisButton;
    [HideInInspector] public float AxisValue;

    public void ResetAxis()
    {
        isAxis = false;
        Axis = string.Empty;
        AxisButton = string.Empty;
        AxisValue = 0;
    }

    public void ResetKey()
    {
        Key = KeyCode.None;
    }
}