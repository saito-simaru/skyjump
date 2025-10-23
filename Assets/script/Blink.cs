using UnityEngine;
using System.Collections;
public class Blink : MonoBehaviour
{
    [Header("ブリンクの設定")]
    [Tooltip("ブリンクの点滅間隔（秒）")]
    public float blinkInterval = 0.1f; // 点滅の速さ
    [Tooltip("ブリンク継続時間（秒）")]
    public float blinkDuration = 1.0f; // ブリンク全体の長さ
    [Tooltip("ブリンク対象のRenderer")]
    public Renderer targetRenderer;

    private bool isBlinking = false;
    public BoxCollider col;

    private void Reset()
    {
        // 自動でRendererを取得（設定し忘れ対策）
        targetRenderer = GetComponent<Renderer>();
        col = GetComponent<BoxCollider>();
    }

    /// <summary>
    /// 外部から呼び出してブリンクを開始
    /// </summary>
    public void StartBlink()
    {
        if (!isBlinking)
        {
            StartCoroutine(BlinkCoroutine());
        }
    }

    private IEnumerator BlinkCoroutine()
    {
        isBlinking = true;
        col.isTrigger = true;
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < blinkDuration)
        {
            if (targetRenderer != null)
                targetRenderer.enabled = visible;

            visible = !visible;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // 最後に表示状態に戻す
        if (targetRenderer != null)
            targetRenderer.enabled = true;

        col.isTrigger = false;
        isBlinking = false;
    }
}
