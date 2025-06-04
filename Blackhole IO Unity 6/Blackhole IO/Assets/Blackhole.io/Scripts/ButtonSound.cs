using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    public static UnityAction OnButtonClicked;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnButtonClicked?.Invoke());
    }
}
