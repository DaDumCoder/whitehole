using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager instance;

    public static UnityAction<string> OnPlayerNameChanged;

    [Header(" Elements ")]
    [SerializeField] private InputField playerNameInputField;
    [SerializeField] private Text menuCoinsText;
    private int coins;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        UpdateUI();

        playerNameInputField.onValueChanged.AddListener(PlayerNameChangedCallback);

        ShopManager.OnSkinPurchased += UseCoins;
    }

    private void OnDestroy()
    {
        ShopManager.OnSkinPurchased -= UseCoins;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerNameInputField.text = Customization.instance.GetMainPlayerName();
        Implementation.Instance.ShowBanner();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AddCoins(580);
    }

    private void UpdateUI()
    {
        menuCoinsText.text = coins.ToString();
    }

    private void PlayerNameChangedCallback(string playerName)
    {
        OnPlayerNameChanged?.Invoke(playerName);
    }

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt("COINS");
    }

    public int GetCoins()
    {
        return coins;
    }

    public void UseCoins(int amount)
    {
        coins -= amount;

        UpdateUI();
        
        SaveData();
    }

    public void AddCoins(int amount)
    {
        UseCoins(-amount);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("COINS", coins);
    }
}
