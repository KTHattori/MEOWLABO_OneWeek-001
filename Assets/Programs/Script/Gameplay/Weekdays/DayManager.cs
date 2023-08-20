using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoSingleton<DayManager>
{
    [SerializeField]
    private int currentDay = 1;
    [SerializeField]
    private WeekDay currentWeekDay = WeekDay.Monday;
    [SerializeField]
    private TMPro.TextMeshProUGUI dayText = null;
    [SerializeField]
    private SCO_ObjectRelay dayChangePanel = null;  
    [SerializeField]
    FadeTransition fadeOut = new FadeTransition();

    [SerializeField]
    FadeTransition dayStartFade = new FadeTransition();

    [SerializeField]
    CutTransition cutOut = new CutTransition();

    [SerializeField]
    CutTransition cutIn = new CutTransition();

    public int CurrentDay { get => currentDay; set => currentDay = value; }
    public WeekDay CurrentWeekDay { get => currentWeekDay; set => currentWeekDay = value; }

    

    void Start()
    {
        if(dayChangePanel.gameObject == null)
        {
            EmergeGhost();
            return;
        }
        HideDay(0);

        dayStartFade.EventList.onTransitOutInSwitch.events.AddListener(HideDay);
        dayStartFade.EventList.onTransitEnd.events.AddListener(EmergeGhost);
        dayStartFade.BaseInfo.inOutType = Transition.IOType.OutIn;

        fadeOut.EventList.onTransitEnd.events.AddListener(ShowDay);
        fadeOut.BaseInfo.inOutType = Transition.IOType.Out;

        FirstDay();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            NextDay();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartDay();
        }
    }

    public void SetDay(WeekDay weekDay)
    {
        currentWeekDay = weekDay;
        currentDay = (int)weekDay;
    }

    public void SetDay(int day)
    {
        currentDay = day;
        currentWeekDay = (WeekDay)day;
    }

    public void FirstDay(float dummy = 1.0f)
    {
        if(dayChangePanel == null) return;
        dayText = dayChangePanel.gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        currentDay = (int)WeekDay.Monday;
        currentWeekDay = WeekDay.Monday;
        ShowDay();
    }

    public void NextDay()
    {
        if(TransitionManager.instance.IsTransition) return;
        currentDay++;
        if(currentDay > (int)WeekDay.Friday)
        {
            currentDay = (int)WeekDay.Monday;
        }
        currentWeekDay = (WeekDay)currentDay;
        TransitionManager.SetBeginTransit(fadeOut);
    }

    public void StartDay()
    {
        TransitionManager.SetBeginTransit(dayStartFade);
    }

    public void ShowDay(float dummy = 1.0f)
    {
        dayChangePanel.ActivateObject();
        dayText.SetText(currentWeekDay.ToString().ToUpper());
    }

    public void HideDay(float dummy = 0.0f)
    {
        if(dayChangePanel.gameObject == null) return;
        if(dayChangePanel.gameObject.activeSelf == false) return;
        dayText.SetText("");
        dayChangePanel.DeactivateObject();
    }

    public void EmergeGhost(float dummy = 1.0f)
    {
        Ghost.StartEmerge();
    }
}
