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

#### 3. Unity 6 でのマテリアル変換手順

1. コンバーターを開く
上部メニューから Window > Rendering > Render Pipeline Converter を選択します。

2. 変換タイプを選択
左上のドロップダウン（初期状態では「Built-in to URP」など）から、「Built-in to URP」 を選択します。

3. 変換項目にチェックを入れる
マテリアルを変換したい場合は、リスト内の Material Upgrade にチェックを入れます。

4. スキャンを実行
右下の Initialize Converters ボタンを押します。これにより、プロジェクト内の変換可能なマテリアルがリストアップされます。

5. 変換を実行
リストに変換対象が表示されたら、右下の Convert Assets ボタンを押すと変換が始まります
