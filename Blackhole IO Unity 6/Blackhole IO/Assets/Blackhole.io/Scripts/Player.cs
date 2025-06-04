using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    /// <summary>
    /// The bool represents if it's the main player or not
    /// </summary>
    public static UnityAction<bool> OnPlayerGotEaten;

    /// <summary>
    /// bool : isMainPlayer
    /// </summary>
    public static UnityAction<bool> OnPlayerAteSomething;

    [Header(" Managers ")]
    [SerializeField] private MonoBehaviour controller;

    [Header(" Settings ")]
    [SerializeField] private float scoreToSizeRatio;
    [SerializeField] private Vector2 minMaxSize;
    private float score;

    [Header(" Customization ")]
    [SerializeField] private SpriteRenderer holeContourRenderer;
    [SerializeField] private string playerName;
    [SerializeField] private Color playerColor;
    private bool isMainPlayer;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        isMainPlayer = GetComponent<JetSystems.JetCharacter>().IsPlayer();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore(float amount)
    {
        OnPlayerAteSomething?.Invoke(isMainPlayer);

        score += amount;
        
        float func1 = (score / 8) + 5;
        float func2 = Mathf.Max(2, Mathf.Min(func1, 40));
        float func3 = (score / 2) + 2;
        float func4 = Mathf.Min(func2, func3);

        transform.localScale = func4 * Vector3.one;

        PlusOneParticleSystem.PlayPlusOneParticles(transform.position + Vector3.up);

        Taptic.Light();
    }

    public float GetSize()
    {
        return transform.localScale.x;
    }

    public string GetName()
    {
        return playerName;
    }

    public Color GetColor()
    {
        return playerColor;
    }

    public void GetEaten()
    {
        // Disable whatever controller this have
        controller.enabled = false;

        // Disable the collider to avoid being eaten twice
        GetComponent<Collider>().enabled = false;

        // Reset the score
        //score = 0;

        // Hide the renderers
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = false;
        Debug.Log(playerName + " just got eaten");
       


        if (isMainPlayer)
            GetComponent<PlayerController>().StopMoving();

        OnPlayerGotEaten?.Invoke(isMainPlayer);
    }

    public float GetSizePercent()
    {
        return Mathf.InverseLerp(minMaxSize.x, minMaxSize.y, transform.localScale.x);
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    public void SetPlayerSkin(Skin skin)
    {
        holeContourRenderer.sprite = skin.sprite;
        playerColor = skin.color;

        holeContourRenderer.transform.localPosition = skin.localPosition;
        holeContourRenderer.transform.localScale = skin.localScale;
    }

    public bool IsMainPlayer()
    {
        return isMainPlayer;
    }
}
