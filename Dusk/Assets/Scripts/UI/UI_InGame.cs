using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;
    public UI_FadeEffect fadeEffect { get; private set; } // read-only

    [Header("Timer and Items")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI batteriesText;

    [Header("UI to show")]
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject batteriesNotCollectedUI;

    [Header("Energy UI")]
    [SerializeField] private GameObject noEnergyLeftUI;
    [SerializeField] private GameObject addEnergyUI;

    private bool isPaused;

    private void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    private void Start()
    {
        fadeEffect.ScreenFade(0, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PauseButton();
    }

    public void PauseButton()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
    }

    

    public void ShowNoEnergyMessage()
    {
        if (UI_EnergyBar.instance.currentEnergy == UI_EnergyBar.instance.maxEnergy || UI_EnergyBar.instance.currentEnergy > 0)
            return;

        noEnergyLeftUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void DeactivateNoEnergyMessage() => noEnergyLeftUI.SetActive(false);

    public void ShowAddEnergyUI()
    {
        if (UI_EnergyBar.instance.currentEnergy == UI_EnergyBar.instance.maxEnergy)
            return;

        addEnergyUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideAddenergyUI() => addEnergyUI.SetActive(false);

    public void ShowBatteriesNotCollectedMessage() => StartCoroutine(BatteriesNotCollected());

    private IEnumerator BatteriesNotCollected()
    {
        batteriesNotCollectedUI.SetActive(true);

        yield return new WaitForSeconds(2);

        batteriesNotCollectedUI.SetActive(false);
    }

    public void GoToMainMenuButton()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void UpdateBatteryUI(int collectedBatteries, int totalBatteries)
    {
        batteriesText.text = collectedBatteries + "/" + totalBatteries;
    }

    public void UpdateTimerUI(float timer)
    {
        timerText.text = timer.ToString("00") + " s";
    }
}
