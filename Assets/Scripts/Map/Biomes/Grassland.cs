using UnityEngine;

public class GrasslandTerrain : MonoBehaviour
{
    public int width = 200;
    public int depth = 200;
    public int height = 20; // Lower height = flatter terrain
    public float scale = 30f; // Larger scale = smoother hills
    public int octaves = 4;
    public float persistence = 0.5f; // How much each octave contributes
    public float lacunarity = 2.0f; // Frequency multiplier per octave
    public int seed = 42;

    private void Start()
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
        terrainData.size = new Vector3(width, height, depth);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        System.Random prng = new System.Random(seed);
        float offsetX = prng.Next(-100000, 100000);
        float offsetZ = prng.Next(-100000, 100000);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                heights[x, z] = GeneratePerlinValue(x, z, offsetX, offsetZ);
            }
        }
        return heights;
    }

    float GeneratePerlinValue(int x, int z, float offsetX, float offsetZ)
    {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++)
        {
            float sampleX = (x + offsetX) / scale * frequency;
            float sampleZ = (z + offsetZ) / scale * frequency;

            float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
            noiseHeight += perlinValue * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return (noiseHeight + 1) / 2f; // Normalize 0–1
    }
}
