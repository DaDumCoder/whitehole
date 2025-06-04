using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(ScrollRect))]
public class ShopScrollView : MonoBehaviour
{
    public static UnityAction OnShopElementChanged;

    [Header(" Elements ")]
    private ScrollRect scrollRect;
    private RectTransform previousCenterElement;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetCenterElement().name);
    }

    public RectTransform GetCenterElement()
    {
        float closestDistance = 5000;
        int closestElementIndex = -1;

        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            float distance = Mathf.Abs(scrollRect.content.GetChild(i).position.x - Screen.width / 2);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestElementIndex = i;
            }
        }

        RectTransform centerElement = scrollRect.content.GetChild(closestElementIndex).GetComponent<RectTransform>();

        if (previousCenterElement == null)
            previousCenterElement = centerElement;
        else if(previousCenterElement != centerElement)
        {
            previousCenterElement = centerElement;
            OnShopElementChanged?.Invoke();
        }    


        return centerElement;
    }
}
