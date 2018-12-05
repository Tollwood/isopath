using UnityEngine;

public class UiManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject gameInfo;

    private CameraOrbit cameraOrbit;

    private void Start()
    {
        cameraOrbit = FindObjectOfType<CameraOrbit>();
    }

    public void OnNewGame(){
        mainMenu.SetActive(false);
        gameInfo.SetActive(true);
        cameraOrbit.shouldOrbit = false;
        cameraOrbit.Reset();
    }

}
