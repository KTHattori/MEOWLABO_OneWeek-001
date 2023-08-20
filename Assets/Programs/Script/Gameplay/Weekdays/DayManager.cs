using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    FadeTransition startDayFade = new FadeTransition();

    

    public int CurrentDay { get => currentDay; set => currentDay = value; }
    public WeekDay CurrentWeekDay { get => currentWeekDay; set => currentWeekDay = value; }

    private bool dayIsShown = false;

    private int taskRemained = 0;

    private float timer = 0.0f;

    private bool isTitleFade = false;

    

    void Start()
    {    
        HideDay();
        dayStartFade.EventList.onTransitOutEnd.events.AddListener(HideDay);
        dayStartFade.EventList.onTransitEnd.events.AddListener(EmergeGhost);
        dayStartFade.BaseInfo.inOutType = Transition.IOType.OutIn;

        fadeOut.EventList.onTransitOutEnd.events.AddListener(ShowDay);
        fadeOut.BaseInfo.inOutType = Transition.IOType.OutIn;

        startDayFade.EventList.onTransitOutEnd.events.AddListener(FirstDay);

        currentDay = (int)WeekDay.Monday;
        currentWeekDay = WeekDay.Monday;

        timer = 0.0f;
        isTitleFade = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(!dayIsShown) return;
        timer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Return) || timer > 3.0f)
        {
            StartToday();
        }
    }

    public void StartTitle()
    {
        dayChangePanel.ActivateObject();
    }

    void UpdateTitle()
    {
        timer += Time.deltaTime;
        dayChangePanel.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f - timer / 5.0f;
        if(timer > 5.0f)
        {
            HideDay();
            isTitleFade = false;
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
        ShowDay();
    }

    public void NextDay()
    {
        if(TransitionManager.instance.IsTransition) return;
        currentDay++;
        if(currentDay > (int)WeekDay.Friday)
        {
            currentDay = (int)WeekDay.Monday;
            currentWeekDay = WeekDay.Monday;
            GameAdministrator.GoTrue();
            return;
        }
        currentWeekDay = (WeekDay)currentDay;
        SceneController.instance.SetLoadSceneName(currentWeekDay.ToString());
        SceneController.TransitToScene(fadeOut);
    }

    public void StartToday()
    {
        TransitionManager.SetBeginTransit(dayStartFade);
        dayIsShown = false;
        timer = 0.0f;
    }

    public void ShowDay(float dummy = 1.0f)
    {
        dayChangePanel.ActivateObject();
        dayText.SetText(currentWeekDay.ToString().ToUpper());
        dayIsShown = true;
        timer = 0.0f;
    }

    public void HideDay(float dummy = 0.0f)
    {
        if(dayChangePanel.gameObject == null) return;
        if(dayChangePanel.gameObject.activeSelf == false) return;
        dayText.SetText("");
        dayChangePanel.DeactivateObject();
        dayIsShown = false;
    }

    public void EmergeGhost(float dummy = 1.0f)
    {
        Ghost.StartEmerge();
    }

    static public void SetWeekday(WeekDay weekDay)
    {
        instance.SetDay(weekDay);
        SceneController.instance.SetLoadSceneName(weekDay.ToString());
        SceneController.TransitToScene(instance.fadeOut);
    }

    public void StartFirstDay()
    {
        taskRemained = 0;
        SetDay(WeekDay.Monday);
        SceneController.instance.SetLoadSceneName(currentWeekDay.ToString());
        SceneController.TransitToScene(startDayFade);
    }

    public static void AddRemainingCount(int amount)
    {
        instance.taskRemained += amount;
        if(amount > 0)
        {
            GameAdministrator.GoBad();
        }
    }

    public static bool IsRemainingZero()
    {
        return instance.taskRemained == 0;
    }
}
