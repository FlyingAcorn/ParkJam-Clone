using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Panel[] panels;
   
    // setting 0,victory 1, tutorial 2 , inplay 3
    //TODO: Cars will have random emoji popups,fill empty methods and implement the proper tweens of the panels and emojis
    //TODO: Hook Coin text in settings and victory panel,level in inplay panel.You should give coin text a increase animation.
    //TODO: Implement a system that corrects the tutorial panels image to a specific vehicle also give animations to the images.
    // you should do animation and tween related stuff in seperate methods.
    public void RestartLevel()
    {
        var gameManager = GameManager.Instance;
        gameManager.currentLevel.gameObject.SetActive(false);
        gameManager.currentLevel = Instantiate(gameManager.currentLevel);
        if (!panels[0].gameObject.activeSelf) return;
        ClosePanel(panels[0]);
        gameManager.UpdateGameState(GameManager.GameState.Play);
    }

    public void NextLevel()
    {
        var gameManager = GameManager.Instance;
        gameManager.currentLevel.gameObject.SetActive(false);
        var idx = PlayerPrefs.GetInt(gameManager.levelNum) >= gameManager.levels.Count
            ? Random.Range(0, 3)
            : PlayerPrefs.GetInt(gameManager.levelNum);
        gameManager.currentLevel = Instantiate(gameManager.levels[idx]);
        InputManager.Instance.mainCamera = Camera.main;
        PlayerPrefs.SetInt(gameManager.lastPlayedLevel, idx);
        // add level increase
        gameManager.UpdateGameState(GameManager.GameState.Play);
    }

    public void OpenPanel(Panel panel)
    {
        // if panel == panels[idx] do someting (ex. change gamestate)
        // Settings and victory panels will have  proper tweens make seperate methods
        panel.gameObject.SetActive(true);
    }

    public void ClosePanel(Panel panel)
    {
        // Settings and victory panels will have  proper tweens
        // if panel == panels[idx] do someting
        panel.gameObject.SetActive(false);
    }

    public void DeleteCache()
    {
    }

    public void ToggleSound(bool tog)
    {
    }

    public void ToggleHaptics(bool tog)
    {
    }
}