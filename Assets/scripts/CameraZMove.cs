using UnityEngine;

public class CameraZMove : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 10.0f;

    void Update()
    {
        // カメラの向いている方向（Z軸）を取得
        Vector3 direction = transform.forward;

        // Zキーで前進
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        
        // Xキーで後退
        if (Input.GetKey(KeyCode.X))
        {
            transform.position -= direction * moveSpeed * Time.deltaTime;
        }
    }
}