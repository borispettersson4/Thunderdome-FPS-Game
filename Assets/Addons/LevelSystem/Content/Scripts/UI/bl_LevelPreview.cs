using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bl_LevelPreview : MonoBehaviour
{
    [SerializeField] private GameObject PreviewPrefab;
    [SerializeField] private Transform Panel;
    [SerializeField] private GameObject LevelList;
    [SerializeField] private Image CurrentLevelImg;
    [SerializeField] private Image NextLevelImg;
    [SerializeField] private Slider ProgressSlider;
    [SerializeField] private Text ScoreText;

    public bool LevelToReachDetected = false;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        InstanceLevels();
        LevelList.SetActive(false);
#if !ULSP
        ScoreText.text = "Require ULogin Pro to work.";
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    void InstanceLevels()
    {
        int score = 10000;
#if ULSP
        if (bl_DataBase.Instance != null)
        {
            score = bl_DataBase.Instance.LocalUser.Score;
        }
#endif
        for (int i = 0; i < bl_LevelManager.Instance.Levels.Count; i++)
        {
            LevelInfo l = bl_LevelManager.Instance.Levels[i];
            GameObject g = Instantiate(PreviewPrefab) as GameObject;
            g.transform.SetParent(Panel, false);
            int s = (LevelToReachDetected) ? -1 : score;
            g.GetComponent<bl_LevelPreviewUI>().Set(l, s, this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowList()
    {
        LevelToReachDetected = false;
        LevelList.SetActive(true);
        CalculateLevel();
    }

    /// <summary>
    /// 
    /// </summary>
    void CalculateLevel()
    {
#if ULSP
        int score = 10000;
        if (bl_DataBase.Instance != null)
        {
            score = bl_DataBase.Instance.LocalUser.Score;
        }
        LevelInfo cl = bl_LevelManager.Instance.GetLevel(score);
        LevelInfo nl = bl_LevelManager.Instance.GetNextLevel(score);

        CurrentLevelImg.sprite = cl.Icon;
        NextLevelImg.sprite = nl.Icon;
        ScoreText.text = score + " XP";
    
        ProgressSlider.value = ((float)score / (float)nl.ScoreNeeded);
#endif
    }
}