using UnityEngine;
using System.Collections.Generic;
using Lovatto.Asset.InputManager;

public class bl_Input : MonoBehaviour
{
    public InputType m_InputType = InputType.Keyboard;
    public InputMapped m_InputMapped;

    public static bl_Input Instance = null;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if (FindObjectOfType<bl_Input>() != null && FindObjectOfType<bl_Input>() != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSaveKeys();
    }

    /// <summary>
    /// 
    /// </summary>
    void LoadSaveKeys()
    {
        string keyPrefix = string.Format("{0}.Keys", m_InputType.ToString());
        string json = PlayerPrefs.GetString(keyPrefix, string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            m_InputMapped.m_Mapped = JsonUtility.FromJson<InputMapped.Mapped>(json);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SaveKeys()
    {
        string keyPrefix = string.Format("{0}.Keys", m_InputType.ToString());
        string json = JsonUtility.ToJson(m_InputMapped.m_Mapped);
        PlayerPrefs.SetString(keyPrefix, json);
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool GetKeyDown(string function)
    {
        if (bl_Input.Instance.m_InputType == InputType.Keyboard)
        {
            return Input.GetKeyDown(bl_Input.Instance.GetKeyCode(function));
        }
        else
        {
            return bl_Input.Instance.GetKeyCodeOrAxis(function);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool GetKey(string function)
    {
        if (bl_Input.Instance.m_InputType == InputType.Keyboard)
        {
            if (function.Contains("Fire"))
            {
                return Input.GetMouseButton(0);
            }
            else if (function.Contains("Aim"))
            {
                return Input.GetMouseButton(1);
            }
            else if (function.Contains("FireDown"))
            {
                return Input.GetMouseButtonDown(0);
            }
            return Input.GetKey(bl_Input.Instance.GetKeyCode(function));
        }
        else
        {
            if (function.Contains("FireDown"))
            {
                function = "Fire";
            }
            return bl_Input.Instance.GetKeyCodeOrAxis(function, false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static bool GetKeyUp(string function)
    {
        return Input.GetKeyUp(bl_Input.Instance.GetKeyCode(function));
    }

    /// <summary>
    /// 
    /// </summary>
    public bool SetKey(string function,KeyCode newKey)
    {
        for (int i = 0; i < m_InputMapped.AllKeys.Count; i++)
        {
            if (m_InputMapped.AllKeys[i].Function == function)
            {
                m_InputMapped.AllKeys[i].Key = newKey;
                m_InputMapped.AllKeys[i].ResetAxis();
                SaveKeys();
                Debug.Log(string.Format("Done, replace key function: {0} with {1} key.", function, newKey.ToString()));
                return true;
            }
        }
        Debug.LogError(string.Format("Function {0} is not setup.", function));
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool SetAxis(string function, string Axis, float value)
    {
        for (int i = 0; i < m_InputMapped.AllKeys.Count; i++)
        {
            if (m_InputMapped.AllKeys[i].Function == function)
            {
                m_InputMapped.AllKeys[i].ResetAxis();
                if (GamePadButtonsNames.DirectionalAxisNames.ContainsKey(Axis))
                {
                    m_InputMapped.AllKeys[i].AxisButton = GamePadButtonsNames.DirectionalAxisNames[Axis][value];
                    m_InputMapped.AllKeys[i].AxisValue = value;
                }
                else { m_InputMapped.AllKeys[i].AxisButton = Axis; }
                m_InputMapped.AllKeys[i].Axis = Axis;
                m_InputMapped.AllKeys[i].isAxis = true;
                m_InputMapped.AllKeys[i].ResetKey();
                SaveKeys();
                Debug.Log(string.Format("Done, replace key function: {0} with {1} key.", function, Axis));
                return true;
            }
        }
        Debug.LogError(string.Format("Function {0} is not setup.", function));
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public KeyCode GetKeyCode(string function)
    {
        for(int i = 0; i < m_InputMapped.AllKeys.Count; i++)
        {
            if(m_InputMapped.AllKeys[i].Function == function)
            {
                return m_InputMapped.AllKeys[i].Key;
            }
        }
        Debug.LogError(string.Format("Key for {0} is not setup.", function));
        return KeyCode.None;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool GetKeyCodeOrAxis(string function, bool down = true)
    {
        for (int i = 0; i < m_InputMapped.AllKeys.Count; i++)
        {
            if (m_InputMapped.AllKeys[i].Function == function)
            {
                bl_KeyInfo key = m_InputMapped.AllKeys[i];
                if (key.isAxis)
                {
                    if (GamePadButtonsNames.DirectionalAxisNames.ContainsKey(key.Axis))
                    {
                        return Input.GetAxis(key.Axis) == key.AxisValue;
                    }
                    else
                    {
                        return Input.GetAxis(key.Axis) == 1;
                    }
                }
                else
                {
                    if (down)
                    {
                        return Input.GetKeyDown(m_InputMapped.AllKeys[i].Key);
                    }
                    else
                    {
                        return Input.GetKey(m_InputMapped.AllKeys[i].Key);
                    }
                }
            }
        }
        Debug.LogError(string.Format("Key for {0} is not setup.", function));
        return false;
    }

    /// <summary>
    /// Use this instead of Input.GetAxis("Vertical");
    /// </summary>
    public static float VerticalAxis
    {
        get
        {
            if (!bl_Input.Instance.isGamePad)
            {
                if (GetKey("Up") && !GetKey("Down"))
                {
                    return 1;
                }
                else if (!GetKey("Up") && GetKey("Down"))
                {
                    return -1;
                }
                else if (GetKey("Up") && GetKey("Down"))
                {
                    return 0.5f;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Input.GetAxis("Vertical");
            }
        }
    }

    /// <summary>
    /// start button on game pad controllers
    /// </summary>
    public static bool isStartPad
    {
        get
        {
            return GetKeyDown("Pause");
        }
    }

    public string GetKeyName(KeyCode key)
    {
        string keyName = key.ToString().ToUpper();
        if (m_InputType == InputType.Xbox)
        {
            if (GamePadButtonsNames.Xbox.ContainsKey(key)) { keyName = GamePadButtonsNames.Xbox[key].ToUpper(); }
        }
        return keyName;
    }

    /// <summary>
    /// Use this instead of Input.GetAxis("Horizontal");
    /// </summary>
    public static float HorizontalAxis
    {
        get
        {
            if (!bl_Input.Instance.isGamePad)
            {
                if (GetKey("Right") && !GetKey("Left"))
                {
                    return 1;
                }
                else if (!GetKey("Right") && GetKey("Left"))
                {
                    return -1;
                }
                else if (GetKey("Right") && GetKey("Left"))
                {
                    return 0.5f;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Input.GetAxis("Horizontal"); ;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool isKeyUsed(KeyCode newKey)
    {
        for (int i = 0; i < m_InputMapped.AllKeys.Count; i++)
        {
            if (m_InputMapped.AllKeys[i].Key == newKey)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool isAxisUsed(string axis)
    {
        for (int i = 0; i < m_InputMapped.AllKeys.Count; i++)
        {
            if (m_InputMapped.AllKeys[i].AxisButton == axis)
            {
                return true;
            }
        }
        return false;
    }

    public bool isGamePad { get { return (m_InputType == InputType.Xbox || m_InputType == InputType.Playstation); } }
}