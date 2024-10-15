using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;

    public enum GameState
    {
        MainMenu,
        Play,
        Settings,
        Victory
    }

    public GameState state;
    public List<Level> levels;
    public int totalCoins;
    public Level currentLevel;
    public List<CarMovement> parkedVehicles;

    public int CoinsAmount
    {
        get => PlayerPrefs.GetInt("Coin", 0);
        private set
        {
            if (value >= 999)
            {
                PlayerPrefs.SetInt("Coin", 999);
            }
            else
            {
                PlayerPrefs.SetInt("Coin", value);
            }
        }
    }

    public int LevelNo
    {
        get => PlayerPrefs.GetInt("Level", 1);
        private set => PlayerPrefs.SetInt("Level", value);
    }


    public int LastPlayedLevel
    {
        get => PlayerPrefs.GetInt("LastPlayedLevel", 0);
        private set => PlayerPrefs.SetInt("LastPlayedLevel", value);
    }

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        UpdateGameState(GameState.MainMenu);
    }

    public void VehicleList()
    {
        var array = currentLevel.GetComponentsInChildren<CarMovement>();
        foreach (var t in array)
        {
            parkedVehicles.Add(t);
        }
    }


    public void UpdateGameState(GameState newState)
    {
        state = newState;

        if (newState == GameState.MainMenu)
        {
            
        }

        if (newState == GameState.Play)
        {
            UIManager.Instance.OpenPanel(UIManager.Instance.panels[3]);
        }

        if (newState == GameState.Settings)
        {
        }

        if (newState == GameState.Victory)
        {
            LevelNo++;
            UIManager.Instance.panels[3].panelTexts[0].text = "Level " + LevelNo;
            CoinsAmount += Random.Range(50, 101);
            UIManager.Instance.OpenPanel(UIManager.Instance.panels[1]);
            UIManager.Instance.UpdateCoins();
            LastPlayedLevel = LevelNo > levels.Count
                ? Random.Range(0, levels.Count)
                : LevelNo - 1;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}