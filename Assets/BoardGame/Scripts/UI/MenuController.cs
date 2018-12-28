using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject gameInfo;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject settingsMenu;

    private Game game;
    private CameraOrbit cameraOrbit;

    private void Start()
    {
        game = FindObjectOfType<Game>();
        cameraOrbit = FindObjectOfType<CameraOrbit>();
    }



    public void OnGameOverMenu(){
        allToFalse();
        gameOverMenu.SetActive(true);
    }

    public void OnMainMenu()
    {
        game.Restart();
        cameraOrbit.startOrbit();
        allToFalse();
        mainMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        allToFalse();
        mainMenu.SetActive(true);
    }

    public void OnSettingsMenu()
    {
        allToFalse();
        settingsMenu.SetActive(true);
    }

    public void OnPause()
    {
        game.OnPause();
        allToFalse();
        pauseMenu.SetActive(true);
    }

    public void OnNewGame()
    {
        game.OnNewGame();
        allToFalse();
        gameInfo.SetActive(true);
    }

    public void OnPlay()
    {
        game.OnPlay();
        allToFalse();
        gameInfo.SetActive(true);
    }

    private void allToFalse(){
        gameInfo.SetActive(false);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }
}
