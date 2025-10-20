using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class hosei : MonoBehaviour
{
    [Header("Ground Detect")]
    public LayerMask groundLayer;
    [Tooltip("曲面に強い接地を取るための球半径。BoxColliderの最小辺/2 くらいが目安")]
    public float sphereRadius = 0.2f;
    [Tooltip("キャラの少し上から下向きにキャストする高さ")]
    public float castHeight = 1.0f;
    [Tooltip("地面からどれだけ“浮かせる”か（コライダーの底面+α）")]
    public float hoverHeight = 0.05f;
    [Tooltip("接地追従の吸着速度（位置補正の追随）")]
    public float snapSpeed = 20f;

    [Header("Move")]
    public float moveSpeed = 4.0f;
    [Tooltip("回転追従のスムージング（大きいほど素早く地面法線へ向く）")]
    public float alignRotationSpeed = 12f;

    // 入力（外部から SetMoveInput で与えてもOK）
    private Vector2 moveInput;

    Rigidbody rb;
    BoxCollider box;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();

        // BoxColliderから良さげなsphere半径を自動推定（任意）
        if (sphereRadius <= 0f)
        {
            var size = box.size;
            sphereRadius = 0.5f * Mathf.Min(size.x, Mathf.Min(size.y, size.z)) * Mathf.Abs(transform.lossyScale.x);
        }
    }

    public void SetMoveInput(Vector2 input) => moveInput = Vector2.ClampMagnitude(input, 1f);

    void Update()
    {
        // 簡易: キーボード入力（不要なら削除し、SetMoveInputを外部から呼ぶ）
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(h) + Mathf.Abs(v) > 0f)
            moveInput = new Vector2(h, v);
        else
            moveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        // 1) 上から下へSphereCast（曲率の大きい面でも安定しやすい）
        Vector3 castOrigin = transform.position + Vector3.up * castHeight;
        float castDist = castHeight + 2.0f; // 余裕を持たせる

        if (Physics.SphereCast(castOrigin, sphereRadius, Vector3.down, out RaycastHit hit, castDist, groundLayer, QueryTriggerInteraction.Ignore))
        {
            // 2) 進行方向を“接平面”に投影して算出
            //   入力をワールド方向に変換（ここでは前方=transform.forward／右=transform.right基準）
            Vector3 wishDirWorld = (transform.forward * moveInput.y + transform.right * moveInput.x);
            wishDirWorld = Vector3.ProjectOnPlane(wishDirWorld, hit.normal).normalized;

            // 実際の移動分
            Vector3 moveDelta = wishDirWorld * moveSpeed * Time.fixedDeltaTime;

            // 3) 接地吸着（ヒット点＋法線×hoverHeightへスムーズに追従）
            Vector3 targetOnGround = hit.point + hit.normal * hoverHeight;
            Vector3 currentPos = rb ? rb.position : transform.position;

            // 先に接平面方向の移動を足す → そのうえで吸着で縦方向を補正
            Vector3 tentative = currentPos + moveDelta;

            // もう一度、移動後の足元で地面を取り直してスナップ品質UP
            Vector3 recastOrigin = tentative + Vector3.up * castHeight;
            if (Physics.SphereCast(recastOrigin, sphereRadius, Vector3.down, out RaycastHit hit2, castDist, groundLayer, QueryTriggerInteraction.Ignore))
            {
                targetOnGround = hit2.point + hit2.normal * hoverHeight;
                hit = hit2; // 回転合わせも最新の法線で
            }

            Vector3 snapped = Vector3.Lerp(tentative, targetOnGround, 1f - Mathf.Exp(-snapSpeed * Time.fixedDeltaTime));

            // 4) 回転：接平面の接線方向（前）と法線（上）で向きを作る
            Vector3 forwardTangent = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
            if (forwardTangent.sqrMagnitude < 1e-6f)
                forwardTangent = Vector3.ProjectOnPlane(Vector3.forward, hit.normal).normalized; // 退避

            Quaternion targetRot = Quaternion.LookRotation(forwardTangent, hit.normal);
            Quaternion newRot = Quaternion.Slerp(rb ? rb.rotation : transform.rotation, targetRot, 1f - Mathf.Exp(-alignRotationSpeed * Time.fixedDeltaTime));

            // 5) 反映
            if (rb)
            {
                rb.MovePosition(snapped);
                rb.MoveRotation(newRot);
            }
            else
            {
                transform.SetPositionAndRotation(snapped, newRot);
            }
        }
        else
        {
            // 空中に出た場合のフォールバック（必要なら重力で落とす等）
            if (rb) rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // キャストの可視化
        Vector3 castOrigin = transform.position + Vector3.up * castHeight;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(castOrigin, sphereRadius);
        Gizmos.DrawLine(castOrigin, castOrigin + Vector3.down * (castHeight + 1.5f));
    }
#endif
}
