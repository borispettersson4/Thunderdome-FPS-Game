using UnityEngine;
using UnityEngine.SceneManagement;

public class bl_LoadScene : MonoBehaviour {
   
    public void Return()
    {
        SceneManager.LoadScene("Customizer");
    }
}