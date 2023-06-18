using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HatchItem : MonoBehaviour
{
    private HatchItem current;
    private bool countdown;

    [SerializeField] private Slider progressSlider;
    [SerializeField] private Button skipButton;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private Button collect;

    private Timer timer;
    public Egg hatchedEgg;

    private void Awake()
    {
        current = this;
        TimerInit();
    }
    private void Start()
    {
        gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = hatchedEgg.Name;
        gameObject.transform.Find("Image").GetComponent<Image>().sprite = hatchedEgg.Icon;
    }
    private void FixedUpdate()
    {
        //if there has to be a countdown display
        if (countdown)
        {
            //set the slider value
            progressSlider.value = (float)(1.0 - timer.secondsLeft / timer.timeToFinish.TotalSeconds);
            //display countdown text
            timeLeftText.text = timer.DisplayTime();
        }
    }
    private void Finish()
    {
        progressSlider.gameObject.SetActive(false);
        collect.gameObject.SetActive(true);
        //Dodaj tablice dla hatched eggs czyli tych które s¹ gotowe do otwarcia ale nie otwarte
    }
    private void TimerInit()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(hatchedEgg.Name, DateTime.Now, hatchedEgg.productionTime);
        timer.TimerFinishedEvent.AddListener(delegate
        {
            Finish();
            Destroy(timer);
        });
        timer.StartTimer();

        //countdown text has to be visible
        countdown = true;
        FixedUpdate();

    }

    //Skip logic
    public void SkipButton()
    {
        //add listeners for both events
        EventManager.Instance.AddListenerOnce<EnoughCurrencyGameEvent>(OnEnoughCurrency);
        EventManager.Instance.AddListenerOnce<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);

        //initialize event infor
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-timer.skipAmount, CurrencyType.Crystals);
        //invoke the event of currency change
        EventManager.Instance.QueueEvent(info);
    }
    //if the player has enough currency
    private void OnEnoughCurrency(EnoughCurrencyGameEvent info)
    {
        //skip timer
        timer.SkipTimer();
        //disable skip button
        skipButton.gameObject.SetActive(false);
        //remove listener so the object doesn't listen to this event anymore
        EventManager.Instance.RemoveListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
    }

    //if the player doesn't have enough currency
    private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent info)
    {
        //remove listener so the object doesn't listen to this event anymore
        EventManager.Instance.RemoveListener<EnoughCurrencyGameEvent>(OnEnoughCurrency);
    }

    //Open egg
    public void OpenEgg()
    {
        StorageManager.current.UpdateItems(hatchedEgg.hatchedChicken.thatChicken, true);
        Destroy(gameObject);
    }
}
