using UnityEngine;

public class CircleDeployer : MonoBehaviour
{
    [Header("Hierarchyにある、見えているSphereをここにドラッグ")]
    public GameObject baseSphere;

    [Header("環状の設定（1つの輪っか）")]
    public int countPerRing = 8;     // 1つの輪あたりの個数
    public float radius = 100.0f;

    [Header("複数配置の設定")]
    public int ringCount = 3;       // 何セット（何枚の輪）作るか
    public float ringInterval = 5.0f; // 輪と輪の間の距離
    public float startY = -10.0f;    // 最初の輪の高さ

    [ContextMenu("Execute Spawning")]
    public void SpawnCircle()
    {
        if (baseSphere == null) return;

        // 1. 輪の数だけループ（縦方向）
        for (int r = 0; r < ringCount; r++)
        {
            // 現在の輪の高さを計算
            float currentY = startY + (r * ringInterval);

            // 2. 1つの輪の中の個数分ループ（横方向）
            for (int i = 0; i < countPerRing; i++)
            {
                float angle = i * Mathf.PI * 2 / countPerRing;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                // 計算した高さを適用
                Vector3 pos = new Vector3(x, currentY, z);
                Vector3 worldPos = transform.TransformPoint(pos);

                GameObject spawned = Instantiate(baseSphere, worldPos, transform.rotation);
                
                spawned.transform.localScale = baseSphere.transform.localScale;
                spawned.name = $"Ball_Ring{r}_{i}";
                spawned.transform.SetParent(this.transform);
                spawned.SetActive(true);
            }
        }
    }
}