using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lovatto.Asset.InputManager;

public class InputManagerAddKeyEditor : EditorWindow
{
    private InputMapped.Mapped m_Mapped;
    private InputType m_InputType = InputType.Keyboard;
    private InputType ChangeInputType = InputType.Keyboard;
    private int WindowID = 0;

    private string KeyText;
    private string CommandKey = "";
    private KeyCode KeyToAdd = KeyCode.A;
    private int KeyToReplace = 0;
    private bool NotDataSaved = false;

    private void OnEnable()
    {
        LoadKeys(m_InputType);
    }

    void LoadKeys(InputType it)
    {
        string keyPrefix = string.Format("{0}.Keys", it);
        string json = PlayerPrefs.GetString(keyPrefix, string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            m_Mapped = JsonUtility.FromJson<InputMapped.Mapped>(json);
            NotDataSaved = false;
            float y = (m_Mapped.AllKeys.Count + 2) * 20;
            y += 42;
            minSize = new Vector2(560, y);
        }
        else
        {
            NotDataSaved = true;
        }
    }

    void SaveKeys()
    {
        string keyPrefix = string.Format("{0}.Keys", m_InputType.ToString());
        string json = JsonUtility.ToJson(m_Mapped);
        PlayerPrefs.SetString(keyPrefix, json);
    }

    private void OnGUI()
    {
        GUI.skin.label.richText = true;
        GUI.skin.button.richText = true;
        if (NotDataSaved)
        {
            GUILayout.BeginHorizontal();
            ChangeInputType = (InputType)EditorGUILayout.EnumPopup("Input Device:", ChangeInputType);
            if (GUILayout.Button("Load"))
            {
                LoadKeys(ChangeInputType);
                m_InputType = ChangeInputType;
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("Not data saved yet for this Input Type,\n Keys will saved after the first time that you open the key window in game.");
            return;
        }
        if (WindowID == 0)
        {
            KeysList();
        }
        else if (WindowID == 1)
        {
            AddKey();
        }
        else if (WindowID == 2)
        {
            ReplaceKey();
        }
    }

    void AddKey()
    {
        GUILayout.BeginVertical("box");
        KeyToAdd = (KeyCode)EditorGUILayout.EnumPopup("Key: ", KeyToAdd);
        CommandKey = EditorGUILayout.TextField("Command: ", CommandKey);
        KeyText = EditorGUILayout.TextField("Description: ", KeyText);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("< BACK", GUILayout.Width(100)))
        {
            WindowID = 0;
        }
        GUI.enabled = !string.IsNullOrEmpty(CommandKey);
        if (GUILayout.Button("ADD", GUILayout.Width(100)))
        {
            bl_KeyInfo i = new bl_KeyInfo();
            i.Key = KeyToAdd;
            i.Function = CommandKey;
            i.Description = KeyText;
            m_Mapped.AllKeys.Add(i);
            ResetAdd();
            WindowID = 0;
        }
        GUI.enabled = true;
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void ReplaceKey()
    {
        GUILayout.BeginVertical("box");
        KeyToAdd = (KeyCode)EditorGUILayout.EnumPopup("Key: ", KeyToAdd);
        CommandKey = EditorGUILayout.TextField("Command: ", CommandKey);
        KeyText = EditorGUILayout.TextField("Description: ", KeyText);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("< BACK", GUILayout.Width(100)))
        {
            WindowID = 0;
        }
        if (GUILayout.Button("REPLACE", GUILayout.Width(100)))
        {
            bl_KeyInfo i = m_Mapped.AllKeys[KeyToReplace];
            i.Key = KeyToAdd;
            i.Function = CommandKey;
            i.Description = KeyText;
            m_Mapped.AllKeys[KeyToReplace] = i;
            m_Mapped.AllKeys.Add(i);
            ResetAdd();
            WindowID = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void KeysList()
    {
        GUILayout.BeginVertical();
        GUI.color = Color.green;
        if (GUILayout.Button("ADD NEW KEY"))
        {
            WindowID = 1;
        }
        GUI.color = Color.white;
        GUILayout.EndVertical();
        if (m_Mapped != null)
        {
            GUILayout.BeginVertical("box");
            for (int i = 0; i < m_Mapped.AllKeys.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("<b>Command:</b> {0}", m_Mapped.AllKeys[i].Function), GUILayout.Width(175));
                GUILayout.Label(string.Format("<b>Key:</b> {0}", m_Mapped.AllKeys[i].Key.ToString()), GUILayout.Width(100));
                if (GUILayout.Button("Remove",EditorStyles.toolbarButton, GUILayout.Width(75)))
                {
                    m_Mapped.AllKeys.RemoveAt(i);
                }
                if (GUILayout.Button("Replace", EditorStyles.toolbarButton, GUILayout.Width(75)))
                {
                    ResetAdd();
                    KeyToReplace = i;
                    KeyToAdd = m_Mapped.AllKeys[i].Key;
                    CommandKey = m_Mapped.AllKeys[i].Function;
                    KeyText = m_Mapped.AllKeys[i].Description;
                    WindowID = 2;
                }
                if (i > 0)
                {
                    if (GUILayout.Button("Up", EditorStyles.toolbarButton, GUILayout.Width(50)))
                    {
                        MoveItem(i, i - 1);
                    }
                }
                if(i < m_Mapped.AllKeys.Count - 1)
                {
                    if (GUILayout.Button("Down", EditorStyles.toolbarButton, GUILayout.Width(50)))
                    {
                        MoveItem(i, i + 1);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("REVERT",GUILayout.Height(32)))
        {
            LoadKeys(m_InputType);
        }
        if (GUILayout.Button("<color=green>SAVE</color>", GUILayout.Height(32)))
        {
            SaveKeys();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        ChangeInputType = (InputType)EditorGUILayout.EnumPopup("Input Device:", ChangeInputType);
        if (m_InputType != ChangeInputType)
        {
            if (GUILayout.Button("Load"))
            {
                LoadKeys(ChangeInputType);
                m_InputType = ChangeInputType;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void MoveItem(int old, int newID)
    {
        bl_KeyInfo k = m_Mapped.AllKeys[old];
        m_Mapped.AllKeys.RemoveAt(old);
        m_Mapped.AllKeys.Insert(newID, k);
    }

    void ResetAdd()
    {
        KeyToAdd = KeyCode.A;
        CommandKey = string.Empty;
        KeyText = string.Empty;
    }

    [MenuItem("MFPS/Addons/InputManager/Edit Keys")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(InputManagerAddKeyEditor));
    }
}