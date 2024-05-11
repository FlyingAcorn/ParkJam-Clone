using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Level currentLevel;
    public List<CarMovement> parkedVehicles;

    protected override void Awake()
    {
        base.Awake();
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
            
        }
        OnGameStateChanged?.Invoke(newState);
    }
}
