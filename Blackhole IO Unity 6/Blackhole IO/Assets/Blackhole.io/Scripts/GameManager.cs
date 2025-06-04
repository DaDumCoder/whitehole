using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header(" Managers ")]
    [SerializeField] private UIManager uiManager;

    [Header(" Elements ")]
    [SerializeField] private GameObject nextButton;

    [Header(" Canvases ")]
    [SerializeField] private CanvasGroup finalLeaderboardCG;

    [Header(" Coins ")]
    [SerializeField] private ParticleControl coinsParticles;
    [SerializeField] private RectTransform coinRT;

    private void Awake()
    {
        TimersManager.OnGameTimerEnded += ShowFinalLeaderboard;
        Player.OnPlayerGotEaten += PlayerGotEatenCallback;

        coinsParticles.onControlledParticlesEnded += ShowNextButton;
    }

    private void OnDestroy()
    {
        TimersManager.OnGameTimerEnded -= ShowFinalLeaderboard;
        Player.OnPlayerGotEaten -= PlayerGotEatenCallback;

        coinsParticles.onControlledParticlesEnded -= ShowNextButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager.SetGame();
        Implementation.Instance.ShowBanner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowFinalLeaderboard()
    {
        LeanTween.alphaCanvas(finalLeaderboardCG, 1, .5f).setOnComplete(FinalLeaderboardShownCallback);
    }

    private void FinalLeaderboardShownCallback()
    {
        Utilsjet.EnableCG(finalLeaderboardCG);

        int rewardCoins = (int)FindObjectOfType<PlayerController>().GetComponent<Player>().GetSize();
        coinsParticles.PlayControlledParticles(Utilsjet.GetScreenCenter(), coinRT, rewardCoins);
    }

    public void PlayerGotEatenCallback(bool isMainPlayer)
    {
        if (isMainPlayer)
            ShowFinalLeaderboard();
    }

    private void ShowNextButton()
    {
        nextButton.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
