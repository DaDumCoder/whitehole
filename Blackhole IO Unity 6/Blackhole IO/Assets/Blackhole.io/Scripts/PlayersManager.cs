using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetSystems;

public class PlayersManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private ColliderGrid colliderGrid;
    Vector3[] holePositions;
    float[] holeRadiuses;


    [Header(" Settings ")]
    [SerializeField] private float sizeIncreaseFactor;

    private void Awake()
    {
        PropDestroyer.OnPropDestroyed += OnPropDestroyedCallback;
    }

    private void OnDestroy()
    {
        PropDestroyer.OnPropDestroyed -= OnPropDestroyedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        holePositions = new Vector3[transform.childCount];
        holeRadiuses = new float[transform.childCount];

        CustomizePlayers();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayersData();
        colliderGrid.UpdateTerrain(holePositions, holeRadiuses);
    }

    private void UpdatePlayersData()
    {
        for (int i = 0; i < holePositions.Length; i++)
        {
            holePositions[i] = transform.GetChild(i).position;
            holeRadiuses[i] = .25f + transform.GetChild(i).localScale.x / 2;
        }
    }

    private void CustomizePlayers()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Player player = transform.GetChild(i).GetComponent<Player>();

            if (player.IsMainPlayer())
                ConfigureMainPlayer(player);
            else
                ConfigureBot(player);
        }
    }

    private void ConfigureMainPlayer(Player player)
    {
        player.SetPlayerName(Customization.instance.GetMainPlayerName());
        player.SetPlayerSkin(Customization.instance.GetMainPlayerSkin());
    }

    private void ConfigureBot(Player player)
    {
        player.SetPlayerName(Customization.instance.GetRandomPlayerName());
        player.SetPlayerSkin(Customization.instance.GetRandomSkin());
    }

    private void OnPropDestroyedCallback(Vector3 propPosition, float propSizeMagnitude)
    {
        // Find the closest player to the destroyed prop
        int closestPlayerIndex = Utilsjet.GetClosestVectorIndexInArray(propPosition, holePositions);
        transform.GetChild(closestPlayerIndex).GetComponent<Player>().IncreaseScore(propSizeMagnitude * sizeIncreaseFactor);
    }
}
