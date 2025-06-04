using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JetSystems;
using TMPro;

public class TimersManager : MonoBehaviour
{
    public static TimersManager instance;



    [Header(" Events ")]
    public static UnityAction OnBeforeGameTimerEnded;
    public static UnityAction OnGameTimerEnded;
    public static UnityAction<int> OnBeforeGameTimerUpdated;

    [Header(" Settings ")]
    private Timer beforeGameTimer;
    private Timer gameTimer;

    [Header(" UI Elements ")]
    [SerializeField] private GameObject beforeGameTimerParent;
    [SerializeField] private TextMeshProUGUI beforeGameTimerText;
    [SerializeField] private TextMeshProUGUI gameTimerText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        beforeGameTimer = new Timer();
        gameTimer = new Timer();

        beforeGameTimer.OnTimerUpdatedEvent += UpdateBeforeGameTimer;
        beforeGameTimer.OnTimerEndedEvent += BeforeGameTimerEndedCallback;

        gameTimer.OnTimerUpdatedEvent += UpdateGameTimer;
        gameTimer.OnTimerEndedEvent += GameTimerEndedCallback;

        UIManager.onGameSet += StartBeforeGameTimer;
    }

    private void OnDestroy()
    {
        beforeGameTimer.OnTimerUpdatedEvent -= UpdateBeforeGameTimer;
        beforeGameTimer.OnTimerEndedEvent -= BeforeGameTimerEndedCallback;

        gameTimer.OnTimerUpdatedEvent -= UpdateGameTimer;
        gameTimer.OnTimerEndedEvent -= GameTimerEndedCallback;

        UIManager.onGameSet -= StartBeforeGameTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError(beforeGameTimer.)
        beforeGameTimer.Update();
        gameTimer.Update();
    }

    private void StartBeforeGameTimer()
    {
        beforeGameTimer.StartTimer(3);
    }

    private void UpdateBeforeGameTimer(float timer)
    {
        beforeGameTimerText.text = ((int)timer).ToString();
        OnBeforeGameTimerUpdated?.Invoke((int)timer);
    }

    private void BeforeGameTimerEndedCallback()
    {
        beforeGameTimerParent.SetActive(false);

        OnBeforeGameTimerEnded?.Invoke();

        StartGameTimer();
    }

    private void StartGameTimer()
    {
        gameTimer.StartTimer(120);
    }

    private void UpdateGameTimer(float timer)
    {
        gameTimerText.text = FormatSeconds(Mathf.Max((int)timer, 0));
    }

    private void GameTimerEndedCallback()
    {
        OnGameTimerEnded?.Invoke();
        //UIManager.setLevelCompleteDelegate?.Invoke(3);
    }

    private string FormatSeconds(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        string minutesString = minutes >= 10 ? minutes.ToString() : "0" + minutes.ToString();
        string secondsString = seconds >= 10 ? seconds.ToString() : "0" + seconds.ToString();

        return minutesString + ":" + secondsString;
    }

}
