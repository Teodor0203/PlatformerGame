using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnergyBar : MonoBehaviour
{
    public static UI_EnergyBar instance { get; private set; }

    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Slider energyBar;
    private int restoreDuration = 10;
    public int maxEnergy = 5;
    public int currentEnergy;
    public DateTime nextEnergyTime;
    public DateTime lastEnergyTime;
    private bool isRestoring = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadEnergyState();
    }

    void Start()
    {

        if (!PlayerPrefs.HasKey("CurrentEnergy"))
        {
            PlayerPrefs.SetInt("CurrentEnergy", maxEnergy);
            //SaveEnergyState();
            StartCoroutine(RestoreEnergyCoroutine());
        }

        else
        {
            StartCoroutine(RestoreEnergyCoroutine());
        }
    }

    private void Update()
    {
        UpdateEnergyTimer();
    }

    public void UseEnergy()
    {
        if (currentEnergy >= 1)
        {
            currentEnergy--;
            SaveEnergyState();
            UpdateEnergy();

            if (isRestoring == false)
            {
                if (currentEnergy + 1 == maxEnergy)
                    nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);

                StartCoroutine(RestoreEnergyCoroutine());
            }
        }

        else
            Debug.Log("Insufficient energy");
    }

    public void Addenergy(int amount) //or int amount as parameter
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy); //or currentEnergy + amount
            UpdateEnergy();
            SaveEnergyState();

            if (isRestoring == false)
                StartCoroutine(RestoreEnergyCoroutine());
        }

        else
            Debug.Log("Energy is already full");
    }

    private IEnumerator RestoreEnergyCoroutine()
    {
        UpdateEnergyTimer();
        isRestoring = true;

        while (currentEnergy < maxEnergy)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime nextDateTime = nextEnergyTime;
            bool isEnergyAdding = false;

            while (currentDateTime > nextDateTime)
            {
                if (currentEnergy < maxEnergy)
                {
                    isEnergyAdding = true;
                    currentEnergy++;
                    UpdateEnergy();
                    DateTime timeToAdd = lastEnergyTime > nextDateTime ? lastEnergyTime : nextDateTime;
                    nextDateTime = AddDuration(timeToAdd, restoreDuration);
                }

                else
                    break;
            }

            if (isEnergyAdding == true)
            {
                lastEnergyTime = DateTime.Now;
                nextEnergyTime = nextDateTime;
                SaveEnergyState();
            }

            UpdateEnergyTimer();
            UpdateEnergy();
            yield return null;
        }

        isRestoring = false;

    }

    private DateTime AddDuration(DateTime dateTime, int duration)
    {
        //return dateTime.AddSeconds(duration);
        return dateTime.AddMinutes(duration);

    }

    private void UpdateEnergyTimer()
    {
        if (currentEnergy >= maxEnergy)
        {
            timerText.text = "Full";
            return;
        }

        TimeSpan time = nextEnergyTime - DateTime.Now;

        if (time < TimeSpan.Zero)
        {
            time = TimeSpan.Zero;
        }

        string timeValue = $"{time.Minutes:D2}:{time.Seconds:D2}";
        timerText.text = timeValue;
    }

    private void UpdateEnergy()
    {
        energyText.text = currentEnergy.ToString() + "/" + maxEnergy.ToString();

        energyBar.maxValue = maxEnergy;
        energyBar.value = currentEnergy;
    }

    private DateTime StringToDate(string dateTime)
    {
        if (String.IsNullOrEmpty(dateTime))
            return DateTime.Now;

        else
            return DateTime.Parse(dateTime);
    }

    private void LoadEnergyState()
    {
        if (!PlayerPrefs.HasKey("CurrentEnergy"))
        {
            currentEnergy = maxEnergy;
            nextEnergyTime = DateTime.Now;
            lastEnergyTime = DateTime.Now;
        }
        else
        {
            currentEnergy = PlayerPrefs.GetInt("CurrentEnergy");
            nextEnergyTime = StringToDate(PlayerPrefs.GetString("NextEnergyTime"));
            lastEnergyTime = StringToDate(PlayerPrefs.GetString("LastEnergyTime"));
        }

        UpdateEnergy();
    }

    public void SaveEnergyState()
    {
        Debug.Log("Energy state saved: " + currentEnergy);
        PlayerPrefs.SetInt("CurrentEnergy", currentEnergy);
        PlayerPrefs.SetString("NextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("LastEnergyTime", lastEnergyTime.ToString());
        PlayerPrefs.Save();
    }
}
