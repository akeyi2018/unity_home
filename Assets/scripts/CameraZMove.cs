using UnityEngine;

public class CameraZMove : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 10.0f;

    [Header("キー設定")]
    public KeyCode forwardKey = KeyCode.Z;
    public KeyCode backwardKey = KeyCode.X;

    void Update()
    {
        Vector3 direction = transform.forward;

        if (Input.GetKey(forwardKey))
            transform.position += direction * moveSpeed * Time.deltaTime;

        if (Input.GetKey(backwardKey))
            transform.position -= direction * moveSpeed * Time.deltaTime;
    }
}