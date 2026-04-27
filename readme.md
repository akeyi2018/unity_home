### UNITY 基礎学習
#### 1. GameObjectの動かし方
``` csharp
// 前方に動かす
transform.Translate(Vector3.forward * speed);
```
#### 2. 玉の生成
``` csharp
// 玉のプレハブを設定するための変数
public GameObject ballPrefab;

// スペースキーで玉を発射するためのコード
if (Input.GetKeyDown(KeyCode.Space))
{    
    // プレイヤーの位置に玉を生成
    Instantiate(ballPrefab, transform.position + Vector3.up, Quaternion.identity);
}
```