using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.ComponentModel;
using NUnit.Framework.Internal;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms;
using Unity.Burst.CompilerServices;
using TMPro;
public class jumpmove : MonoBehaviour
{
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;
    private float moveSpeed; // 移動スピード
    
    [SerializeField] private buttobi buttobi;
    [SerializeField] private fazecontroller FC;
    [Header("Tag設定")]
    public string redtag = "redline"; // 特定のTagを指定
    public string pinktag = "pinkline"; // 特定のTagを指定

    [Header("Ray設定")]
    public float rayLength = 5f; // 下方向へのRayの長さ
    public LayerMask groundLayer; // 検知対象のレイヤー（省略可）

    public bool isjumping = false;
    private bool ispressjumpbutton = false;
    private BoxCollider col;
    private Vector3 localPos;
    private Vector3 startRayposition;
    [Header("ジャンプ可能範囲")]
    public GameObject hasiobj;
    public int variabljumpfield;
    private float possiblejumppos;
    [Header("ジャンプ時の基本加速度")]
    public int acceleeration;
    public speedmanager speedmanager;
    [Header("判定UI")]
    public TextMeshProUGUI FadeText; // フェード対象のCanvasGroup
    public float fadeSpeed = 2f; // フェード速度（大きいほど速い）
    public string perfectword;
    public string goodword;

    void Start()
    {
        col = GetComponent<BoxCollider>();

        possiblejumppos = hasiobj.transform.position.x - variabljumpfield;

        FadeText.alpha = 0f;

        // localPos = transform.localPosition;

        // // 親のスケールを考慮した「実際のサイズの半分」
        // float halfSizeX = (transform.localScale.x * transform.lossyScale.x / transform.parent.lossyScale.x) / 2f;

        // // localPosition.x からその分を引く
        // localPos.x -= halfSizeX;
        // startRayposition = localPos;
    }


    // public void Setjumppoint(Vector3 newTarget, float Speed)
    // {
    //     // ターゲットが更新されたら、今の移動を中断
    //     if (moveCoroutine != null)
    //     {
    //         StopCoroutine(moveCoroutine);
    //     }
    //     moveSpeed = Speed;
    //     targetPosition = newTarget;
    //     moveCoroutine = StartCoroutine(MoveToJumppoint(moveSpeed));
    // }
    
    // private IEnumerator MoveToJumppoint(float Speed)
    // {
    //     while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
    //     {
    //         transform.position = Vector3.MoveTowards(
    //             transform.position,
    //             targetPosition,
    //             Speed * Time.deltaTime
    //         );

    //         yield return null; // 次のフレームまで待機
    //     }

    //     // 最終位置を正確に補正
    //     transform.position = targetPosition;
    //     moveCoroutine = null;
    // }

    public void Onjump(InputAction.CallbackContext context)
    {
        //端から設定した分の距離内にいればじっこう
        if (context.performed)
        {
            if (possiblejumppos < transform.position.x && hasiobj.transform.position.x > transform.position.x)
            {
                Debug.Log("jumpinghopping");
                float difference = hasiobj.transform.position.x - transform.position.x;
                Debug.Log(difference);
                ispressjumpbutton = true;
                buttobi.SetJumpconfig(speedmanager.speed * (acceleeration - (difference * 20)), speedmanager.speed);
                StartCoroutine(evaluateDifference(difference));
            }

        }
    }

    IEnumerator evaluateDifference(float difference)
    {
        // 一旦時間を止める
        Time.timeScale = 0f;

        float targetAlpha = 1f; // 目標の透明度
        float threshold = 0.95f; // ループ終了のしきい値

        if (difference < 3)
        {
            FadeText.color = Color.yellow;
            FadeText.text = perfectword.ToString();
            AudioManager.I.PlaySFX(SoundKey.Jump);
        }
        else
        {
            FadeText.color = Color.white;
            FadeText.text = goodword.ToString();
            AudioManager.I.PlaySFX(SoundKey.goodjump);
        }


        // alpha値が一定以上になるまでLerpで上げる
        while (FadeText.alpha < threshold)
        {
            FadeText.alpha = Mathf.Lerp(FadeText.alpha, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);
            yield return null; // 次のフレームまで待機
        }

        // 最終的にalphaをMAXにする
        FadeText.alpha = 1f;

        // 1秒（リアルタイム）待つ
        yield return new WaitForSecondsRealtime(1f);

        FadeText.alpha = 0f;

        // 時間を再開する
        Time.timeScale = 1f;
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("ground"))
    //     {
    //         Debug.Log("zimenn");
    //         isfinish = true;
    //         // 任意の処理をここに書く
    //         // 例）体力を減らす、エフェクトを出すなど
    //     }
    // }


    void Update()
    {
        if (!FC.isjumpfaze) return;

        // Vector3 localOffset = new Vector3(0f, 0f, 0f);
        // Vector3 rayStart = transform.TransformPoint(localOffset);
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, rayLength, groundLayer))
        {
            col.isTrigger = false;
            Debug.Log("ColliderのTriggerをオフにしました");
            Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.green);
            Debug.Log("out");

            isjumping = true;
            
            if (ispressjumpbutton == false)
            {
                AudioManager.I.PlaySFX(SoundKey.miss);
            }

            
        }
        else
        {

            // 下に何もない場合の処理
            Debug.Log("下にオブジェクトがあります");
            // --- ここに処理を書く ---
            // 例：落下判定、警告エフェクトなど
        }
    }
}
