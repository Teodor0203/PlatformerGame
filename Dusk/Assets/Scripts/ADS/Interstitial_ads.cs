using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interstitial_ads : MonoBehaviour
{
    [SerializeField] private int timeToSkip = 1;

    void Start()
    {
        int skipNumber = PlayerPrefs.GetInt("Interstitial", timeToSkip);
        if (skipNumber != 0)
        {
            skipNumber -= 1;
            PlayerPrefs.SetInt("Interstitial", skipNumber);
        }
        else
        {
            LoadInterstitialAd();
            PlayerPrefs.SetInt("Interstitial", timeToSkip);
        }

    }

    private void OnEnable()
    {
        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
    }

    private void LoadInterstitialAd()
    {
        IronSource.Agent.loadInterstitial();
    }

    private void ShowInterstitialAd()
    {
        if (IronSource.Agent.isInterstitialReady() && timeToSkip == 1)
        {
            IronSource.Agent.showInterstitial();
            Time.timeScale = 0;
        }

        else
        {
            Debug.Log("Ad not ready");
        }
    }

    /************* Interstitial AdInfo Delegates *************/
    // Invoked when the interstitial ad was loaded succesfully.
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        ShowInterstitialAd();
    }
    // Invoked when the initialization process has failed.
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
    }
    // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the ad failed to show.
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the interstitial ad closed and the user went back to the application screen.
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Time.timeScale = 1;
    }
    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
    // This callback is not supported by all networks, and we recommend using it only if  
    // it's supported by all networks you included in your build. 
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
    }

}
