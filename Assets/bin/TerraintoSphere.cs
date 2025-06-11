using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainToSphereTriplanar : MonoBehaviour {
    public Terrain terrain;
    public float sphereRadius = 1f;
    public float heightMultiplier = 1f;

    void Start() {
        if (terrain == null) terrain = Terrain.activeTerrain;
        if (terrain == null) {
            Debug.LogError("🛑 No Terrain assigned or found!");
            return;
        }

        TerrainData td = terrain.terrainData;
        if (td == null) {
            Debug.LogError("🛑 TerrainData is missing!");
            return;
        }

        int res = td.heightmapResolution;
        float[,] heights = td.GetHeights(0, 0, res, res);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++) {
            Vector3 dir = verts[i].normalized;

            // spherical UV
            float u = 0.5f + Mathf.Atan2(dir.z, dir.x) / (2f * Mathf.PI);
            float v = 0.5f - Mathf.Asin(dir.y) / Mathf.PI;

            // sample three offsets
            float h1 = Sample(uv: new Vector2(u, v), heights, res);
            float h2 = Sample(uv: new Vector2(u + 0.25f, v), heights, res);
            float h3 = Sample(uv: new Vector2(u, v + 0.25f), heights, res);

            // triplanar blend weights
            Vector3 n = dir.normalized;
            Vector3 blend = new Vector3(Mathf.Abs(n.x), Mathf.Abs(n.y), Mathf.Abs(n.z));
            blend /= (blend.x + blend.y + blend.z + 1e-6f);

            float height = h1 * blend.x + h2 * blend.y + h3 * blend.z;
            verts[i] = dir * (sphereRadius + height * heightMultiplier);
        }

        mesh.vertices = verts;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        Debug.Log("✅ Sphere terrain mapping with triplanar blend complete!");
    }

    float Sample(Vector2 uv, float[,] heights, int resolution) {
        int x = Mathf.Clamp((int)(uv.x * resolution), 0, resolution - 1);
        int y = Mathf.Clamp((int)(uv.y * resolution), 0, resolution - 1);
        return heights[y, x];
    }
}
