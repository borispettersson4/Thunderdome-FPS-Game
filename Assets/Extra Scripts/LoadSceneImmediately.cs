using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneImmediately : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
