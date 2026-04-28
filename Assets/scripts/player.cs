using UnityEngine;

public class player : MonoBehaviour
{
    public float speed = 5f;
    
    // 玉のプレハブを設定するための変数
    public GameObject ballPrefab;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Time.deltaTime を掛けることで、フレームレートに関わらず一定の速度で移動させます
        float moveDistance = speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            // 現在の座標に、前方向へのベクトルを加算して代入する
            transform.position += Vector3.forward * moveDistance;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * moveDistance;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * moveDistance;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveDistance;
        }

        // スペースキーで玉を発射するためのコード
        if (Input.GetKeyDown(KeyCode.Space))
        {    
            // プレイヤーの位置に玉を生成
            Instantiate(ballPrefab, transform.position + Vector3.up, Quaternion.identity);
        }
        // ゲームを終了するためのコード
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
