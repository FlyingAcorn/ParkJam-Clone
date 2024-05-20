using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;

    public enum GameState
    {
        Play,
        Settings,
        Victory
    }

    public GameState state;
    public List<Level> levels;
    public readonly string levelNum = "levelNum";
    public readonly string lastPlayedLevel = "lastPlayedLevel";
    public readonly string coinAmount = "coinAmount";
    public int totalCoins;
    public Level currentLevel;
    public List<CarMovement> parkedVehicles;

    protected override void Awake()
    { 
        base.Awake();
        currentLevel = Instantiate(levels[PlayerPrefs.GetInt(lastPlayedLevel)]);
        
    }

    private void Start()
    {
        UpdateGameState(GameState.Play);
        if (PlayerPrefs.GetInt(levelNum) == 0)
        {
            PlayerPrefs.SetInt(levelNum,1);
        }
        UIManager.Instance.panels[3].panelTexts[0].text = "Level " + (PlayerPrefs.GetInt(levelNum));
        
    }

    private void VehicleList()
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
        
        if (newState == GameState.Play)
        {
            if (parkedVehicles.Count == 0)
            {
                VehicleList();
            }
            UIManager.Instance.OpenPanel(UIManager.Instance.panels[3]);
            
            
        }

        if (newState == GameState.Settings)
        {
            
        }
        
        if (newState == GameState.Victory)
        {
            
            Extensions.UpdateInt(coinAmount, x => x+Random.Range(50,101));
            UIManager.Instance.OpenPanel(UIManager.Instance.panels[1]);
            Extensions.UpdateInt(levelNum, x => x + 1);
            UIManager.Instance.UpdateCoins();
        }

        OnGameStateChanged?.Invoke(newState);
    }
}