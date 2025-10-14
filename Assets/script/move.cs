using Unity.VisualScripting;
using UnityEngine;

public class move : MonoBehaviour
{
    //プレイヤーの移動できるポイントを５つ設定
    Vector3[] movepositions = new Vector3[]
    {
        new Vector3(-0.35f, 0.6f, 0.4f),
        new Vector3(-0.35f, 0.6f, 0.2f),
        new Vector3(-0.35f, 0.6f, 0f),
        new Vector3(-0.35f, 0.6f, -0.2f),
        new Vector3(-0.35f, 0.6f, -0.4f)
    };

    public void OnMove()
    {
        Debug.Log("横移動を開始");
    }
}
