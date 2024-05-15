using System;

using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    
    public static event Action<GameState> OnGameStateChanged;
    public enum  GameState
    {
        Play,
        Pause,
        Settings,
        MainMenu,
        Victory
    }
    public GameState state;
    public List<Level> levels;

    private string _levelNum = "levelNum";
    private string _lastPlayedLevel = "lastPlayerLevel";
    public Level currentLevel;
    public List<CarMovement> parkedVehicles;

    protected override void Awake()
    {
        base.Awake();
      // currentLevel = Instantiate(levels[PlayerPrefs.GetInt(_lastPlayedLevel)]);
        UpdateGameState(GameState.Play);
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
        if (newState == GameState.Pause)
        {
            
        }
        if (newState == GameState.Play)
        {
            VehicleList();
        }if (newState == GameState.Settings)
        {
            
        }if (newState == GameState.MainMenu)
        {
            
        }
        if (newState == GameState.Victory)
        {
            currentLevel.gameObject.SetActive(false);
           Extensions.UpdateInt(_levelNum, x => x + 1);
           var idx = PlayerPrefs.GetInt(_levelNum) >= levels.Count ? Random.Range(0, 3) : PlayerPrefs.GetInt(_levelNum);
           currentLevel = Instantiate(levels[idx]); 
            InputManager.Instance.mainCamera = Camera.main;
            PlayerPrefs.SetInt(_lastPlayedLevel,idx);
           UpdateGameState(GameState.Play);
        }
        OnGameStateChanged?.Invoke(newState);
    }
}
