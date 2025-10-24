using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class wall : MonoBehaviour
{
    private Vector3 currentposition;
    private Vector3 targetPosition;
    [SerializeField] private speedmanager speedmanager;
    [SerializeField] private LayerMask discriptiontarget;
    
    private GameObject player;
    private Blink blink;
    private float currentSpeed;
    

    private void OnEnable()
    {
        speedmanager.OnChanged += HandleChanged;
        HandleChanged(); // 初回にも現在の設定を反映
    }

    private void OnDisable()
    {
        speedmanager.OnChanged -= HandleChanged;
    }

    private void HandleChanged()
    {
        Debug.Log("cahngespped");
        currentSpeed = speedmanager.speed;
    }
    void Start()
    {
        player = GameObject.Find("player");
        blink = player.GetComponent<Blink>();

        //生成時に現在座標からｘ座標だけターゲット位置を差し替えたVector3にする。ー１はプレイヤーに向かって画面外へ消えるまでのｘ座標
        currentposition = gameObject.transform.localPosition;
        targetPosition = new Vector3(-1, currentposition.y, currentposition.z);
        StartCoroutine(MoveToTarget());

    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetPosition,
                currentSpeed * Time.deltaTime
            );

            yield return null; // 次のフレームまで待機
        }

        // 最終位置を正確に補正
        transform.localPosition = targetPosition;
        Destroy(gameObject);
    }
        // Triggerに何かが入ったときに呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // other.gameObject が discriptiontarget に含まれているか判定
        //blinkingで無敵状態を判別
        if (((1 << other.gameObject.layer) & discriptiontarget) != 0 && !blink.isBlinking)
        {
            AudioManager.I.PlaySFX(SoundKey.Conflict);
            Debug.Log($"{other.gameObject.name} が Trigger に入りました！（Layer: {LayerMask.LayerToName(other.gameObject.layer)}）");
            speedmanager.AddDelta(-0.5f);
            blink.StartBlink();
        }

    }
}
