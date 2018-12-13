using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject gameInfo;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    private Game game;
    private CameraOrbit cameraOrbit;

    private void Start()
    {
        game = FindObjectOfType<Game>();
        cameraOrbit = FindObjectOfType<CameraOrbit>();
    }


    public void OnMainMenu()
    {
        game.Restart();
        gameInfo.SetActive(false);
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        cameraOrbit.startOrbit();
    }

    public void BackToMenu()
    {
        gameInfo.SetActive(false);
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void OnSettingsMenu()
    {
        gameInfo.SetActive(false);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnPause()
    {
        gameInfo.SetActive(false);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        game.OnPause();
    }

    public void OnNewGame()
    {
        gameInfo.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        game.OnNewGame();
    }

    public void OnPlay()
    {
        gameInfo.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        game.OnPlay();
    }
}
