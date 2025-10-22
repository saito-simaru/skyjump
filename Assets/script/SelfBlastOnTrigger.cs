using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SelfBlastOnTrigger : MonoBehaviour
{
    [Header("Blast Settings")]
    [Tooltip("爆発の強さ（Impulse推奨）")]
    public float force = 20f;

    [Tooltip("爆発の有効半径（自分にも必須）")]
    public float radius = 3f;

    [Tooltip("上方向オフセット。大きいほど真上に持ち上がりやすい")]
    public float upwardsModifier = 2f;

    [Tooltip("爆心地を自分の真下にどれだけ下げるか")]
    public float originDownOffset = 1.0f;

    [Header("Randomness")]
    [Tooltip("爆心地に加えるランダム位置揺らぎ（水平メイン）")]
    public float positionJitter = 0.25f;

    [Tooltip("上向き補正に加えるランダム幅")]
    public float upwardsJitter = 0.5f;

    [Header("Trigger Conditions")]
    [Tooltip("このタグの相手が入ったときのみ反応（空なら誰でも反応）")]
    public string activatorTag = "";

    [Tooltip("一度だけ吹っ飛ぶか")]
    public bool oneShot = true;

    [Tooltip("連続反応を防ぐクールダウン(秒)")]
    public float cooldown = 0.2f;

    [Header("Force Mode")]
    public ForceMode forceMode = ForceMode.Impulse;

    Rigidbody rb;
    Collider col;
    float lastTriggeredTime = -999f;
    bool consumed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Reset()
    {
        // 新規追加時の安全設定
        var c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
        var r = GetComponent<Rigidbody>();
        if (r)
        {
            r.isKinematic = false;
            r.useGravity = true;
            r.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (consumed) return;
        if (Time.time - lastTriggeredTime < cooldown) return;
        if (!string.IsNullOrEmpty(activatorTag) && !other.CompareTag(activatorTag)) return;

        // 爆心地：自分の少し下＋わずかにランダム
        Vector3 jitter =
            new Vector3(Random.Range(-positionJitter, positionJitter), 0f,
                        Random.Range(-positionJitter, positionJitter));
        Vector3 explosionPos = transform.position + Vector3.down * originDownOffset + jitter;

        // 上向き補正もランダムに揺らす
        float upMod = upwardsModifier + Random.Range(-upwardsJitter, upwardsJitter);

        // 自分自身のRigidbodyへ爆発力
        rb.AddExplosionForce(force, explosionPos, radius, upMod, forceMode);

        lastTriggeredTime = Time.time;
        if (oneShot) consumed = true;
    }
}
