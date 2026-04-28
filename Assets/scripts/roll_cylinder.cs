using UnityEngine;

public class KeyMoveGroupBalls : MonoBehaviour
{
    [Header("CircleDeployerがついているオブジェクト（PIPE出口）")]
    public GameObject deployerObject;

    [Header("移動速度設定(Y軸)")]
    public float moveSpeed = 0.0f; 
    public float acceleration = 15.0f;
    public float maxSpeed = 20.0f; 
    public float minLimitSpeed = 0.0f; 

    [Header("回転速度設定(X軸)")]
    public float currentRotationSpeed = 0.0f; 
    public float rotationAcceleration = 100.0f; 
    public float maxRotationSpeed = 300.0f;   
    [Range(0, 10)] public float rotationDeceleration = 2.0f; 

    [Header("ループ設定（ローカル座標）")]
    public float minY = -20.0f; 
    public float spawnY = 50.0f; 

    void Update()
    {
        if (deployerObject == null) return;

        // --- 入力の取得 (キーボード用) ---
        // Vertical: W/Sキー または 上下矢印キー
        // Horizontal: A/Dキー または 左右矢印キー
        float inputY = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        
        // 1. 移動速度更新
        if (Mathf.Abs(inputY) > 0.01f)
            moveSpeed += inputY * acceleration * Time.deltaTime;
        else
            moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 1.5f);
        
        moveSpeed = Mathf.Clamp(moveSpeed, minLimitSpeed, maxSpeed);

        // 2. 回転速度更新
        if (Mathf.Abs(inputX) > 0.01f)
            currentRotationSpeed += inputX * rotationAcceleration * Time.deltaTime;
        else
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, rotationDeceleration * Time.deltaTime);
        
        currentRotationSpeed = Mathf.Clamp(currentRotationSpeed, -maxRotationSpeed, maxRotationSpeed);

        // 3. 全ての子要素を「親のローカル空間」で計算
        if (Mathf.Abs(currentRotationSpeed) > 0.01f || Mathf.Abs(moveSpeed) > 0.001f)
        {
            Quaternion rotationStep = Quaternion.Euler(0, -currentRotationSpeed * Time.deltaTime, 0);

            foreach (Transform child in deployerObject.transform)
            {
                // A. 回転
                child.localPosition = rotationStep * child.localPosition;

                // B. 自転
                child.localRotation = rotationStep * child.localRotation;

                // C. 移動
                Vector3 lp = child.localPosition;
                lp.y -= moveSpeed * Time.deltaTime;

                // D. ループ処理
                if (lp.y < minY)
                {
                    float offset = minY - lp.y;
                    lp.y = spawnY - offset; 
                }
                child.localPosition = lp;
            }
        }
    }
}