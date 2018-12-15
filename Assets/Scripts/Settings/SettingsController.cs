using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{

    private Game game;
    public Toggle diggerAiToggle;
    public Toggle climberAiToggle;
    public Slider boardSizeSlider;
    public TextMeshProUGUI boardSizeValue;

    void Start()
    {
        game = FindObjectOfType<Game>();


        diggerAiToggle.isOn = game.settings.aiDigger;
        climberAiToggle.isOn = game.settings.aiClimber;
        boardSizeSlider.value = game.settings.size;
        boardSizeValue.text = game.settings.size + "";

    }

    public void OnDiggerAIChanged()
    {

        game.settings.aiDigger = !game.settings.aiDigger;
        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(game.settings));
    }

    public void OnClimberAIChanged()
    {
        game.settings.aiClimber = !game.settings.aiClimber;
        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(game.settings));
    }


    public void OnBoardSizeChanged()
    {
        game.settings.size = (int)boardSizeSlider.value;
        boardSizeValue.text = game.settings.size + "";
        PlayerPrefs.SetString("Settings", JsonUtility.ToJson(game.settings));
        game.Restart();
    }
}
