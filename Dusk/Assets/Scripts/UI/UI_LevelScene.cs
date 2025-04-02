using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LvlSelection : MonoBehaviour
{
    private UI_FadeEffect fadeEffect;
    public string FirstLevelName;


    [SerializeField] private GameObject[] uiElements;

    [Header("Interactive Camera")]
    [SerializeField] private MenuCharcater menuCharacter;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private Transform mainMenuPoint;
    [SerializeField] private Transform skinSelectionPoint;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, new RefreshRate() { numerator = 60, denominator = 1 });
    }

    private void Start()
    {

        fadeEffect.ScreenFade(0, 1.5f);
    }

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        uiToEnable.SetActive(true);

        AudioManager.instance.PlaySFX(4);
    }

    public void SwitchScene(string sceneToLoad)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        SceneManager.LoadScene(sceneToLoad);

        AudioManager.instance.PlaySFX(4);

    }

    public void NewGame()
    {
        fadeEffect.ScreenFade(1, 1.5f, LoadLevelScene);
        AudioManager.instance.PlaySFX(4);
    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene(FirstLevelName);
    }

    private bool HasLevelProgression()
    {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;

        return hasLevelProgression;
    }

}
