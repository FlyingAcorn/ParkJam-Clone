using System;
using System.Collections.Generic;

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
    public string levelNum = "levelNum";
    public string lastPlayedLevel = "lastPlayerLevel";
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
        
        if (newState == GameState.Play)
        {
            VehicleList();
        }

        if (newState == GameState.Settings)
        {
            
        }
        
        if (newState == GameState.Victory)
        {
            UIManager.Instance.OpenPanel(UIManager.Instance.panels[1]);
            Extensions.UpdateInt(levelNum, x => x + 1);
            // add random amount of coin
            
        }

        OnGameStateChanged?.Invoke(newState);
    }
}