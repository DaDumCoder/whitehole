using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [Header(" Sounds ")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource shopElementChangedSound;
    [SerializeField] private AudioSource skinPurchasedSound;

    [SerializeField] private AudioSource moveLoopSound;
    [SerializeField] private AudioSource countdownSound;
    [SerializeField] private AudioSource goSound;

    [SerializeField] private AudioSource eatSound;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        ButtonSound.OnButtonClicked = PlayButtonSound;
        ShopScrollView.OnShopElementChanged += PlayShopElementSound;
        ShopManager.OnSkinPurchased += PlaySkinPurchasedSound;

        PlayerController.OnPlayerMovementChanged += ManageMoveLoopSound;
        Player.OnPlayerAteSomething += PlayEatSound;

        TimersManager.OnBeforeGameTimerUpdated += PlayCountdownSound;
    }

    private void OnDestroy()
    {
        ShopScrollView.OnShopElementChanged -= PlayShopElementSound;
        ShopManager.OnSkinPurchased -= PlaySkinPurchasedSound;

        PlayerController.OnPlayerMovementChanged -= ManageMoveLoopSound;
        Player.OnPlayerAteSomething -= PlayEatSound;

        TimersManager.OnBeforeGameTimerUpdated -= PlayCountdownSound;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayButtonSound()
    {
        buttonSound.Play();
    }

    private void PlayShopElementSound()
    {
        shopElementChangedSound.Play();
    }

    private void PlaySkinPurchasedSound(int none)
    {
        skinPurchasedSound.Play();
    }

    private void ManageMoveLoopSound(bool playerIsMoving)
    {
        if (playerIsMoving)
            moveLoopSound.Play();
        else
            moveLoopSound.Stop();
    }

    private void PlayCountdownSound(int beforeGameTimer)
    {
        if (beforeGameTimer > 0)
            countdownSound.Play();
        else if (beforeGameTimer == 0)
            goSound.Play();
    }

    private void PlayEatSound(bool isMainPlayer)
    {
        if(isMainPlayer)
            eatSound.Play();
    }
}
