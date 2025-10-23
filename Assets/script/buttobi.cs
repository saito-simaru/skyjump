using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class buttobi : MonoBehaviour
{
    public Rigidbody rb;
    public bool isfinish = false;

    [Header("初期設定")]
    public Vector3 direction = Vector3.forward;  // 発射方向
    public float startSpeed = 100f;              // 開始速度
    public float minSpeed = 10f;                 // 重力へ切り替える速度閾値
    public float decelerationRate = 1f;          // 減速率（Lerp係数）

    [Header("動作フラグ")]
    public bool enableGravityAfterSlow = true;   // 閾値を下回ったら重力有効化
    public bool debugDraw = true;                // Gizmo線を描く

    private float currentSpeed;
    private Vector3 dirN;
    private bool falling = false; // 自由落下中かどうか

    // void Reset()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }
    //rigidbodyの設定と飛ぶ速さなどの値を設定
    void Start()
    {
        isfinish = false;
    }
    public void SetJumpconfig(float subspeed)
    {
        startSpeed = subspeed;

        if (!rb) rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
        dirN = direction.sqrMagnitude > 0f ? direction.normalized : transform.forward;

        currentSpeed = startSpeed;
    }

    void FixedUpdate()
    {
        //ジャンプしてないとき、自由落下タイムじゃないときは移動させない
        if (falling || !jumpmove.isjumping) return; // もう重力に任せる

        Debug.Log("呼ばれていないはず");

        // スムーズに減速（LerpでもMoveTowardsでもOK）
        currentSpeed = Mathf.Lerp(currentSpeed, 0f, decelerationRate * Time.fixedDeltaTime);

        // 移動
        rb.MovePosition(rb.position + dirN * currentSpeed * Time.fixedDeltaTime);

        // 一定速度以下になったら物理に切り替え
        if (enableGravityAfterSlow && currentSpeed <= minSpeed)
        {
            SwitchToPhysics();
        }
    }

    void SwitchToPhysics()
    {
        falling = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        isfinish = true;

        // 減速時点の速度を反映（方向は維持）
        rb.linearVelocity = dirN * currentSpeed;
    }

    void OnDrawGizmosSelected()
    {
        if (!debugDraw) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + direction.normalized * 3f);
    }
}
