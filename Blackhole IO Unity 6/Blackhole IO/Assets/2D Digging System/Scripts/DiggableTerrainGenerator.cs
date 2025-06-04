using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggableTerrainGenerator : MonoBehaviour
{
    [Header(" Settings ")]
    [SerializeField] private MapGenerator mapGeneratorPrefab;
    [SerializeField] private int xSubdivisions;
    [SerializeField] private int ySubdivisions;
    [SerializeField] private float gridSize;
    [SerializeField] private int chunkWidth;
    [SerializeField] private int chunkHeight;
    [SerializeField] private float isoLevel;


    [Header(" Performance ")]
    [SerializeField] private float updateDistanceThreshold;

    [Header(" Other ")]
    MapGenerator[] generatedChunks;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateTerrain()
    {
        while(transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        float xStep = chunkWidth * gridSize - gridSize;
        float zStep = chunkHeight * gridSize - gridSize;

        Vector3 initialPos = Vector3.left * xStep / 2 * (xSubdivisions - 1);

        initialPos.z = -zStep / 2 * (ySubdivisions - 1);

        generatedChunks = new MapGenerator[xSubdivisions * ySubdivisions];

        for (int x = 0; x < xSubdivisions; x++)
        {
            for (int y = 0; y < ySubdivisions; y++)
            {
                Vector3 spawnPos = initialPos + x * Vector3.right * xStep + y * Vector3.forward * zStep;
                generatedChunks[y + x * ySubdivisions] = Instantiate(mapGeneratorPrefab, spawnPos, Quaternion.identity, transform);
                generatedChunks[y + x * ySubdivisions].Initialize(gridSize, chunkWidth, chunkHeight, isoLevel);

                generatedChunks[y + x * ySubdivisions].name = "Chunk (" + x + ";" + y + ")";
            }
        }
    }

    HashSet<MapGenerator> mapGeneratorsToUpdate = new HashSet<MapGenerator>();
    public void DigAtPosition(Vector3 hitPos, int holeRadius, bool updateMeshes = false)
    {
        float worldSpaceRadius = holeRadius;
        Collider[] detectedColliders = Physics.OverlapSphere(hitPos, worldSpaceRadius / 2);
        //mapGeneratorsToUpdate.Clear();


        foreach (Collider detectedCollider in detectedColliders)
            if (detectedCollider.GetComponent<RaycastablePlane>())
            {
                detectedCollider.GetComponent<RaycastablePlane>().GetMapGenerator().Dig(hitPos, holeRadius);                    
                mapGeneratorsToUpdate.Add(detectedCollider.GetComponent<RaycastablePlane>().GetMapGenerator());
            }

        if (updateMeshes)
        {
            InterpolateChunkBorders();
            UpdateMapGenerators();
        }        
    }

    private void UpdateMapGenerators()
    {
        Debug.Log($"Updating {mapGeneratorsToUpdate.Count} map generators");

        foreach (MapGenerator mapGenerator in mapGeneratorsToUpdate)
            mapGenerator.UpdateMesh();
    }


    public void Dig(Vector3[] holesPositions, int[] holesRadiuses)
    {
        mapGeneratorsToUpdate.Clear();

    



        
        Collider[] detectedMapGenerators = GetDetectedMapGenerators(holesPositions, holesRadiuses);

        foreach (Collider detectedCollider in detectedMapGenerators)
            if (detectedCollider.GetComponent<RaycastablePlane>() != null)
            {
                RaycastablePlane detectedMapGeneratorPlane = detectedCollider.GetComponent<RaycastablePlane>();
                detectedMapGeneratorPlane.GetMapGenerator().Dig(holesPositions, holesRadiuses);



                mapGeneratorsToUpdate.Add(detectedMapGeneratorPlane.GetMapGenerator());
            }
        


        InterpolateChunkBorders();
        UpdateMapGenerators();
    }

    private Collider[] GetDetectedMapGenerators(Vector3[] holePositions, int[] holeRadiuses)
    {
        List<Collider> detectedMapGenerators = new List<Collider>();

        for (int i = 0; i < holePositions.Length; i++)
        {
            float worldSpaceRadius = (float)holeRadiuses[i];
            detectedMapGenerators.AddRange(Physics.OverlapSphere(holePositions[i], worldSpaceRadius));
        }

        return detectedMapGenerators.ToArray();
    }

    public void DigLine(Vector3 hitPos, Vector3 previousPos, int holeRadius)
    {
        mapGeneratorsToUpdate.Clear();

        Vector3 direction = hitPos - previousPos;
        float distance = direction.magnitude;
        int steps = 1;
        float distanceStep = distance / steps;

        for (int i = 0; i < steps; i++)
        {
            Vector3 digPosition = previousPos + direction.normalized * distanceStep * i;
            DigAtPosition(digPosition, holeRadius);
        }

        InterpolateChunkBorders();
        UpdateMapGenerators();
    }



    private void InterpolateChunkBorders()
    {
        if (generatedChunks == null || generatedChunks.Length <= 0)
            return;

        for (int x = 0; x < xSubdivisions; x++)
        {
            for (int y = 0; y < ySubdivisions; y++)
            {
                int currentChunkIndex = y + x * ySubdivisions;
                int rightNeighborChunkIndex = y + (x + 1) * ySubdivisions;
                int aboveNeighborChunkIndex = (y + 1) + x * ySubdivisions;

                if (!ChunkIndexOutOfBounds(rightNeighborChunkIndex))
                    InterpolateRightBorder(currentChunkIndex, rightNeighborChunkIndex);

                if (!ChunkIndexOutOfBounds(aboveNeighborChunkIndex))
                    InterpolateAboveBorder(currentChunkIndex, aboveNeighborChunkIndex);

            }
        }
    }

    private void InterpolateRightBorder(int chunkIndex, int rightChunkIndex)
    {
        MapGenerator chunk = generatedChunks[chunkIndex];
        MapGenerator rightChunk = generatedChunks[rightChunkIndex];

        float[,] chunkMap = chunk.GetMap();
        float[,] rightChunkMap = rightChunk.GetMap();

        int x = chunkWidth - 1;

        for (int y = 0; y < chunkHeight; y++)
        {
            float halfValue = (chunkMap[x, y] + rightChunkMap[0, y])/2;

            chunkMap[x, y] = halfValue;
            rightChunkMap[0, y] = halfValue;

        }

        chunk.SetMap(chunkMap);
        rightChunk.SetMap(rightChunkMap);
    }

    private void InterpolateAboveBorder(int chunkIndex, int aboveChunkIndex)
    {
        MapGenerator chunk = generatedChunks[chunkIndex];
        MapGenerator aboveChunk = generatedChunks[aboveChunkIndex];

        float[,] chunkMap = chunk.GetMap();
        float[,] aboveChunkMap = aboveChunk.GetMap();

        int y = chunkHeight - 1;

        for (int x = 0; x < chunkWidth; x++)
        {
            float halfValue = (chunkMap[x, y] + aboveChunkMap[x, 0]) / 2;

            chunkMap[x, y] = halfValue;
            aboveChunkMap[x, 0] = halfValue;
        }

        chunk.SetMap(chunkMap);
        aboveChunk.SetMap(aboveChunkMap);
    }

    private bool ChunkIndexOutOfBounds(int chunkIndex)
    {
        return chunkIndex >= generatedChunks.Length;
    }

    public MapGenerator[] GetGeneratedChunks()
    {
        return generatedChunks;
    }

    public int GetChunkWidth()
    {
        return chunkWidth;
    }  
    
    public int GetChunkHeight()
    {
        return chunkHeight;
    }

    public float GetIsoLevel()
    {
        return isoLevel;
    }
}
