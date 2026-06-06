using UnityEngine;

public class KeyMoveGroupBallsEmission : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private GameObject deployerObject;
    [SerializeField] private Material ballMaterial; 

    [Header("移動・回転設定")]
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private float acceleration = 15.0f;
    [SerializeField] private float maxSpeed = 20.0f;
    [SerializeField] private float currentRotationSpeed = 0.0f;
    [SerializeField] private float rotationAcceleration = 100.0f;
    [SerializeField] private float maxRotationSpeed = 300.0f;
    [Range(0, 10)] [SerializeField] private float rotationDeceleration = 2.0f;

    [Header("ループ設定")]
    [SerializeField] private float minY = -20.0f;
    [SerializeField] private float spawnY = 50.0f;

    [Header("Emission点滅設定")]
    [SerializeField] private float flashesPerSecond = 4.0f;
    [ColorUsage(true, true)] [SerializeField] private Color emissionColor = Color.white;

    [Header("キー設定")]
    [SerializeField] private KeyCode forwardKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode backwardKey = KeyCode.DownArrow;
    [SerializeField] private KeyCode rotateLeftKey = KeyCode.LeftArrow;
    [SerializeField] private KeyCode rotateRightKey = KeyCode.RightArrow;

    private Color blackColor = Color.black;

    void OnDisable()
    {
        if (ballMaterial != null)
        {
            ballMaterial.SetColor("_EmissionColor", emissionColor);
            ballMaterial.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        if (deployerObject == null || ballMaterial == null) return;

        float inputY = 0f;
        if (Input.GetKey(forwardKey))  inputY =  1f;
        if (Input.GetKey(backwardKey)) inputY = -1f;

        float inputX = 0f;
        if (Input.GetKey(rotateRightKey)) inputX =  1f;
        if (Input.GetKey(rotateLeftKey))  inputX = -1f;

        moveSpeed = Mathf.Clamp(moveSpeed + inputY * acceleration * Time.deltaTime, 0, maxSpeed);
        currentRotationSpeed = Mathf.Clamp(currentRotationSpeed + inputX * rotationAcceleration * Time.deltaTime, -maxRotationSpeed, maxRotationSpeed);

        float interval = 1.0f / flashesPerSecond;
        bool isOn = (Time.time % interval) < (interval / 2.0f);
        
        Color finalColor = isOn ? emissionColor : blackColor;
        ballMaterial.SetColor("_EmissionColor", finalColor);
        ballMaterial.EnableKeyword("_EMISSION");

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