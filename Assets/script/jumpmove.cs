using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class jumpmove : MonoBehaviour
{
    private Vector3 targetPosition;
    private Coroutine moveCoroutine;
    public float moveSpeed = 2f; // 移動スピード
    public void Setjumppoint(Vector3 newTarget,float Speed)
    {
                // ターゲットが更新されたら、今の移動を中断
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveSpeed = Speed;
        targetPosition = newTarget;
        moveCoroutine = StartCoroutine(MoveToTarget(moveSpeed));
    }
    private IEnumerator MoveToTarget(float Speed)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPosition,
                Speed * Time.deltaTime
            );

            yield return null; // 次のフレームまで待機
        }

        // 最終位置を正確に補正
        transform.localPosition = targetPosition;
        moveCoroutine = null;
    }
}
