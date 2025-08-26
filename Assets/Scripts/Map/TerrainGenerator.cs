using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int chunkSize = 100;          // Width & length of each chunk
    public float heightMultiplier = 20f; // Max terrain height
    public float noiseScale = 20f;       // Controls terrain smoothness
    public Material groundMaterial;      // Drag your material here in the Inspector
    public float textureTiling = 10f;    // Controls how often texture repeats across terrain

    public GameObject GenerateChunk(int chunkX, int chunkZ)
    {
        GameObject chunk = new GameObject($"Chunk_{chunkX}_{chunkZ}");

        // Correct chunk position
        chunk.transform.position = new Vector3(chunkX * (chunkSize - 1), 0, chunkZ * (chunkSize - 1));

        MeshRenderer meshRenderer = chunk.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = chunk.AddComponent<MeshFilter>();
        MeshCollider meshCollider = chunk.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[chunkSize * chunkSize];
        int[] triangles = new int[(chunkSize - 1) * (chunkSize - 1) * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        int t = 0;
        for (int z = 0; z < chunkSize; z++)
    {
        for (int x = 0; x < chunkSize; x++)
        {
            int i = z * chunkSize + x;

            float worldX = (chunkX * (chunkSize - 1) + x) / noiseScale;
            float worldZ = (chunkZ * (chunkSize - 1) + z) / noiseScale;

            float y = Mathf.PerlinNoise(worldX, worldZ) * heightMultiplier;

            vertices[i] = new Vector3(x, y, z);

            uvs[i] = new Vector2(
                (x + chunkX * chunkSize) / textureTiling,
                (z + chunkZ * chunkSize) / textureTiling
            );

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
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        meshRenderer.material = groundMaterial;

        return chunk;
    }
}
