using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour
{
	[Header(" Settings ")]
	[SerializeField] private float gridSize;
	[SerializeField] private float isoLevel;
	public int width;
	public int height;

	[Header(" Components ")]
	[SerializeField] private Transform raycastablePlane;


	float[,] map;
	float[,] initialMap;

	void Start()
	{
		raycastablePlane.localScale = new Vector3(width * gridSize, height * gridSize, 1);

		//GenerateMap();
	}

	public void Initialize(float gridSize, int width, int height, float isoLevel)
	{
		this.gridSize = gridSize;
		this.width = width;
		this.height = height;
		this.isoLevel = isoLevel;

		GenerateMap();
	}

	void GenerateMap()
	{
		//ConfigureCircleMap();
		ConfigureSquareMap();
		//map = new float[width, height];

		UpdateMesh();
	}

	private void ConfigureCircleMap()
    {
		map = new float[width, height];
		initialMap = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
				Coord coord = new Coord(x, y);
				Vector3 worldCoordPos = CoordToWorldPoint(coord);// + transform.position;

				map[x, y] = Mathf.Exp(-worldCoordPos.sqrMagnitude / 5) - isoLevel - .1f;
				initialMap[x, y] = map[x, y];

				/*
				if (worldCoordPos.sqrMagnitude > 5)
					map[x,y] = Mathf.Exp(-worldCoordPos.sqrMagnitude / 5) - isoLevel - .1f;
				*/
			}
        }

		//initialMap = map;
	}

	private void ConfigureSquareMap()
    {
		map = new float[width, height];

	}

	public void Dig(Vector3 holePosition, int radius)
	{
		//DigLine(holePosition, holePosition, radius);

		Vector3 rotatedCurrentHolePosition = transform.rotation * holePosition;
		//Debug.Log("Local : " + rotatedCurrentHolePosition);
		//Debug.Log("World : " + transform.TransformPoint(rotatedCurrentHolePosition));

		Coord holeCoord = WorldToCoordPoint(holePosition);
		DrawCircle(holeCoord, radius);

		//UpdateMesh();	 
	}

	public void Dig(Vector3[] holesPositions, int[] radiuses)
    {
		Coord[] holesCoords = new Coord[holesPositions.Length];

        for (int i = 0; i < holesCoords.Length; i++)
			holesCoords[i] = WorldToCoordPoint(holesPositions[i]);

		DrawHoles(holesCoords, radiuses);
    }

	public void DigLine(Vector3 currentHolePosition, Vector3 previousHolePosition, int radius)
    {
		Coord from = WorldToCoordPoint(currentHolePosition);
		Coord to = WorldToCoordPoint(previousHolePosition);

		List<Coord> holeCoords = GetLine(from, to);

		foreach (Coord coord in holeCoords)
			DrawCircle(coord, radius);

		//UpdateMesh();
    }

	public float[,] GetMap()
    {
		return map;
    }

	public float[,] GetInitialMap()
    {
		return initialMap;
    }

	public void SetMap(float[,] map)
    {
		this.map = map;
    }

	public void UpdateMesh()
	{
		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(map, gridSize, isoLevel);
	}

	void DrawCircle(Coord c, int r)
	{
		for (int x = -r; x <= r; x++)
		{
			for (int y = -r; y <= r; y++)
			{

				int drawX = c.tileX + x;
				int drawY = c.tileY + y;

				if (IsInMapRange(drawX, drawY))
				{
					Vector3 centerTilePos = CoordToWorldPoint(c);
					Vector3 tilePos = CoordToWorldPoint(new Coord(drawX, drawY));

					float distance = Vector3.Distance(tilePos, centerTilePos);
					//map[drawX, drawY] = isoLevel - (Mathf.Exp(-(distance * distance) / r * r) - (isoLevel * 1.1f));

					map[drawX, drawY] = -1;

					//map[drawX, drawY] = Mathf.Exp(-(distance * distance) / (float)r) - isoLevel - .1f;

					//map[drawX, drawY] -= (r - distance) / r;
				}
			}
		}
	}

	private void DrawHoles(Coord[] coordsArray, int[] radiusesArray)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
				bool squareIsInAHole = false;

                // Check each if this square is within a hole radius
                for (int holeIndex = 0; holeIndex < coordsArray.Length; holeIndex++)
                {
					Coord currentCoord = coordsArray[holeIndex];
					int currentHoleRadius = radiusesArray[holeIndex];

					if (x >= currentCoord.tileX - currentHoleRadius && x <= currentCoord.tileX + currentHoleRadius && y >= currentCoord.tileY - currentHoleRadius && y <= currentCoord.tileY + currentHoleRadius)
                    {
						// The square is in a hole, dig it and check the next square
						map[x, y] = -1;
						squareIsInAHole = true;
						break;
                    }
                }

				if (!squareIsInAHole)
					map[x, y] = 0;
            }
        }
    }

	List<Coord> GetLine(Coord from, Coord to)
	{
		List<Coord> line = new List<Coord>();

		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;
		int step = Math.Sign(dx);
		int gradientStep = Math.Sign(dy);

		int longest = Mathf.Abs(dx);
		int shortest = Mathf.Abs(dy);

		if (longest < shortest)
		{
			inverted = true;
			longest = Mathf.Abs(dy);
			shortest = Mathf.Abs(dx);

			step = Math.Sign(dy);
			gradientStep = Math.Sign(dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0; i < longest; i++)
		{
			line.Add(new Coord(x, y));

			if (inverted)
			{
				y += step;
			}
			else
			{
				x += step;
			}

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest)
			{
				if (inverted)
				{
					x += gradientStep;
				}
				else
				{
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}

		return line;
	}

	/*
	Vector3 CoordToWorldPoint(Coord tile)
	{
		return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
	}*/
	

	
	Vector3 CoordToWorldPoint(Coord tile)
	{
		//int nodeCountX = width;// map.GetLength(0);
		//int nodeCountY = height;// map.GetLength(1);

		float mapWidth = width * gridSize;
		float mapHeight = height * gridSize;

		Vector3 pos = new Vector3(-mapWidth / 2 + tile.tileX * gridSize + gridSize / 2, 0, -mapHeight / 2 + tile.tileY * gridSize + gridSize / 2);
		pos += transform.position;

		//Vector3 rotatedPos = pos * transform.rotation;

		//return rotatedPos;
		return pos;
	}
	

	Coord WorldToCoordPoint(Vector3 worldPoint)
	{
		Coord c = new Coord();

		//worldPoint -= transform.position;
		Vector3 correctedWorldPoint = transform.InverseTransformPoint(worldPoint);

		float mapWidth = width * gridSize - gridSize;
		float mapHeight = height * gridSize - gridSize;

		c.tileX = (int)((correctedWorldPoint.x + (mapWidth / 2)) / gridSize);
		c.tileY = (int)((correctedWorldPoint.z + (mapHeight / 2f)) / gridSize);

		return c;
	}

	/*
	Coord WorldToCoordPoint(Vector3 worldPoint)
	{
		Coord c = new Coord();

		worldPoint -= transform.position;

		float mapWidth = width * gridSize - gridSize;
		float mapHeight = height * gridSize - gridSize;

		c.tileX = (int)((worldPoint.x + (mapWidth / 2)) / gridSize);
		c.tileY = (int)((worldPoint.z + (mapHeight / 2f)) / gridSize);

		return c;
	}
	*/


	bool IsInMapRange(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int x, int y)
		{
			tileX = x;
			tileY = y;
		}

		public override string ToString()
		{
			return tileX + " - " + tileY;
		}
	}
}
