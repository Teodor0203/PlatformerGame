using UnityEngine;

public class ADS_Manager : MonoBehaviour
{
    public static ADS_Manager instance;
    public bool adWatched = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        IronSource.Agent.setManualLoadRewardedVideo(false);

        IronSource.Agent.init("1f7d0f8e5");
        IronSource.Agent.validateIntegration();
    }

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;        
    }

    private void SdkInitializationCompletedEvent() {}

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
}