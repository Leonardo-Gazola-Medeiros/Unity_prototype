using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    public int chunkSize = 50;
    public float scale = 0.1f;
    public float height = 10f;

    public Vector2 chunkCoord; // Assigned when creating chunk (x, z index)

    public void GenerateChunk()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        int[] triangles = new int[chunkSize * chunkSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++)
            {
                // Use WORLD coordinates to keep continuity
                float worldX = x + chunkCoord.x * chunkSize;
                float worldZ = z + chunkCoord.y * chunkSize;

                float y = Mathf.PerlinNoise(worldX * scale, worldZ * scale) * height;

                vertices[vert] = new Vector3(x, y, z);

                if (x < chunkSize && z < chunkSize)
                {
                    triangles[tris + 0] = vert;
                    triangles[tris + 1] = vert + chunkSize + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + chunkSize + 1;
                    triangles[tris + 5] = vert + chunkSize + 2;
                    tris += 6;
                }

                vert++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
