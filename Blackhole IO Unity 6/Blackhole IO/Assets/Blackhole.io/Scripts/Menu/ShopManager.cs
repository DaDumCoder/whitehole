using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using JetSystems;

public class ShopManager : MonoBehaviour
{
    public static UnityAction<Skin> OnSkinSelected;
    public static UnityAction<int> OnSkinPurchased;

    [Header(" Managers ")]
    [SerializeField] private ShopScrollView shopScrollView;

    [Header(" Elements ")]
    [SerializeField] private SkinContainer skinContainerPrefab;
    [SerializeField] private Transform skinContainersParent;
    [SerializeField] private Text skinPriceText;

    [Header(" Panels ")]
    [SerializeField] private CanvasGroup shopPanel;

    [Header(" Buttons ")]
    [SerializeField] private GameObject selectSkinButton;
    [SerializeField] private GameObject purchaseSkinButton;
    [SerializeField] private GameObject skinPriceContainer;


    // Start is called before the first frame update
    void Start()
    {
        InitializeUI();
    }

    // Update is called once per frame
    void Update()
    {
        ManageSelectedSkin();
    }

    private void InitializeUI()
    {
        Skin[] skins = Customization.instance.GetSkins();

        for (int i = 0; i < skins.Length; i++)
        {
            SkinContainer skinContainerInstance = Instantiate(skinContainerPrefab, skinContainersParent);
            skinContainerInstance.Configure(skins[i]);
        }
    }

    public void OpenShop()
    {
        LeanTween.alphaCanvas(shopPanel, 1, .5f).setOnComplete(() => Utilsjet.EnableCG(shopPanel));
    }

    public void CloseShop()
    {
        Utilsjet.DisableCG(shopPanel);
    }

    public void ManageSelectedSkin()
    {
        RectTransform selectedSkin = shopScrollView.GetCenterElement();
        Transform selectedSkinTransform = selectedSkin;

        for (int i = 0; i < skinContainersParent.childCount; i++)
        {
            bool isSelected = selectedSkinTransform == skinContainersParent.GetChild(i);

            if (isSelected)
                ConfigureShopData(selectedSkinTransform.GetComponent<SkinContainer>());

            skinContainersParent.GetChild(i).GetComponent<SkinContainer>().SetSelectState(isSelected);
        }
    }

    private void ConfigureShopData(SkinContainer currentSkinContainer)
    {
        // 1. Check if this skin is unlocked or not
        bool isUnlocked = PlayerPrefs.GetInt("SkinUnlocked" + currentSkinContainer.transform.GetSiblingIndex()) == 1;

        if (currentSkinContainer.GetSkin().price == 0 && !isUnlocked)
        {
            UnlockSkin(currentSkinContainer.transform.GetSiblingIndex());
            isUnlocked = true;
        }

        if(isUnlocked)
        {
            // We should show the select button instead of the purchase one
            selectSkinButton.SetActive(true);
            purchaseSkinButton.SetActive(false);

            skinPriceContainer.SetActive(false);
        }
        else
        {
            // Show the purchase button
            selectSkinButton.SetActive(false);
            purchaseSkinButton.SetActive(true);

            skinPriceContainer.SetActive(true);

            // Configure the price text
            skinPriceText.text = currentSkinContainer.GetSkin().price.ToString();
        }
    }

    public void TryPurchaseSkin()
    {
        RectTransform selectedSkinTransform = shopScrollView.GetCenterElement();
        Skin selectedSkin = selectedSkinTransform.GetComponent<SkinContainer>().GetSkin();

        int currentCoins = MenuUIManager.instance.GetCoins();

        if (currentCoins < selectedSkin.price)
            return;

        UnlockSkin(selectedSkinTransform.GetSiblingIndex());

        OnSkinPurchased?.Invoke(selectedSkin.price);

        SelectSkin();
    }

    public void SelectSkin()
    {
        RectTransform selectedSkinTransform = shopScrollView.GetCenterElement();
        Skin selectedSkin = selectedSkinTransform.GetComponent<SkinContainer>().GetSkin();

        OnSkinSelected?.Invoke(selectedSkin);
    }

    private void UnlockSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("SkinUnlocked" + skinIndex, 1);
    }

}


