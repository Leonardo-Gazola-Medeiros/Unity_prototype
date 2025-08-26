using UnityEngine;
using System.Collections.Generic;

public class ChunkLoader : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public TerrainGenerator generator;    // Reference to TerrainGenerator script
    public int viewDistance = 3;          // How many chunks around the player are visible
    public int unloadDistance = 5;        // Distance after which chunks are destroyed

    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        if (player == null || generator == null) return;

        Vector2Int playerChunkCoord = new Vector2Int(
            Mathf.FloorToInt(player.position.x / generator.chunkSize),
            Mathf.FloorToInt(player.position.z / generator.chunkSize)
        );

        HashSet<Vector2Int> chunksToKeep = new HashSet<Vector2Int>();

        // Load nearby chunks
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(playerChunkCoord.x + x, playerChunkCoord.y + y);
                chunksToKeep.Add(chunkCoord);

                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    GameObject chunk = generator.GenerateChunk(chunkCoord.x, chunkCoord.y);
                    loadedChunks.Add(chunkCoord, chunk);
                }
            }
        }

        // Unload distant chunks
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var kvp in loadedChunks)
        {
            float dist = Vector2Int.Distance(playerChunkCoord, kvp.Key);
            if (dist > unloadDistance)
            {
                Destroy(kvp.Value);
                chunksToRemove.Add(kvp.Key);
            }
        }

        foreach (var coord in chunksToRemove)
        {
            loadedChunks.Remove(coord);
        }
    }
}
