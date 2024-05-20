using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Panel[] panels;

    // panel indexes = setting 0,victory 1, tutorial 2 , inplay 3
    //TODO: Cars will have random emoji popups,fill empty methods and implement the proper tweens of the panels and emojis
    //TODO: Implement a system that corrects the tutorial panels image to a specific vehicle also give animations to the images.
    // TODO: You can add a panel that that will pop up and give daily random coin on opening game (make a state for it)
    // you should do animation and tween related stuff in seperate methods.
    public void RestartLevel()
    {
        var gameManager = GameManager.Instance;
        gameManager.currentLevel.gameObject.SetActive(false);
        gameManager.currentLevel = Instantiate(gameManager.levels[PlayerPrefs.GetInt(gameManager.lastPlayedLevel)]);
        gameManager.currentLevel.gameObject.SetActive(true);
        InputManager.Instance.mainCamera = Camera.main;
        gameManager.parkedVehicles.Clear();
        gameManager.UpdateGameState(GameManager.GameState.Play);
        if (!panels[0].gameObject.activeSelf) return;
        ClosePanel(panels[0]);
    }

    public void NextLevel()
    {
        var gameManager = GameManager.Instance;
        gameManager.currentLevel.gameObject.SetActive(false);
        var idx = PlayerPrefs.GetInt(gameManager.levelNum) >= gameManager.levels.Count+1
            ? Random.Range(0, gameManager.levels.Count)
            : PlayerPrefs.GetInt(gameManager.levelNum)-1;
        gameManager.currentLevel = Instantiate(gameManager.levels[idx]);
        InputManager.Instance.mainCamera = Camera.main;
        PlayerPrefs.SetInt(gameManager.lastPlayedLevel, idx);
        panels[3].panelTexts[0].text = "Level " + PlayerPrefs.GetInt(gameManager.levelNum);
        ClosePanel(panels[1]);
        gameManager.UpdateGameState(GameManager.GameState.Play);
    }


    public void OpenPanel(Panel panel)
    {
        if (panel == panels[0])
        {
            ClosePanel(panels[3]);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Settings);
            StartCoroutine(SettingsPanelCoroutine());
        }
        else if (panel == panels[1])
        {
            //will do fade in
            ClosePanel(panels[3]);
            // settings panel needs to be hidden if game won while looking at the settings menu.
            panels[0].gameObject.transform.DOLocalMoveX(1080, 0.5f);
            panel.gameObject.SetActive(true);
        }
        else
        {
            panel.gameObject.SetActive(true);
        }

        // if panel == panels[idx] do someting (ex. change gamestate)
        // Settings and victory panels will have  proper tweens make seperate methods
    }

    private IEnumerator SettingsPanelCoroutine()
    {
        panels[0].gameObject.transform.DOLocalMoveX(0, 0.5f);
        yield return new WaitForSeconds(0.2f);
        foreach (var elements in panels[0].panelElements)
        {
            elements.transform.DOPunchPosition(Vector3.left*50, 0.5f,0,1,true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ClosePanel(Panel panel)
    {
        if (panel == panels[0])
        {
            panels[0].gameObject.transform.DOLocalMoveX(1080, 0.5f);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Play);
        }
        else
        {
            panel.gameObject.SetActive(false);
        }
        // Settings and victory panels will have proper tweens
        // if panel == panels[idx] do someting (ex. change gamestate)
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

    public void UpdateCoins()
    {
        DOVirtual.Int(GameManager.Instance.totalCoins, PlayerPrefs.GetInt(GameManager.Instance.coinAmount), 2, (x) =>
        {
            GameManager.Instance.totalCoins = x;
            panels[1].panelTexts[1].text = "+" + x;
        });
        panels[0].panelTexts[0].text = PlayerPrefs.GetInt(GameManager.Instance.coinAmount).ToString();
    }
}