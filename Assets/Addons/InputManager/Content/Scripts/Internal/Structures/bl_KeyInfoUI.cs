using UnityEngine;
using UnityEngine.UI;

public class bl_KeyInfoUI : MonoBehaviour
{
    [SerializeField]private Text FunctionText;
    [SerializeField]private Text KeyText;
    [SerializeField] private Color PrimaryColor;
    [SerializeField] private Color SecondaryColor;

    private bl_KeyInfo cacheInfo;
    private bl_KeyOptionsUI KeyOptions;
    private Button m_Button;

    public void Init(bl_KeyInfo info,bl_KeyOptionsUI koui, bool second)
    {
        cacheInfo = info;
        KeyOptions = koui;
        FunctionText.text = info.Description.ToUpper();
        if (!info.isAxis)
        {
            string keyName = bl_Input.Instance.GetKeyName(info.Key);
            KeyText.text = keyName.ToUpper();
        }
        else
        {
            string keyName = (string.IsNullOrEmpty(info.AxisButton)) ? info.Axis : info.AxisButton;
            KeyText.text = keyName.ToUpper();
        }
        m_Button = GetComponentInChildren<Button>();
        if(m_Button != null)
        {
            Color c = PrimaryColor;
            if (second) { c = SecondaryColor; }
            ColorBlock cb = m_Button.colors;
            cb.normalColor = c;
            m_Button.colors = cb;
        }
    }

    public void SetKeyChange()
    {
        KeyOptions.SetWaitKeyProcess(cacheInfo);
    }
 
}