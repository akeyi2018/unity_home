using UnityEngine;

public class tama : MonoBehaviour
{
    // 玉の速度を設定するための変数
    public float speed = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 玉を前方に移動させるためのコード
        transform.Translate(Vector3.forward * speed);
    }
}
