using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour {

    private Game game;

    public Toggle diggerAiToggle;
    public Toggle climberAiToggle;
    public Slider boardSizeSlider;
    public TextMeshProUGUI boardSizeValue;

	void Start () {
        game = FindObjectOfType<Game>();
        diggerAiToggle.isOn = game.gameConfig.aiDigger;
        climberAiToggle.isOn = game.gameConfig.aiClimber;
        boardSizeSlider.value = game.gameConfig.size;
        boardSizeValue.text = game.gameConfig.size + "";
	}
	
    public void OnDiggerAIChanged(){

        game.gameConfig.aiDigger = !game.gameConfig.aiDigger;
    }

    public void OnClimberAIChanged()
    {
        game.gameConfig.aiClimber = !game.gameConfig.aiClimber;
    }


    public void OnBoardSizeChanged()
    {
        game.gameConfig.size = (int)boardSizeSlider.value;
        boardSizeValue.text = game.gameConfig.size +"";
        game.Restart();
    }
}
