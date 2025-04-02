using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private UI_InGame inGameUI;

    [Header("Level Managment")]
    [SerializeField] private float levelTimer;
    [SerializeField] private int currentLevelIndex;
    private int nextLevelIndex;

    [Header("Batteries Management")]
    public bool fruitsAreRandom;
    public int batteriesCollected;
    public int totalBatteries;
    public Transform batteriesParent;

    [Header("Coin Management")]
    public int coinsCollected;
    public int totalCoins;

    [Header("Diamond Management")]
    public int diamondsCollected;
    public int totalDiamonds;


    [Header("Checkpoints")]
    public bool canReactivate;

    [Header("Managers")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private DifficultyManager difficultyManager;
    [SerializeField] private ObjectCreator objectCreator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); ;
    }

    private void Start()
    {
        inGameUI = UI_InGame.instance;

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;


        nextLevelIndex = currentLevelIndex + 1;

        CollectBatteriesInfo();
        CreateManagersIfNeeded();
    }

    private void Update()
    {
        levelTimer += Time.deltaTime;

        inGameUI.UpdateTimerUI(levelTimer);
    }

    private void CreateManagersIfNeeded()
    {
        if (AudioManager.instance == null)
            Instantiate(audioManager);

        if (PlayerManager.instance == null)
            Instantiate(playerManager);

        if (SkinManager.instance == null)
            Instantiate(skinManager);

        if (DifficultyManager.instance == null)
            Instantiate(difficultyManager);

        if (ObjectCreator.instance == null)
            Instantiate(objectCreator);
    }

    private void CollectBatteriesInfo()
    {
        Battery[] allBatteries = FindObjectsByType<Battery>(FindObjectsSortMode.None);
        totalBatteries = allBatteries.Length;

        inGameUI.UpdateBatteryUI(batteriesCollected, totalBatteries);

        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalBatteries", totalBatteries);
    }

    [ContextMenu("Parent All Batteries")]
    private void ParentAllTheBatteries()
    {
        if (batteriesParent == null)
            return;

        Battery[] allBatteries = FindObjectsByType<Battery>(FindObjectsSortMode.None);

        foreach (Battery battery in allBatteries)
        {
            battery.transform.parent = batteriesParent;
        }
    }

    public void AddBattery()
    {
        batteriesCollected++;
        inGameUI.UpdateBatteryUI(batteriesCollected, totalBatteries);
    }

    public void AddCoin(int amount)
    {
        coinsCollected += amount;
        //inGameUI.UpdateCoinsUI(PlayerPrefs.GetInt("TotalCoinsAmount"));
    }

    /* public void AddDiamond(int amount)
     {
         diamondsCollected += amount;
         inGameUI.UpdateDiamondUI(diamondsCollected);
     }*/

    public void RemoveFruit()
    {
        batteriesCollected--;
        inGameUI.UpdateBatteryUI(batteriesCollected, totalBatteries);
    }

    public int FruitsCollected() => batteriesCollected;

    public int CoinsCollected() => coinsCollected;

    //public int DiamondsCollected() => diamondsCollected;

    public bool FruitsHaveRandomLook() => fruitsAreRandom;

    public void LevelFinished()
    {
        if (UI_EnergyBar.instance.currentEnergy > 0)
        {
            UI_EnergyBar.instance.UseEnergy();

            UI_EnergyBar.instance.SaveEnergyState();

            LoadNextScene();
        }
        else
            UI_InGame.instance.ShowNoEnergyMessage();

        SaveLevelProgression();
        SaveBestTime();
        SaveBatteriesInfo();
        SaveCoinsInfo();
        //SaveDiamondsInfo();
    }

    private void SaveBatteriesInfo()
    {
        int batteriesCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "BatteriesCollected");

        if (batteriesCollectedBefore < batteriesCollected)
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "BatteriesCollected", batteriesCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalBatteriesAmount");
        PlayerPrefs.SetInt("TotalBatteriesAmount", totalFruitsInBank + batteriesCollected);
    }

    private void SaveCoinsInfo()
    {
        int coinsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "CoinsCollected");

        if (coinsCollectedBefore < coinsCollected)
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "CoinsCollected", coinsCollected);

        int totalCoinsInBank = PlayerPrefs.GetInt("TotalCoinsAmount");
        PlayerPrefs.SetInt("TotalCoinsAmount", totalCoinsInBank + coinsCollected);
    }

    /*private void SaveDiamondsInfo()
    {
        int diamondsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "DiamondsCollected");

        if (diamondsCollectedBefore < diamondsCollected)
            PlayerPrefs.SetInt("Level" + currentLevelIndex + "DiamondsCollected", diamondsCollected);

        int totalDiamondsInBank = PlayerPrefs.GetInt("TotalDiamondsAmount");
        PlayerPrefs.SetInt("TotalDiamondsAmount", totalDiamondsInBank + diamondsCollected);
    }*/


    private void SaveBestTime()
    {
        float lastTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime", 99);

        if (levelTimer < lastTime)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTimer);
    }

    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);

        if (NoMoreLevels() == false)
        {
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);


            SkinManager skinManager = SkinManager.instance;

            if (skinManager != null)
                PlayerPrefs.SetInt("LastUsedSkin", skinManager.GetSkinId());
        }
    }

    public void RestartLevel() => UI_InGame.instance.fadeEffect.ScreenFade(1, .75f, LoadCurrentScene);

    private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);

    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    private void LoadNextLevel() => SceneManager.LoadScene("Level_" + nextLevelIndex);

    private void LoadNextScene()
    {
        Time.timeScale = 1;

        UI_FadeEffect fadeEffect = UI_InGame.instance.fadeEffect;

        if (NoMoreLevels())
            fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
        else
            fadeEffect.ScreenFade(1, 1.5f, LoadNextLevel);
    }

    private bool NoMoreLevels()
    {
        int lastLevelIndex = SceneManager.sceneCountInBuildSettings - 2; // We have main menu and The End scene, that's why we use number 2
        bool noMoreLevels = currentLevelIndex == lastLevelIndex;

        return noMoreLevels;
    }
}
