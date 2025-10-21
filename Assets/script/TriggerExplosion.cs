using UnityEditor.Callbacks;
using UnityEngine;

public class TriggerExplosion : MonoBehaviour
{
    [SerializeField] private float explosionForce = 10f;   // 吹っ飛ばす強さ
    [SerializeField] private float explosionRadius = 5f;   // 効果範囲
    [SerializeField] private bool affectUpward = true;     // 上方向成分を加えるか
    Vector3 explosionPos;

    private void OnTriggerEnter(Collider other)
    {
        // Rigidbody rb = other.attachedRigidbody;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        BoxCollider col = gameObject.GetComponent<BoxCollider>();

        // Rigidbodyを持つオブジェクトのみ吹っ飛ばす
        if (rb != null)
        {
            // 自分（Trigger）の位置
            Vector3 UnderPos = col.bounds.min;
            explosionPos = new Vector3(transform.position.x, UnderPos.y, transform.position.z);
            Debug.Log(explosionPos);

            // 上方向補正を加えたい場合
            if (affectUpward)
                explosionPos += Vector3.up * 1.0f;

            // Rigidbodyに爆発的な力を与える
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.5f, ForceMode.Impulse);

            Debug.Log($"{other.name} を吹っ飛ばしました！");
        }
    }

    [SerializeField] private Color gizmoColor = new Color(1f, 0.5f, 0f, 0.3f); // 半透明オレンジ

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
