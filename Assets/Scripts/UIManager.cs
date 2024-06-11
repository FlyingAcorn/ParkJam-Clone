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
    // you should do animation and tween related stuff in seperate methods.(tutorial is missing for now)
    public void RestartLevel()
    {
        var gameManager = GameManager.Instance;
        gameManager.currentLevel.gameObject.SetActive(false);
        gameManager.currentLevel = Instantiate(gameManager.levels[gameManager.LastPlayedLevel]);
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
        gameManager.currentLevel = Instantiate(gameManager.levels[gameManager.LastPlayedLevel]);
        InputManager.Instance.mainCamera = Camera.main;
        ClosePanel(panels[1]);
        gameManager.UpdateGameState(GameManager.GameState.Play);
        OpenPanel(panels[3]);
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
            ClosePanel(panels[3]);
            ClosePanel(panels[1]);
            StartCoroutine(VictoryPanelCoroutine());
        }
        else
        {
            panel.gameObject.SetActive(true);
        }
    }

    private IEnumerator SettingsPanelCoroutine()
    {
        panels[0].gameObject.transform.DOLocalMoveX(0, 0.5f);
        yield return new WaitForSeconds(0.2f);
        foreach (var elements in panels[0].panelElements)
        {
            elements.transform.DOPunchPosition(Vector3.left * 50, 0.5f, 0, 1, true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator VictoryPanelCoroutine()
    {
        panels[1].gameObject.SetActive(true);
        foreach (var t in panels[1].panelTexts)
        {
            t.DOFade(1, 1);
        }

        foreach (var t in panels[1].panelElements)
        {
            t.DOFade(1, 1);
        }

        panels[1].panelImage.DOFade(0.5f, 1);

        yield return new WaitForSeconds(1);
        panels[1].buttons[0].gameObject.SetActive(true);
    }

    public void ClosePanel(Panel panel)
    {
        if (panel == panels[0])
        {
            OpenPanel(panels[3]);
            panels[0].gameObject.transform.DOLocalMoveX(1080, 0.5f);
            if (GameManager.Instance.state != GameManager.GameState.Victory)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Play);
            }
        }
        else if (panel == panels[1])
        {
            foreach (var t in panels[1].panelTexts)
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
            }

            foreach (var t in panels[1].panelElements)
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
            }

            panel.buttons[0].gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
        else
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void DeleteCache()
    {
        PlayerPrefs.DeleteAll();
        GameManager.Instance.totalCoins = 0;
        RestartLevel();
        panels[3].panelTexts[0].text = "Level " + GameManager.Instance.LevelNo;
        UpdateCoins();
    }

    public void ToggleSound(bool tog)
    {
        SoundManager.Instance.audioSource.mute =!tog;
    }

    public void ToggleHaptics(bool tog)
    {
        //its just a placeholder
        HapticsManager.Instance.disableHaptics = !tog;
    }

    public void UpdateCoins()
    {
        DOVirtual.Int(GameManager.Instance.totalCoins, GameManager.Instance.CoinsAmount, 2, (x) =>
        {
            GameManager.Instance.totalCoins = x;
            panels[1].panelTexts[1].text = "+" + x;
        });
        panels[0].panelTexts[0].text = GameManager.Instance.CoinsAmount.ToString();
    }
}