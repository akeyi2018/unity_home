using UnityEngine;
using UnityEngine.Video;

public class ScreenManager : MonoBehaviour
{
    [Header("作成したCubeのPrefabをここに割り当てる")]
    public GameObject curvedScreenPrefab;

    [Header("再生したいビデオクリップ（動画ファイル）")]
    public VideoClip videoToPlay;

    [Header("生成時のスクリーンの初期設定")]
    [Range(0f, 10f)] public float initialCurvature = 1.2f;
    [Range(4, 40)] public int initialSegments = 30;

    [Header("スクリーンの物理的な大きさ（幅・高さ・厚み）")]
    public Vector3 targetScreenScale = new Vector3(2.0f, 1.125f, 1.0f); // 巨大化を防ぐための固定サイズ

    [Header("カメラ（追随対象）からの相対位置")]
    public Vector3 cameraOffset = new Vector3(0, -0.2f, 2.0f); // 正面2メートル、少し下め

    [Header("【重要】CenterEyeAnchor をここに割り当ててください")]
    public Transform targetCameraAnchor;

    [Header("Body追随の滑らかさ（値が小さいほど、ふわっと遅れてついてくる）")]
    [Range(1f, 20f)] public float followSpeed = 5.0f;

    [Header("スクリーン専用ライトの明るさ（光量）")]
    [Range(0f, 5f)] public float lightIntensity = 1.0f;

    private GameObject screenContainer;

    void Start()
    {
        // もしインスペクターで指定されていなければ、自動的にCenterEyeAnchorを探す
        if (targetCameraAnchor == null)
        {
            GameObject centerEye = GameObject.Find("CenterEyeAnchor");
            if (centerEye != null)
            {
                targetCameraAnchor = centerEye.transform;
            }
            else if (Camera.main != null)
            {
                targetCameraAnchor = Camera.main.transform;
            }
        }

        SpawnCurvedScreen();
    }

    public void SpawnCurvedScreen()
    {
        if (curvedScreenPrefab == null)
        {
            Debug.LogError("Prefabが割り当てられていません！");
            return;
        }

        // 1. 空のコンテナを作成（これがBody追随の親になります）
        screenContainer = new GameObject("CurvedScreen_Container");
        
        if (targetCameraAnchor != null)
        {
            Vector3 targetPos = targetCameraAnchor.position + targetCameraAnchor.TransformDirection(cameraOffset);
            screenContainer.transform.position = targetPos;
            
            // 💡 プレイヤーの向き（Y軸）だけ合わせ、余計な回転（180）は一切入れない
            float targetYRotation = targetCameraAnchor.eulerAngles.y;
            screenContainer.transform.rotation = Quaternion.Euler(0, targetYRotation, 0);
        }

        // 2. スクリーンを生成し、コンテナの子要素にする
        GameObject newScreen = Instantiate(curvedScreenPrefab, screenContainer.transform);
        newScreen.transform.localPosition = Vector3.zero;
        newScreen.transform.localRotation = Quaternion.identity; // 完全に無回転で生み出す

        // 💡 親の巨大スケールに影響されないよう、ここで理想のサイズに固定
        newScreen.transform.localScale = targetScreenScale;

        // 3. 数値を渡した直後に、手動でメッシュを「今すぐ」再生成させる
        CubeToCurve curveScript = newScreen.GetComponent<CubeToCurve>();
        if (curveScript != null)
        {
            curveScript.curvature = initialCurvature;
            curveScript.segments = initialSegments;

            // 💡 完全にまっすぐな状態で曲面メッシュを確定させる
            curveScript.ForceUpdateMesh(); 
        }

        // 4. 🌟【修正】親コンテナ側をひっくり返すのをやめ、完全にまっすぐな状態で固定
        float currentY = screenContainer.transform.eulerAngles.y;
        screenContainer.transform.rotation = Quaternion.Euler(0, currentY, 0);

        // 5. スクリーン専用の Directional Light を動的に生成してコンテナに入れる
        GameObject lightObj = new GameObject("Screen_DirectionalLight");
        lightObj.transform.SetParent(screenContainer.transform);
        lightObj.transform.localPosition = new Vector3(0, 0, -1.0f); 
        lightObj.transform.localRotation = Quaternion.identity; 

        Light screenLight = lightObj.AddComponent<Light>();
        screenLight.type = LightType.Directional;
        screenLight.intensity = lightIntensity;
        screenLight.color = Color.white;

        // 6. ビデオプレイヤーの紐付けと再生処理
        VideoPlayer videoPlayer = newScreen.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = FindFirstObjectByType<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            if (videoToPlay != null)
            {
                videoPlayer.source = VideoSource.VideoClip;
                videoPlayer.clip = videoToPlay;
            }

            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            
            RenderTexture rt = new RenderTexture(1920, 1080, -10);
            videoPlayer.targetTexture = rt;
            newScreen.GetComponent<MeshRenderer>().material.mainTexture = rt;

            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("VideoPlayerコンポーネントが見つかりません。動画を再生できません。");
        }
    }

    // 毎フレーム、プレイヤーの体（位置と水平の向き）に滑らかに追随させる（Body追随）
    void LateUpdate()
    {
        if (screenContainer == null || targetCameraAnchor == null) return;

        // 【位置の追随】カメラの正面方向に offset 分だけ離れた位置へ
        Vector3 targetPosition = targetCameraAnchor.position + targetCameraAnchor.TransformDirection(cameraOffset);
        screenContainer.transform.position = Vector3.Lerp(screenContainer.transform.position, targetPosition, Time.deltaTime * followSpeed);

        // 【向きの追随（Body追随）】
        // 🌟【修正】ここからも 180度回転の計算を排除し、ピュアにプレイヤーの正面（0）を向かせます
        float targetYRotation = targetCameraAnchor.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetYRotation, 0); 
        screenContainer.transform.rotation = Quaternion.Slerp(screenContainer.transform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }
}