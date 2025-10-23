using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class sinmove : MonoBehaviour
{
    private int movepositonindex = 2;
    public float moveSpeed = 2f; // 移動スピード
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;

    //プレイヤーの移動できるポイントを５つ設定
    Vector3[] movepositions = new Vector3[]
    {
        new Vector3(-5f, -0.7f, 4.7f),
        new Vector3(-5f, -0.7f, 2.4f),
        new Vector3(-5f, -0.7f, 0f),
        new Vector3(-5f, -0.7f, -2.4f),
        new Vector3(-5f, -0.7f, -4.7f)
    };
    // 対象のレイヤーを指定（インスペクターで設定できるようにする）
    
    [SerializeField] private speedmanager speedmanager;

    
    void Start()
    {
        gameObject.transform.localPosition = movepositions[2];
    }



    public void Leftmove(InputAction.CallbackContext context)
    {

        if (context.performed && fazecontroller.isjumpfaze == false)
        {
            Debug.Log("Left");
            if (movepositonindex > 0)
            {
                movepositonindex -= 1;
            }

            SetTarget(movepositions[movepositonindex],moveSpeed);
        }

    }
    public void Rightmove(InputAction.CallbackContext context)
    {
        Debug.Log("i");
        if (context.performed && fazecontroller.isjumpfaze == false)
        {
            Debug.Log("right");
            if (movepositonindex < 4)
            {
                movepositonindex += 1;
            }

            SetTarget(movepositions[movepositonindex],moveSpeed);
        }
    }


    // 新しいターゲット座標をセット
    public void SetTarget(Vector3 newTarget, float setspeed)
    {
        // ターゲットが更新されたら、今の移動を中断
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        targetPosition = newTarget;
        moveCoroutine = StartCoroutine(MoveToTarget(setspeed));
    }
    private IEnumerator MoveToTarget(float speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            yield return null; // 次のフレームまで待機
        }

        // 最終位置を正確に補正
        transform.position = targetPosition;
        moveCoroutine = null;
    }

}
