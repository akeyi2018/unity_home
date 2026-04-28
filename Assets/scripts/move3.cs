using UnityEngine;

public class KeyMoveGroupBallsEmission : MonoBehaviour
{
    [Header("CircleDeployerがついているオブジェクト")]
    public GameObject deployerObject;

    [Header("点滅させるマテリアル")]
    public Material ballMaterial; 

    [Header("移動・回転設定")]
    public float moveSpeed = 0.0f;
    public float acceleration = 15.0f;
    public float maxSpeed = 20.0f;
    public float currentRotationSpeed = 0.0f;
    public float rotationAcceleration = 100.0f;
    public float maxRotationSpeed = 300.0f;
    [Range(0, 10)] public float rotationDeceleration = 2.0f;

    [Header("ループ設定")]
    public float minY = -20.0f;
    public float spawnY = 50.0f;

    [Header("Emission点滅設定")]
    public float flashesPerSecond = 4.0f;
    [ColorUsage(true, true)] public Color emissionColor = Color.white; // インスペクターで光の色と強さを設定

    private Color blackColor = Color.black;

    void OnDisable()
    {
        // 終了時にEmissionを元に戻さないと、エディタ上でマテリアルが真っ暗なままになるのを防ぐ
        if (ballMaterial != null)
        {
            ballMaterial.SetColor("_EmissionColor", emissionColor);
            ballMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        if (deployerObject == null || ballMaterial == null) return;

        // --- 入力と移動 ---
        float inputY = Input.GetAxis("Vertical");
        float inputX = Input.GetAxis("Horizontal");
        moveSpeed = Mathf.Clamp(moveSpeed + inputY * acceleration * Time.deltaTime, 0, maxSpeed);
        currentRotationSpeed = Mathf.Clamp(currentRotationSpeed + inputX * rotationAcceleration * Time.deltaTime, -maxRotationSpeed, maxRotationSpeed);

        // --- Emission 点滅ロジック ---
        float interval = 1.0f / flashesPerSecond;
        bool isOn = (Time.time % interval) < (interval / 2.0f);
        
        // isOnのときは設定した色、offのときは黒
        Color finalColor = isOn ? emissionColor : blackColor;
        ballMaterial.SetColor("_EmissionColor", finalColor);
        
        // キーワードを有効にしないと反映されない場合がある
        ballMaterial.EnableKeyword("_EMISSION");

        // --- 子要素の移動・回転 ---
        Quaternion rotationStep = Quaternion.Euler(0, -currentRotationSpeed * Time.deltaTime, 0);
        foreach (Transform child in deployerObject.transform)
        {
            child.localPosition = rotationStep * child.localPosition;
            child.localRotation = rotationStep * child.localRotation;

            Vector3 lp = child.localPosition;
            lp.y -= moveSpeed * Time.deltaTime;
            if (lp.y < minY) lp.y = spawnY;
            child.localPosition = lp;
        }
    }
}