using UnityEngine;

public class speedovertime : MonoBehaviour
{
    [SerializeField] private speedmanager speedManager; // 共有SO（前に作ったやつ）
    [SerializeField] private float risePerSecond = 2f;  // 1秒間に増やす量（例: 2なら毎秒+2）
    // [SerializeField] private float maxSpeed = 10f;      // 上限（不要なら大きくしておく）

    private void Start()
    {
        speedManager.SetAbsolute(0.5f);
    }
    private void Update()
    {
        // Δt秒の間に増える量 = risePerSecond * Δt
        float delta = risePerSecond * Time.deltaTime;

        // 現在のスピードに加算し、上限を超えないようにClamp
        // float newSpeed = Mathf.Min(speedManager.speed + delta, maxSpeed);
        

        // 実際の適用（同じ値なら内部で弾かれるようにしてあると安全）
        speedManager.AddDelta(delta);
    }
}
