using UnityEngine;

public class KeyMoveGroupBallsFinal : MonoBehaviour
{
    [Header("CircleDeployerがついているオブジェクト")]
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

    [Header("ライトの点滅設定（1秒間に4回）")]
    public float minIntensity = 0.0f;   // 消灯時の明るさ
    public float maxIntensity = 10.0f;  // 点灯時の明るさ
    public float flashesPerSecond = 4.0f; 

    void Update()
    {
        if (deployerObject == null) return;

        // --- 1. キーボード入力の取得 ---
        float inputY = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");

        // 移動速度更新
        if (Mathf.Abs(inputY) > 0.01f)
            moveSpeed += inputY * acceleration * Time.deltaTime;
        else
            moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime * 1.5f);
        
        moveSpeed = Mathf.Clamp(moveSpeed, minLimitSpeed, maxSpeed);

        // 回転速度更新
        if (Mathf.Abs(inputX) > 0.01f)
            currentRotationSpeed += inputX * rotationAcceleration * Time.deltaTime;
        else
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, rotationDeceleration * Time.deltaTime);
        
        currentRotationSpeed = Mathf.Clamp(currentRotationSpeed, -maxRotationSpeed, maxRotationSpeed);

        // --- 2. ライトの共通輝度計算（1秒間に4回点滅） ---
        // 方法A: パキパキとON/OFFさせる（推奨）
        float interval = 1.0f / flashesPerSecond; // 0.25秒
        bool isOn = (Time.time % interval) < (interval / 2.0f);
        float sharedIntensity = isOn ? maxIntensity : minIntensity;

        /* // 方法B: SIN波で滑らかに明滅させたい場合はこちらを有効にしてください
        float sinValue = Mathf.Sin(Time.time * (flashesPerSecond * 2.0f * Mathf.PI));
        float sharedIntensity = Mathf.Lerp(minIntensity, maxIntensity, (sinValue + 1.0f) / 2.0f);
        */

        // --- 3. 全ての子要素を計算 ---
        Quaternion rotationStep = Quaternion.Euler(0, -currentRotationSpeed * Time.deltaTime, 0);

        foreach (Transform child in deployerObject.transform)
        {
            // A. 回転
            child.localPosition = rotationStep * child.localPosition;
            child.localRotation = rotationStep * child.localRotation;

            // B. 移動
            Vector3 lp = child.localPosition;
            lp.y -= moveSpeed * Time.deltaTime;

            // C. ループ処理
            if (lp.y < minY)
            {
                float offset = minY - lp.y;
                lp.y = spawnY - offset; 
            }
            child.localPosition = lp;

            // D. ライトの強度を適用
            Light light = child.GetComponent<Light>();
            if (light != null)
            {
                light.intensity = sharedIntensity;
            }
        }
    }
}