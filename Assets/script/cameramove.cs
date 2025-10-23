using UnityEngine;
using Unity.Cinemachine; // Unity6のCinemachine用

public class CameraFollowXOnly : MonoBehaviour
{
    [Header("追従対象（例：プレイヤー）")]
    public Transform target;

    [Header("オフセット")]
    public float xOffset = 0f;

    private Vector3 initialPosition; // カメラ初期位置

    void Start()
    {
        // カメラの初期位置を記録
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 現在のカメラ位置を取得
        Vector3 newPos = initialPosition;

        // Xだけターゲットに追従
        newPos.x = target.position.x + xOffset;

        // カメラ位置を更新
        transform.position = newPos;
    }
}
