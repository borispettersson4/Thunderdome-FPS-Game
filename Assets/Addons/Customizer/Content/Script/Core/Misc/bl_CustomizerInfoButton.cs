using UnityEngine;
using UnityEngine.UI;

public class bl_CustomizerInfoButton : MonoBehaviour {

	[SerializeField]private Text m_Text;
    private string UniqueName;
    private Button[] AllButtons;

    public void Init(string name)
    {
        UniqueName = name;
        m_Text.text = UniqueName;
    }


    public void OnSelect()
    {
        if(AllButtons == null || AllButtons.Length <= 0) { AllButtons = transform.parent.GetComponentsInChildren<Button>(); }

        bl_CustomizerManager c = FindObjectOfType<bl_CustomizerManager>();
        c.Active_Custom(UniqueName);

        foreach(Button b in AllButtons)
        {
            b.interactable = true;
        }
        GetComponent<Button>().interactable = false;
    }
}