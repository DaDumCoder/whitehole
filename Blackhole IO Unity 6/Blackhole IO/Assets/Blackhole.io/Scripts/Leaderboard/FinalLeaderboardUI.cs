using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class FinalLeaderboardUI : MonoBehaviour
{
    [Header(" Leaderboard ")]
    [SerializeField] private Leaderboard leaderboard;

    [Header(" Elements ")]
    [SerializeField] private Transform leaderboardElementsParent;
    LeaderboardElement[] leaderboardElements;

    private void Awake()
    {
        TimersManager.OnGameTimerEnded += UpdateDisplay;
        Player.OnPlayerGotEaten += UpdateDisplay;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        TimersManager.OnGameTimerEnded -= UpdateDisplay;
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateDisplay();
    }

    private void Initialize()
    {
        leaderboardElements = new LeaderboardElement[leaderboardElementsParent.childCount];

        for (int i = 0; i < leaderboardElements.Length; i++)
            leaderboardElements[i] = leaderboardElementsParent.GetChild(i).GetComponent<LeaderboardElement>();
    }

    private void UpdateDisplay(bool isMainPlayer)
    {
        if(isMainPlayer)
            UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        LeaderboardCharacter[] leaderboardCharacters = leaderboard.GetCharactersList();

        for (int i = 0; i < leaderboardElements.Length; i++)
        {
            Implementation.Instance.ShowInterstitial();
            Player player = leaderboardCharacters[i].GetComponent<Player>();

            string playerName = player.GetName();
            Color playerColor = player.GetColor();

            string playerPoints = ((int)((player.GetSize() - 2) * 1000)).ToString();

            string leaderboardText = leaderboard.GetPositionString(i + 1) + " - " + playerName + " - " + playerPoints + "pts";

            leaderboardElements[i].SetText(leaderboardText);
            leaderboardElements[i].SetColor(playerColor);
        }
    }
}

