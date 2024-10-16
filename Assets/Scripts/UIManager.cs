using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Panel[] panels;
    [SerializeField] private Image emojiObject;
    // panel indexes = setting 0,victory 1, tutorial 2 , inplay 3, shop 4, mainmenu 5, daily Reward 6
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
        GameManager.Instance.VehicleList();
        InputManager.Instance.mainCamera = Camera.main;
        ClosePanel(panels[1]);
        gameManager.UpdateGameState(GameManager.GameState.Play);
        OpenPanel(panels[3]);
    }

    public void OpenPanel(Panel panel)
    {
        if (panel == panels[0]) // settings
        {
            ClosePanel(panels[3]);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Settings);
            StartCoroutine(SettingsPanelCoroutine());
        }
        else if (panel == panels[1]) // victory
        {
            ClosePanel(panels[3]);
            ClosePanel(panels[1]);
            StartCoroutine(VictoryPanelCoroutine());
        }
        else if (panel == panels[4]) // shop
        {
            ClosePanel(panels[3]);
            GameManager.Instance.UpdateGameState(GameManager.GameState.Settings);
             // shop routine
        }
        else if (panel == panels[5]) // main menu
        {
            // bir tane level gibi yap arabalar sürekli hareket edip gelsin
            // menu biraz saydam olsun
            // başka bir gamescene de yapabilirsin
        }
        else if (panel == panels[6]) // daily rewards
        {
             // yukarı doğru kaysın
             // main menu sahnesinde çıksın
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
        if (panel == panels[0]) // settings
        {
            OpenPanel(panels[3]);
            panels[0].gameObject.transform.DOLocalMoveX(1080, 0.5f);
            if (GameManager.Instance.state != GameManager.GameState.Victory)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Play);
            }
        }
        else if (panel == panels[1]) // victory
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
        else if (panel == panels[4]) // shop
        {
            OpenPanel(panels[3]);
            panels[0].gameObject.transform.DOLocalMoveX(1080, 0.5f);
            if (GameManager.Instance.state != GameManager.GameState.Victory)
            {
                GameManager.Instance.UpdateGameState(GameManager.GameState.Play);
            }
        }
        else if (panel == panels[5]) // main menu
        {
             // bir tane level gibi yap arabalar sürekli hareket edip gelsin
             // menu biraz saydam olsun
             // başka bir gamescene de yapabilirsin
        }
        else if (panel == panels[6]) // daily Rewards
        {
            // main menu sahnesinde çıksın
            // aşağıya doğru kaysın
        }
        else
        {
            panel.gameObject.SetActive(false);
        }
    }

    public IEnumerator TutorialSequence() // you can do a better system later
    {
        panels[2].gameObject.SetActive(true);
        var selectedVehicle =
            GameManager.Instance.parkedVehicles[Random.Range(0, GameManager.Instance.parkedVehicles.Count)];
        var screenPoint1 = RectTransformUtility.WorldToScreenPoint(Camera.main, selectedVehicle.transform.position);
        foreach (var image in panels[2].panelElements)
        {
            transform.position = new Vector3(screenPoint1.x, screenPoint1.y+150);
        }
        panels[2].panelElements[0].transform.DOLocalMoveY
            (panels[2].panelElements[0].transform.localPosition.y + 10, 1).SetLoops(12, LoopType.Yoyo);
        panels[2].panelElements[1].transform.DOLocalMoveX
            (panels[2].panelElements[1].transform.localPosition.x + 25, 1).SetLoops(12, LoopType.Yoyo);
        panels[2].panelElements[2].transform.DOLocalMoveX
            (panels[2].panelElements[2].transform.localPosition.x -25, 1).SetLoops(12, LoopType.Yoyo);
        yield return new WaitForSeconds(12);
        panels[2].gameObject.SetActive(false);
        
    }

    public void StartGame()
    {   panels[5].gameObject.SetActive(false);
        GameManager.Instance.mainMenuBackGround.transform.gameObject.SetActive(false);
        GameManager.Instance.currentLevel = Instantiate(GameManager.Instance.levels[GameManager.Instance.LastPlayedLevel]);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Play);
        GameManager.Instance.totalCoins = GameManager.Instance.CoinsAmount;
        UpdateCoins();
        panels[3].panelTexts[0].text = "Level " + GameManager.Instance.LevelNo;
        if (GameManager.Instance.LevelNo == 1) StartCoroutine(UIManager.Instance.TutorialSequence()); 
        GameManager.Instance.VehicleList();
        SoundManager.Instance.PlayBackgroundSound(1);
        
    }
    public void LeaveGame()
    {
        Application.Quit();
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
        SoundManager.Instance.sfxSource.mute = !tog;
        SoundManager.Instance.backgroundSource.mute = !tog;
    }

    public void ToggleHaptics(bool tog)
    {
        //its just a placeholder
        HapticsManager.Instance.disableHaptics = !tog;
    }

    public void EmojiPopupOnCrash(Vector3 movingObject, Vector3 hitObject)
    {
        var screenPoint1 = RectTransformUtility.WorldToScreenPoint(Camera.main, movingObject);
        Instantiate(emojiObject, new Vector3(screenPoint1.x, screenPoint1.y), Quaternion.identity, panels[3].transform);
        var screenPoint2 = RectTransformUtility.WorldToScreenPoint(Camera.main, hitObject);
        Instantiate(emojiObject, new Vector3(screenPoint2.x, screenPoint2.y), Quaternion.identity, panels[3].transform);
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