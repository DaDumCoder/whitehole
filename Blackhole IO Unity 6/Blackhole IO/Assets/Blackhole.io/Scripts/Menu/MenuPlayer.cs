using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private SpriteRenderer holeSpriteRenderer;
    [SerializeField] private SpriteRenderer contourSpriteRenderer;

    private void Awake()
    {
        ShopManager.OnSkinSelected += OnSkinSelectedCallback;
        Customization.OnLastPlayerSkinLoaded += OnSkinSelectedCallback;
    }

    private void OnDestroy()
    {
        ShopManager.OnSkinSelected -= OnSkinSelectedCallback;
        Customization.OnLastPlayerSkinLoaded -= OnSkinSelectedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        //OnSkinSelectedCallback(Customization.instance.GetMainPlayerSkin());
    }

    private void OnEnable()
    {
        if(Customization.instance != null)
            Customization.instance.SetLastSkin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSkinSelectedCallback(Skin skin)
    {
        contourSpriteRenderer.sprite = skin.sprite;

        contourSpriteRenderer.transform.localPosition = skin.localPosition;
        contourSpriteRenderer.transform.localScale = skin.localScale;
    }
}
