using UnityEngine;
using UnityEngine.InputSystem;

public class front_move : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 10f;
    public float maxDistance = 700f;

    [Header("パイプ設定")]
    public GameObject[] pipes;
    private float pipeLength = 65f;
    private int currentLeadPipeIndex = 0;
    private bool isMoving = false;

    [Header("入力設定")]
    [SerializeField] private InputActionReference _startStopAction;

    // PIPEの移動量を自前で管理
    private float _totalMoved = 0f;

    void Start()
    {
        // PIPEの初期位置設定（カメラは動かさない）
        if (pipes != null && pipes.Length == 3)
        {
            for (int i = 0; i < pipes.Length; i++)
            {
                if (pipes[i] != null)
                {
                    pipes[i].transform.position = new Vector3(0, 0, i * pipeLength);
                }
            }
        }
        else
        {
            Debug.LogError("Pipes配列に3つのオブジェクトをセットしてください。");
        }

        // InputActionの登録
        if (_startStopAction != null)
        {
            _startStopAction.action.performed += OnStartStop;
            _startStopAction.action.Enable();
        }
    }

    void OnDestroy()
    {
        // イベント解除（メモリリーク防止）
        if (_startStopAction != null)
        {
            _startStopAction.action.performed -= OnStartStop;
        }
    }

    private void OnStartStop(InputAction.CallbackContext ctx)
    {
        isMoving = !isMoving;
    }

    void Update()
    {
        if (!isMoving) return;
        if (_totalMoved >= maxDistance) return;

        // カメラではなくPIPE群をZ方向に移動（マイナス方向＝手前に流れる）
        float delta = moveSpeed * Time.deltaTime;
        delta = Mathf.Min(delta, maxDistance - _totalMoved); // 上限制限

        foreach (var pipe in pipes)
        {
            if (pipe != null)
                pipe.transform.position += new Vector3(0, 0, -delta);
        }

        _totalMoved += delta;

        // ゴール到達で停止
        if (_totalMoved >= maxDistance)
        {
            isMoving = false;
        }

        // 通り抜けたPIPEを前方に再配置
        GameObject tailPipe = pipes[currentLeadPipeIndex];
        if (tailPipe.transform.position.z < -pipeLength)
        {
            tailPipe.transform.position += new Vector3(0, 0, pipeLength * 3);
            currentLeadPipeIndex = (currentLeadPipeIndex + 1) % pipes.Length;
        }
    }
}