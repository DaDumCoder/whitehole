using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image image;
    [SerializeField] private GameObject selector;
    private Color color;
    Skin thisSkin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(Skin skin)
    {
        thisSkin = skin;

        image.sprite = skin.icon;//skin.sprite;
        this.color = skin.color;

        image.SetNativeSize();
    }

    public void SetSelectState(bool selectState)
    {
        selector.SetActive(selectState);
    }

    public Skin GetSkin()
    {
        return thisSkin;
    }
}
