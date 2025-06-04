using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Customization : MonoBehaviour
{
    public static Customization instance;

    public static UnityAction<Skin> OnLastPlayerSkinLoaded;

    [Header(" Skins ")]
    [SerializeField] private Skin[] skins;

    [Header(" Player Names ")]
    [SerializeField] private TextAsset gamerTagsTextFile;
    private string[] gamerTagsList;
    private Skin playerSkin;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        MenuUIManager.OnPlayerNameChanged += PlayerNameChangedCallback;
        ShopManager.OnSkinSelected += SkinSelectedCallback;
    }

    private void OnDestroy()
    {
        MenuUIManager.OnPlayerNameChanged -= PlayerNameChangedCallback;
        ShopManager.OnSkinSelected -= SkinSelectedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        StoreGamerTags();
        SetLastSkin();
    }

    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StoreGamerTags()
    {
        string gamerTagsString = gamerTagsTextFile.text;
        gamerTagsList = gamerTagsString.Split('\n');     
    }

    private void PlayerNameChangedCallback(string playerName)
    {
        SavePlayerName(playerName);    
    }

    private void SavePlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
    }

    public string GetMainPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    private void SkinSelectedCallback(Skin skin)
    {
        playerSkin = skin;

        // Save this data too
        int playerSkinIndex = 0;
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].sprite == playerSkin.sprite)
            {
                playerSkinIndex = i;
                break;
            }
        }

        PlayerPrefs.SetInt("PlayerSkinIndex", playerSkinIndex);
    }

    public void SetLastSkin()
    {
        int lastSkinIndex = PlayerPrefs.GetInt("PlayerSkinIndex");
        playerSkin = skins[lastSkinIndex];

        OnLastPlayerSkinLoaded?.Invoke(playerSkin);
    }

    public Skin GetMainPlayerSkin()
    {
        return playerSkin;
    }

    public string GetRandomPlayerName()
    {
        string gamerTag = gamerTagsList[Random.Range(0, gamerTagsList.Length)];
        gamerTag = gamerTag.Substring(0, gamerTag.Length - 1);
        return gamerTag;
    }

    public Skin GetRandomSkin()
    {
        return skins[Random.Range(0, skins.Length)];
    }

    public Skin[] GetSkins()
    {
        return skins;
    }

}

[System.Serializable]
public struct Skin
{
    public Color color;
    public Sprite sprite;
    public Sprite icon;
    public Vector3 localPosition;
    public Vector3 localScale;
    public int price;
}