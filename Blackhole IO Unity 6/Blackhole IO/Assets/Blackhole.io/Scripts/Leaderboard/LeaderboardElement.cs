using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardElement : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI leaderboardText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        leaderboardText.text = text;
    }

    public void SetColor(Color color)
    {
        leaderboardText.color = color;
    }
}
