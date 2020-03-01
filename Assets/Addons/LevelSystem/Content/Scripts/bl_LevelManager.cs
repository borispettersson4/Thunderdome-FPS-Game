using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class bl_LevelManager : ScriptableObject
{

    public List<LevelInfo> Levels = new List<LevelInfo>();

    [Header("Settings")]
    public bool UpdateLevelsInMidGame = true;

    public bool isNewLevel { get; set; }
    private int LastLevel = 0;

    public void GetInfo()
    {
        LastLevel = GetLevelID();
    }

    public LevelInfo GetLevel(int score)
    {
        if (Levels == null || Levels.Count <= 0)
            return null;

        if (score >= Levels[Levels.Count - 1].ScoreNeeded) { return Levels[Levels.Count - 1]; } //max level

        for (int i = 0; i < Levels.Count; i++)
        {
            if (score >= Levels[i].ScoreNeeded) continue;
            if (score < Levels[i].ScoreNeeded)
            {
                int id = (i > 0) ? i - 1 : i;
                return Levels[id];
            }
        }
        return Levels[0];
    }

    public LevelInfo GetNextLevel(int score)
    {
        if (Levels == null || Levels.Count <= 0)
            return null;

        if (score >= Levels[Levels.Count - 1].ScoreNeeded) {return Levels[Levels.Count - 1]; } //max level

        for (int i = 0; i < Levels.Count; i++)
        {
            if (score >= Levels[i].ScoreNeeded) continue;
            if (score < Levels[i].ScoreNeeded)
            {
                int id = (i > 0) ? i - 1 : i;
                return Levels[id + 1];
            }
        }
        return Levels[0];
    }

    public LevelInfo GetLevel()
    {
        int score = 0;
        if (Levels == null || Levels.Count <= 0)
            return null;

#if ULSP
        bl_DataBase db = bl_DataBase.Instance;
        if(db != null)
        {
            score = db.LocalUser.Score;
        }
#else
        Debug.LogWarning("Level system need 'ULogin Pro' add-on to work");
#endif
        if (score >= Levels[Levels.Count - 1].ScoreNeeded) { return Levels[Levels.Count - 1]; } //max level

        for (int i = 0; i < Levels.Count; i++)
        {
            if (score >= Levels[i].ScoreNeeded) continue;
            if (score < Levels[i].ScoreNeeded)
            {
                int id = (i > 0) ? i - 1 : i;
                return Levels[id];
            }
        }
        return Levels[0];
    }

    public LevelInfo GetPlayerLevelInfo(Player player)
    {
        int score = 0;
        if (Levels == null || Levels.Count <= 0)
            return null;

#if ULSP
        score = (int)player.CustomProperties["TotalScore"];
#else
        Debug.LogWarning("Level system need 'ULogin Pro' add-on to work");
#endif
        if (UpdateLevelsInMidGame)
        {
            score += player.GetPlayerScore();
        }
        if (score >= Levels[Levels.Count - 1].ScoreNeeded) { return Levels[Levels.Count - 1]; } //max level

        for (int i = 0; i < Levels.Count; i++)
        {
            if (score >= Levels[i].ScoreNeeded) continue;
            if (score < Levels[i].ScoreNeeded)
            {
                int id = (i > 0) ? i - 1 : i;
                return Levels[id];
            }
        }
        return Levels[0];
    }

    public int GetLevelID(int score)
    {
        if (Levels == null || Levels.Count <= 0)
            return 0;

        if (score >= Levels[Levels.Count - 1].ScoreNeeded) {  return Levels.Count - 1; } //max level

        for (int i = 0; i < Levels.Count; i++)
        {
            if (score >= Levels[i].ScoreNeeded) continue;
            if (score < Levels[i].ScoreNeeded)
            {
                int id = (i > 0) ? i - 1 : i;
                return id;
            }
        }
        return 0;
    }

    public int GetLevelID()
    {
        int score = 0;
        if (Levels == null || Levels.Count <= 0)
            return 0;
#if ULSP
        bl_DataBase db = bl_DataBase.Instance;
        if (db != null)
        {
            score = db.LocalUser.Score;
        }
#else
        Debug.LogWarning("Level system need 'ULogin Pro' add-on to work");
#endif


        if (score >= Levels[Levels.Count - 1].ScoreNeeded) { return Levels.Count - 1; } //max level

        for (int i = 0; i < Levels.Count; i++)
        {
            if (score >= Levels[i].ScoreNeeded) continue;
            if (score < Levels[i].ScoreNeeded)
            {
                int id = (i > 0) ? i - 1 : i;
                return id;
            }
        }
        return 0;
    }

     public void Check(int score)
    {
        int current = GetLevelID(score);
        if(current > LastLevel)
        {
            isNewLevel = true;
        }
    }

    public void Refresh()
    {
        isNewLevel = false;
    }

    private static bl_LevelManager _instance;
    public static bl_LevelManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Resources.Load<bl_LevelManager>("LevelManager") as bl_LevelManager;
            }
            return _instance;
        }
    }
}