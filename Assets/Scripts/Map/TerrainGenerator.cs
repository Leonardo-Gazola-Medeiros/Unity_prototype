using UnityEngine;
using System.Collections.Generic;

public class TerrainChunkGenerator : MonoBehaviour
{
    public Transform player;             // Reference to the player
    public int chunkSize = 100;          // Width & length of each chunk
    public int height = 20;              // Max terrain height
    public float noiseScale = 60f;       // How "stretchy" the noise is
    public int renderDistance = 2;       // How many chunks away to load

    private Dictionary<Vector2, GameObject> terrainChunks = new Dictionary<Vector2, GameObject>();

    void Update()
    {
        GenerateChunksAroundPlayer();
    }

    void GenerateChunksAroundPlayer()
    {
        // Find player's current chunk position
        int playerX = Mathf.FloorToInt(player.position.x / chunkSize);
        int playerZ = Mathf.FloorToInt(player.position.z / chunkSize);

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2 chunkCoord = new Vector2(playerX + x, playerZ + z);

                if (!terrainChunks.ContainsKey(chunkCoord))
                {
                    CreateChunk(chunkCoord);
                }
            }
        }
    }

    void CreateChunk(Vector2 coord)
    {
        // Create GameObject for terrain chunk
        GameObject chunk = new GameObject("Chunk_" + coord);
        chunk.transform.position = new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);

        // Add Terrain + TerrainData
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = chunkSize + 1;
        terrainData.size = new Vector3(chunkSize, height, chunkSize);

        // Fill with Perlin Noise
        float[,] heights = new float[chunkSize + 1, chunkSize + 1];
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                float xCoord = (coord.x * chunkSize + x) / noiseScale;
                float zCoord = (coord.y * chunkSize + z) / noiseScale;
                heights[x, z] = Mathf.PerlinNoise(xCoord, zCoord);
            }
        }
        terrainData.SetHeights(0, 0, heights);

        // Add terrain component
        Terrain terrain = chunk.AddComponent<Terrain>();
        terrain.terrainData = terrainData;

        // Add collider
        TerrainCollider collider = chunk.AddComponent<TerrainCollider>();
        collider.terrainData = terrainData;

        // Store in dictionary
        terrainChunks.Add(coord, chunk);
    }
}
