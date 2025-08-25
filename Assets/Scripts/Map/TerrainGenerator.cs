using UnityEngine;
using System.Collections.Generic;

public class TerrainChunkGenerator : MonoBehaviour
{
    public Transform player;             // Reference to the player
    public int chunkSize = 100;          // Width & length of each chunk
    public float heightMultiplier = 20f; // Max terrain height
    public float noiseScale = 20f;       // Controls terrain smoothness
    public int renderDistance = 2;       // How many chunks away to load

    private Dictionary<Vector2, GameObject> terrainChunks = new Dictionary<Vector2, GameObject>();

    void Update()
    {
        GenerateChunksAroundPlayer();
    }

    void GenerateChunksAroundPlayer()
    {
        // Find player's current chunk position in chunk grid
        int playerX = Mathf.FloorToInt(player.position.x / (chunkSize - 1));
        int playerZ = Mathf.FloorToInt(player.position.z / (chunkSize - 1));

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2 chunkCoord = new Vector2(playerX + x, playerZ + z);

                if (!terrainChunks.ContainsKey(chunkCoord))
                {
                    GameObject chunk = GenerateChunk((int)chunkCoord.x, (int)chunkCoord.y);
                    terrainChunks.Add(chunkCoord, chunk);
                }
            }
        }
    }

    public GameObject GenerateChunk(int chunkX, int chunkZ)
    {
        GameObject chunk = new GameObject($"Chunk_{chunkX}_{chunkZ}");
        chunk.transform.position = new Vector3(chunkX * (chunkSize - 1), 0, chunkZ * (chunkSize - 1));

        MeshFilter mf = chunk.AddComponent<MeshFilter>();
        MeshRenderer mr = chunk.AddComponent<MeshRenderer>();
        MeshCollider mc = chunk.AddComponent<MeshCollider>();

        mr.material = new Material(Shader.Find("Standard"));

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[chunkSize * chunkSize];
        int[] triangles = new int[(chunkSize - 1) * (chunkSize - 1) * 6];

        int t = 0;
        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int i = z * chunkSize + x;

                // ✅ FIX: use world-space sampling with division, not multiplication
                float worldX = (x + chunkX * (chunkSize - 1)) / noiseScale;
                float worldZ = (z + chunkZ * (chunkSize - 1)) / noiseScale;

                float y = Mathf.PerlinNoise(worldX, worldZ) * heightMultiplier;

                vertices[i] = new Vector3(x, y, z);

                if (x < chunkSize - 1 && z < chunkSize - 1)
                {
                    triangles[t++] = i;
                    triangles[t++] = i + chunkSize;
                    triangles[t++] = i + 1;

                    triangles[t++] = i + 1;
                    triangles[t++] = i + chunkSize;
                    triangles[t++] = i + chunkSize + 1;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mf.mesh = mesh;
        mc.sharedMesh = mesh;

        return chunk;
    }
}
