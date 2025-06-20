﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JetSystems
{
	public class UIManager : MonoBehaviour
	{
        public enum Orientation { Portrait, Landscape }
        public Orientation orientation;

        public enum GameState { MENU, GAME, LEVELCOMPLETE, GAMEOVER, SETTINGS, SHOP }
        public static GameState gameState;

        #region Static Variables

        public static int COINS;
        public static UIManager instance;

        #endregion

        #region Delegates

        public delegate void SetMenuDelegate();
        public static SetMenuDelegate setMenuDelegate;

        public delegate void OnMenuSet();
        public static OnMenuSet onMenuSet;



        public delegate void SetGameDelegate();
        public static SetGameDelegate setGameDelegate;

        public delegate void OnGameSet();
        public static OnGameSet onGameSet;



        public delegate void SetLevelCompleteDelegate(int starsCount = 3);
        public static SetLevelCompleteDelegate setLevelCompleteDelegate;

        public delegate void OnLevelCompleteSet(int starsCount = 3);
        public static OnLevelCompleteSet onLevelCompleteSet;



        public delegate void SetGameoverDelegate();
        public static SetGameoverDelegate setGameoverDelegate;

        public delegate void OnGameoverSet();
        public static OnGameoverSet onGameoverSet;


        public delegate void SetSettingsDelegate();
        public static SetSettingsDelegate setSettingsDelegate;

        public delegate void OnSettingsSet();
        public static OnSettingsSet onSettingsSet;



        public delegate void UpdateProgressBarDelegate(float value);
        public static UpdateProgressBarDelegate updateProgressBarDelegate;



        public delegate void OnNextLevelButtonPressed();
        public static OnNextLevelButtonPressed onNextLevelButtonPressed;

        public delegate void OnRetryButtonPressed();
        public static OnRetryButtonPressed onRetryButtonPressed;

        #endregion


        // Canvas Groups
        public CanvasGroup MENU;
        public CanvasGroup GAME;
        public CanvasGroup LEVELCOMPLETE;
        public CanvasGroup GAMEOVER;
        public CanvasGroup SETTINGS;
        public ShopManager shopManager;
        public CanvasGroup[] canvases;

        // Menu UI
        public Text menuCoinsText;

        // Game UI
        public Slider progressBar;
        public Text gameCoinsText;
        public Text levelText;

        // Shop UI
        public Text shopCoinsText;

        // Level Complete UI
        public Text levelCompleteCoinsText;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            // Get the coins amount
            COINS = PlayerPrefsManager.GetCoins();
            UpdateCoins();

            // Store the canvases
            canvases = new CanvasGroup[] { MENU, GAME, LEVELCOMPLETE, GAMEOVER, SETTINGS };

            // Configure the delegates
            ConfigureDelegates();
        }

        // Start is called before the first frame update
        void Start()
		{

            // Set the menu at start
            //SetMenu();
		}

        private void ConfigureDelegates()
        {
            // Basic events
            setMenuDelegate += SetMenu;
            setGameDelegate += SetGame;
            setLevelCompleteDelegate += SetLevelComplete;
            setGameoverDelegate += SetGameover;
            setSettingsDelegate += SetSettings;

            // Progress bar events
            updateProgressBarDelegate += UpdateProgressBar;
        }

        private void OnDestroy()
        {
            // Basic events
            setMenuDelegate -= SetMenu;
            setGameDelegate -= SetGame;
            setLevelCompleteDelegate -= SetLevelComplete;
            setGameoverDelegate -= SetGameover;
            setSettingsDelegate -= SetSettings;

            // Progress bar events
            updateProgressBarDelegate -= UpdateProgressBar;
        }


        // Update is called once per frame
        void Update()
		{
            if (Input.GetKeyDown(KeyCode.C))
                SetLevelComplete();
		}

        public void SetMenu()
        {
            gameState = GameState.MENU;
            Utilsjet.HideAllCGs(canvases, MENU);

            // Hide the shop
            shopManager.gameObject.SetActive(false);

            // Invoke the delegate
            onMenuSet?.Invoke();
        }

        public void SetGame()
        {
            gameState = GameState.GAME;
            Utilsjet.HideAllCGs(canvases, GAME);

            // Invoke the delegate
            onGameSet?.Invoke();

            // Reset the progress bar
            progressBar.value = 0;

            // Update the level text
            levelText.text = "Level " + (PlayerPrefsManager.GetLevel() + 1);
        }

        public void SetLevelComplete(int starsCount = 3)
        {
            gameState = GameState.LEVELCOMPLETE;
            Utilsjet.HideAllCGs(canvases, LEVELCOMPLETE);

            // Invoke the delegate
            onLevelCompleteSet?.Invoke(starsCount);
        }

        public void SetGameover()
        {
            //Implementation.Instance.ShowInterstitial();
            gameState = GameState.GAMEOVER;
            Utilsjet.HideAllCGs(canvases, GAMEOVER);

            // Invoke the delegate
            onGameoverSet?.Invoke();
        }

        public void SetSettings()
        {
            gameState = GameState.SETTINGS;
            Utilsjet.EnableCG(MENU);
            Utilsjet.HideAllCGs(canvases, SETTINGS);

            // Invoke the delegate
            onSettingsSet?.Invoke();
        }

        public void SetShop()
        {
            gameState = GameState.SHOP;

            // Enable the shop gameobject
            shopManager.gameObject.SetActive(true);

            // Hide all the other canvases
            Utilsjet.HideAllCGs(canvases);
        }


        public void CloseShop()
        {
            // Disable the shop object
            shopManager.gameObject.SetActive(false);

            // Get back to the menu
            SetMenu();
        }

        public void NextLevelButtonCallback()
        {
            SetMenu();

            // Invoke the next button delegate
            onNextLevelButtonPressed?.Invoke();
        }

        public void RetryButtonCallback()
        {
            SetMenu();

            // Invoke the retry button delegate
            onRetryButtonPressed?.Invoke();
        }

        public void CloseSettings()
        {
            SetMenu();
        }

        public void UpdateProgressBar(float value)
        {
            progressBar.value = value;
        }

        private void UpdateCoins()
        {
            menuCoinsText.text = Utilsjet.FormatAmountString(COINS);
            gameCoinsText.text = menuCoinsText.text;
            shopCoinsText.text = menuCoinsText.text;
            levelCompleteCoinsText.text = menuCoinsText.text;
        }

        #region Static Methods

        public static void AddCoins(int amount)
        {
            // Increase the amount of coins
            COINS += amount;

            // Update the coins
            instance.UpdateCoins();

            // Save the amount of coins
            PlayerPrefsManager.SaveCoins(COINS);
        }

        public static bool IsGame()
        {
            return gameState == GameState.GAME;
        }

        public static bool IsLevelComplete()
        {
            return gameState == GameState.LEVELCOMPLETE;
        }

        public static bool IsGameover()
        {
            
            return gameState == GameState.GAMEOVER;
        }

        #endregion
    }


}