using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText;

    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI batteriesText;

    private int levelIndex;
    private string sceneName;

    public void SetupButton(int newLevelIndex)
    {
        levelIndex = newLevelIndex;

        levelNumberText.text = "Level " + levelIndex;
        sceneName = "Level_" + levelIndex;

        Debug.Log("Button setup");
        bestTimeText.text = TimerInfoText();
        batteriesText.text = BatteriesInfoText();
    }

    public void LoadLevel()
    {
        UI_EnergyBar.instance.UseEnergy();

        AudioManager.instance.PlaySFX(4);

        int difficultyIndex = ((int)DifficultyManager.instance.difficulty);
        PlayerPrefs.SetInt("GameDifficulty", difficultyIndex);
        SceneManager.LoadScene(sceneName);

        Debug.Log("Level is loading");
    }

    private string BatteriesInfoText()
    {
        int totalBatteries = PlayerPrefs.GetInt("Level" + levelIndex + "TotalBatteries", 0);
        string totalBatteriesText = totalBatteries == 0 ? "?" : totalBatteries.ToString();

        int batteriesCollected = PlayerPrefs.GetInt("Level" + levelIndex + "BatteriesCollected");

        return "Batteries: " + batteriesCollected + " / " + totalBatteriesText;
    }

    private string TimerInfoText()
    {
        float timerValue = PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime");

        string timerValueText = (int)timerValue == 0 ? "--" : timerValue.ToString("00");

        return "Best Time: " + timerValueText + " s";
    }
}
