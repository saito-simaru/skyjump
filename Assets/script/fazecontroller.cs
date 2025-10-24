using UnityEngine;
using System.Collections;

public class fazecontroller : MonoBehaviour
{
    [SerializeField] private float delayTime = 30f;
    [SerializeField] private GameObject createwall;
    [SerializeField] private jumpmove jumpmove;
    [SerializeField] private sinmove sinmove;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Transform followcameraobj;
    [SerializeField] private Transform icepapy;
    private Vector3 jumpposition = new Vector3(-5f, -0.7f, 0f);
    private Vector3 outposition = new Vector3(206f, -0.7f, 0f);
    public bool isjumpfaze = false;
    [SerializeField] private int delayTojumpTime;
    [SerializeField] private float speedToJumpPosition;
    [SerializeField] private float speedToOutPositon;
    private void Start()
    {
        AudioManager.I.PlayBGM(SoundKey.Bgm, 2f);
        StartCoroutine(TriggerAfterDelay());
    }

    private IEnumerator TriggerAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        childDelete();
        isjumpfaze = true;
        Debug.Log($"{delayTime}秒経過！イベント発動！");
        createwall.SetActive(false);

        sinmove.SetTarget(jumpposition, speedToJumpPosition);

        yield return new WaitForSeconds(delayTojumpTime);
        // カメラを親オブジェクトの子にする
        // targetCamera.transform.SetParent(followcameraobj);

        sinmove.SetTarget(outposition, speedToOutPositon);
    }

    private void childDelete()
    {
        // 子オブジェクトを逆順で削除（安全）
        for (int i = icepapy.childCount - 1; i >= 0; i--)
        {
            Destroy(icepapy.GetChild(i).gameObject);
        }
    }
}
