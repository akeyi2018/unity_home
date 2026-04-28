using UnityEngine;

public class PipeScaleController : MonoBehaviour
{
    [Header("スケール変更速度")]
    public float scaleSpeed = 2.0f;

    [Header("スケール制限")]
    public float minScaleY = 0.5f;
    public float maxScaleY = 5.0f;

    void Update()
    {
        // 現在のローカルスケールを取得
        Vector3 currentScale = transform.localScale;

        // Eキーで拡大、Qキーで縮小
        if (Input.GetKey(KeyCode.E))
        {
            currentScale.y += scaleSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            currentScale.y -= scaleSpeed * Time.deltaTime;
        }

        // Y軸の値を制限の範囲内に収める
        currentScale.y = Mathf.Clamp(currentScale.y, minScaleY, maxScaleY);

        // スケールを反映
        transform.localScale = currentScale;
    }
}