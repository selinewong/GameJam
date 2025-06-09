using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainToSphere : MonoBehaviour {
    public Terrain terrain;
    public float sphereRadius = 1f;       // Match your sphere's scale
    public float heightMultiplier = 1f;   // Adjust how “bumpy” it gets

    void Start() {
        if (terrain == null) {
            terrain = Terrain.activeTerrain;
            if (terrain == null) {
                Debug.LogError("🛑 No Terrain assigned or found in scene!");
                return;
            } else {
                Debug.Log($"Auto-assigned Terrain: {terrain.name}");
            }
        }

        var tData = terrain.terrainData;
        if (tData == null) {
            Debug.LogError("🛑 TerrainData is missing!");
            return;
        }

        int res = tData.heightmapResolution;
        float[,] heights = tData.GetHeights(0, 0, res, res);

        var mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++) {
            Vector3 dir = verts[i].normalized;

            // Spherical UV mapping
            float u = 0.5f + Mathf.Atan2(dir.z, dir.x) / (2f * Mathf.PI);
            float v = 0.5f - Mathf.Asin(dir.y) / Mathf.PI;

            int x = Mathf.Clamp((int)(u * res), 0, res - 1);
            int y = Mathf.Clamp((int)(v * res), 0, res - 1);

            float h = heights[y, x];
            verts[i] = dir * (sphereRadius + h * heightMultiplier);
        }

        mesh.vertices = verts;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Debug.Log("✅ Sphere terrain mapping complete!");
    }
}
