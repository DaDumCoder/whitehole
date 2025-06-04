using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEater : MonoBehaviour
{
    [Header(" Managers ")]
    [SerializeField] private Player player;


    [Header(" Settings ")]
    [SerializeField] private LayerMask eatableObjectsMask;
    [SerializeField] private LayerMask playersMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectEatableObjects();
        DetectEatablePlayers();
    }

    private void DetectEatableObjects()
    {
        int maxColliders = 20;
        Collider[] detectedEatableObjects = new Collider[maxColliders]; 
        int detectedEatableObjectsCount = Physics.OverlapSphereNonAlloc(transform.position, player.GetSize() / 2, detectedEatableObjects, eatableObjectsMask);

        for (int i = 0; i < detectedEatableObjectsCount; i++)
        {
            MeshRenderer eatableObjectRenderer = detectedEatableObjects[i].GetComponent<MeshRenderer>();

            if (eatableObjectRenderer == null)
                continue;

            Vector2 projectedBounds = new Vector2(eatableObjectRenderer.bounds.size.x, eatableObjectRenderer.bounds.size.z);

            float eatableObjectsBoundsMagnitude = projectedBounds.magnitude;

            if (eatableObjectsBoundsMagnitude > player.GetSize() * 1.9f)
                continue;

            detectedEatableObjects[i].GetComponent<EatableObject>().EnablePhysics();        
        }
    }

    private void DetectEatablePlayers()
    {
        int maxColliders = 20;
        Collider[] detectedPlayers = new Collider[maxColliders];
        int detectedPlayersCount = Physics.OverlapSphereNonAlloc(transform.position, player.GetSize() / 2, detectedPlayers, playersMask);

        for (int i = 0; i < detectedPlayersCount; i++)
        {
            Player detectedPlayer = detectedPlayers[i].GetComponent<Player>();

            if (detectedPlayer == GetComponent<Player>())
                continue;

            // Check who's the biggest
            float otherPlayerSize = detectedPlayer.GetSize();
            float thisSize = GetComponent<Player>().GetSize();

            if (thisSize < otherPlayerSize + 1)
                continue;

            // Now perform a distance check, the other player needs to be inside this player to be eaten
            float distanceBetweenPlayer = Vector3.Distance(transform.position, detectedPlayer.transform.position);
            float otherPlayerRadius = otherPlayerSize / 2;
            float thisPlayerRadius = thisSize / 2;

            if(distanceBetweenPlayer + otherPlayerRadius/2 < thisPlayerRadius)
                EatPlayer(detectedPlayer);
        }
    }

    private void EatPlayer(Player playerToEat)
    {
        GetComponent<Player>().IncreaseScore(playerToEat.GetSize());
        playerToEat.GetEaten();

        Taptic.Medium();
        //Destroy(playerToEat.gameObject);
    }
}
