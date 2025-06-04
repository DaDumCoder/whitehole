using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class LeaderboardUI : MonoBehaviour
{
    [Header(" Leaderboard ")]
    [SerializeField] private Leaderboard leaderboard;

    [Header(" Elements ")]
    [SerializeField] private Transform leaderboardElementsParent;
    LeaderboardElement[] leaderboardElements;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void Initialize()
    {
        leaderboardElements = new LeaderboardElement[leaderboardElementsParent.childCount];

        for (int i = 0; i < leaderboardElements.Length; i++)
            leaderboardElements[i] = leaderboardElementsParent.GetChild(i).GetComponent<LeaderboardElement>();        
    }

    public void UpdateDisplay()
    {
        LeaderboardCharacter[] leaderboardCharacters = leaderboard.GetCharactersList();

        for (int i = 0; i < leaderboardElements.Length; i++)
        {
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
