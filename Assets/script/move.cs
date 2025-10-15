using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class move : MonoBehaviour
{
    private int movepositonindex = 2;
    public float moveSpeed = 2f; // 移動スピード
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;
    
    //プレイヤーの移動できるポイントを５つ設定
    Vector3[] movepositions = new Vector3[]
    {
        new Vector3(-0.35f, 0.6f, 0.4f),
        new Vector3(-0.35f, 0.6f, 0.2f),
        new Vector3(-0.35f, 0.6f, 0f),
        new Vector3(-0.35f, 0.6f, -0.2f),
        new Vector3(-0.35f, 0.6f, -0.4f)
    };

    void Start()
    {
        gameObject.transform.localPosition = movepositions[2];
    }



    public void Leftmove(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            Debug.Log("Left");
            if (movepositonindex > 0)
            {
                movepositonindex -= 1;
            }

            SetTarget(movepositions[movepositonindex]);
        }

    }
    public void Rightmove(InputAction.CallbackContext context)
    {
        Debug.Log("i");
        if (context.performed)
        {
            Debug.Log("right");
            if (movepositonindex < 4)
            {
                movepositonindex += 1;
            }

            SetTarget(movepositions[movepositonindex]);
        }
    }


    // 新しいターゲット座標をセット
    public void SetTarget(Vector3 newTarget)
    {
        // ターゲットが更新されたら、今の移動を中断
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        targetPosition = newTarget;
        moveCoroutine = StartCoroutine(MoveToTarget());
    }
    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null; // 次のフレームまで待機
        }

        // 最終位置を正確に補正
        transform.localPosition = targetPosition;
        moveCoroutine = null;
    }
    // // コルーチンでなめらかに移動
    // private IEnumerator MoveToTarget()
    // {
    //     while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
    //     {
    //         transform.localPosition = Vector3.Lerp(
    //             transform.localPosition,
    //             targetPosition,
    //             Time.deltaTime * moveSpeed
    //         );

    //         // フレームをまたいで継続（フリーズ防止）
    //         yield return null;
    //     }

    //     // 最終的に誤差を補正
    //     transform.localPosition = targetPosition;
    //     moveCoroutine = null;
    // }
}
