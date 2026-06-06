using UnityEngine;

[ExecuteInEditMode]
public class CubeToCurve : MonoBehaviour
{
    [Range(0f, 5f)] public float curvature = 0.5f; 
    [Range(4, 40)] public int segments = 20; 

    private MeshFilter meshFilter;
    private Mesh generatedMesh;

    private bool isCurved = true; // 現在曲面かどうか

    void OnValidate() { UpdateMesh(); }
    void Start() { UpdateMesh(); }

    void Update()
    {
        // --- エディタテスト用（Spaceキー） ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleScreenShape();
        }

        // --- 実機用（OVRInputでの開通を確認済みの一発検知） ---
        // 右手の人差し指トリガーがカチッと押された瞬間を検知
        // if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        // {
        //     ToggleScreenShape();
        // }
    }

    public void ToggleScreenShape()
    {
        isCurved = !isCurved; // 状態を反転
        UpdateMesh();         // メッシュを再生成
    }

    // 🌟【追加】ScreenManagerから呼び出される「強制メッシュ更新」の関数
    // これが足りなかったため、コンパイルエラー（CS1061）が発生していました
    public void ForceUpdateMesh()
    {
        isCurved = true; // 生成時は確実に曲面（true）からスタートさせる
        UpdateMesh();    // 今すぐ現在のサイズと曲率でメッシュを編み直す
    }

    void UpdateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        if (generatedMesh == null)
        {
            generatedMesh = new Mesh();
            generatedMesh.name = "CurvedScreenMesh";
        }

        int vCount = (segments + 1) * 2;
        Vector3[] vertices = new Vector3[vCount];
        Vector2[] uvs = new Vector2[vCount];
        int[] triangles = new int[segments * 6];

        float width = transform.localScale.x;
        float height = transform.localScale.y;

        int vIdx = 0;
        int tIdx = 0;

        // 平面時は曲率を0fにする
        float currentCurvature = isCurved ? curvature : 0f;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float x = (t - 0.5f) * width;
            float z = Mathf.Sin(t * Mathf.PI) * currentCurvature * (width * 0.2f);

            // 自作の最も綺麗なマッピングロジックを維持
            vertices[vIdx] = new Vector3(x, height * 0.5f, z);
            uvs[vIdx] = new Vector2(t, 1f);
            vIdx++;

            vertices[vIdx] = new Vector3(x, -height * 0.5f, z);
            uvs[vIdx] = new Vector2(t, 0f);
            vIdx++;

            if (i < segments)
            {
                int current = i * 2;
                triangles[tIdx++] = current;
                triangles[tIdx++] = current + 2;
                triangles[tIdx++] = current + 1;

                triangles[tIdx++] = current + 1;
                triangles[tIdx++] = current + 2;
                triangles[tIdx++] = current + 3;
            }
        }

        generatedMesh.Clear();
        generatedMesh.vertices = vertices;
        generatedMesh.uv = uvs;
        generatedMesh.triangles = triangles;
        generatedMesh.RecalculateNormals();
        generatedMesh.RecalculateBounds();

        meshFilter.mesh = generatedMesh;
    }
}