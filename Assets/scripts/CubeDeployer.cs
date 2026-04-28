using UnityEngine;

public class CubeDeployer : MonoBehaviour
{
    [Header("配置するオブジェクト (Cubeなど)")]
    public GameObject baseObject;

    [Header("使用するマテリアルのリスト")]
    public Material[] materials; // ここにm1, m2, m3, m4をアサイン

    [Header("環状の設定")]
    public float radius = 100.0f;
    public float startAngleDeg = 22.5f;   // 開始角度（度）
    public float endAngleDeg = 337.5f;     // 終了角度（度）
    public float stepAngleDeg = 45.0f;    // 間隔（度）

    [Header("複数配置の設定")]
    public int ringCount = 3;
    public float ringInterval = 5.0f;
    public float startY = -10.0f;

    [ContextMenu("Execute Spawning")]
    public void SpawnCubes()
    {
        if (baseObject == null) return;

        // 通し番号（マテリアル選択用）
        int totalCount = 0;

        for (int r = 0; r < ringCount; r++)
        {
            float currentY = startY + (r * ringInterval);

            for (float angleDeg = startAngleDeg; angleDeg <= endAngleDeg + 0.1f; angleDeg += stepAngleDeg)
            {
                float angleRad = angleDeg * Mathf.Deg2Rad;
                
                float x = Mathf.Cos(angleRad) * radius;
                float z = Mathf.Sin(angleRad) * radius;

                Vector3 pos = new Vector3(x, currentY, z);
                Vector3 worldPos = transform.TransformPoint(pos);

                GameObject spawned = Instantiate(baseObject, worldPos, transform.rotation);
                
                // --- マテリアルの適用処理 ---
                if (materials != null && materials.Length > 0)
                {
                    Renderer rend = spawned.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        // 生成された順番に合わせてマテリアルを割り当て
                        rend.material = materials[totalCount % materials.Length];
                    }
                }
                // --------------------------

                spawned.transform.localScale = baseObject.transform.localScale;
                spawned.name = $"Cube_Ring{r}_Deg{angleDeg}";
                spawned.transform.SetParent(this.transform);
                spawned.SetActive(true);

                // カウンターを増やす
                totalCount++;
            }
        }
    }
}