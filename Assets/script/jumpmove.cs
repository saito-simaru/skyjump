using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework.Internal;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms;
public class jumpmove : MonoBehaviour
{
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;
    public float moveSpeed = 2f; // 移動スピード
    [SerializeField] private buttobi buttobi;
    [Header("Tag設定")]
    public string redtag = "redline"; // 特定のTagを指定
    public string pinktag = "pinkline"; // 特定のTagを指定

    [Header("Ray設定")]
    public float rayLength = 5f; // 下方向へのRayの長さ
    public LayerMask groundLayer; // 検知対象のレイヤー（省略可）
    private bool inredline = false;
    private bool inpinkline = false;
    public static bool isjumping = false;
    private BoxCollider col;
    private Vector3 localPos;
    private Vector3 startRayposition;

    void Start()
    {
        // col = GetComponent<BoxCollider>();

        // localPos = transform.localPosition;

        // // 親のスケールを考慮した「実際のサイズの半分」
        // float halfSizeX = (transform.localScale.x * transform.lossyScale.x / transform.parent.lossyScale.x) / 2f;

        // // localPosition.x からその分を引く
        // localPos.x -= halfSizeX;
        // startRayposition = localPos;
    }


    public void Setjumppoint(Vector3 newTarget,float Speed)
    {
                // ターゲットが更新されたら、今の移動を中断
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveSpeed = Speed;
        targetPosition = newTarget;
        moveCoroutine = StartCoroutine(MoveToJumppoint(moveSpeed));
    }
    private IEnumerator MoveToJumppoint(float Speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                Speed * Time.deltaTime
            );

            yield return null; // 次のフレームまで待機
        }

        // 最終位置を正確に補正
        transform.position = targetPosition;
        moveCoroutine = null;
    }

    void OnTriggerEnter(Collider other)
    {
        // 特定のTagを持つオブジェクトと衝突したとき
        if (other.CompareTag(redtag))
        {
            Debug.Log("特定のTag（" + redtag + "）を持つオブジェクトと接触しました！");
            inredline = true;
            // --- ここに処理を書く ---
            // 例：スコア加算、エフェクト再生など
        }
        else if (other.CompareTag(pinktag))
        {
            Debug.Log("特定のTag（" + pinktag + "）を持つオブジェクトと接触しました！");
            inpinkline = true;
            // --- ここに処理を書く ---
            // 例：スコア加算、エフェクト再生など

        }
    }

    public void Onjump(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {

            if (inredline)
            {
                Debug.Log("私はレッド");
                buttobi.SetJumpconfig(300);
                isjumping = true;
            }
            else if (inpinkline)
            {
                Debug.Log("私はピンキー");
                buttobi.SetJumpconfig(150);
                isjumping = true;
            }
            else
            {
                inredline = false;
                inpinkline = false;
            }

        }


    }
    
    void Update()
    {
        if (!fazecontroller.isjumpfaze) return;

        Vector3 localOffset = new Vector3(0f, 0f, 0f);
        Vector3 rayStart = transform.TransformPoint(localOffset);
        Ray ray = new Ray(rayStart, Vector3.down);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, rayLength, groundLayer))
        {
            col.isTrigger = false;
            Debug.Log("ColliderのTriggerをオフにしました");
            Debug.DrawRay(rayStart, Vector3.down * rayLength, Color.green);
            Debug.Log("out");
            isjumping = true;

            
        }
        else
        {

            // 下に何もない場合の処理
            Debug.Log("下にオブジェクトが見つかりません！");
            // --- ここに処理を書く ---
            // 例：落下判定、警告エフェクトなど
        }
    }
}
