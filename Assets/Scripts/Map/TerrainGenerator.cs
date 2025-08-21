using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 100;         // terrain width
    public int height = 100;        // terrain length
    public int depth = 20;          // max terrain height

    public float baseScale = 20f;   // overall zoom of the terrain

    [Range(1, 8)]
    public int octaves = 3;         // how many noise layers
    [Range(0f, 1f)]
    public float persistence = 0.5f; // controls amplitude reduction per octave
    public float lacunarity = 2.0f;  // controls frequency increase per octave

    public int seed = 42;           // random seed
    public Vector2 offset;          // move terrain around

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        System.Random prng = new System.Random(seed);

        float offsetX = prng.Next(-100000, 100000) + offset.x;
        float offsetY = prng.Next(-100000, 100000) + offset.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + offsetX) / baseScale * frequency;
                    float sampleY = (y + offsetY) / baseScale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; 
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence; // decrease amplitude
                    frequency *= lacunarity; // increase frequency
                }

                // normalize values to [0,1]
                heights[x, y] = Mathf.InverseLerp(-1f, 1f, noiseHeight);
            }
        }
        return heights;
    }
}
