using UnityEngine;
using System.Collections;

public class fazecontroller : MonoBehaviour
{
    [SerializeField] private float delayTime = 30f;
    [SerializeField] private GameObject createwall;
    [SerializeField] private jumpmove jumpmove;
    [SerializeField] private sinmove move;
    [SerializeField] private Camera targetCamera; 
    [SerializeField] private Transform parentObject;
    private Vector3 jumpposition = new Vector3(-0.3f, 0.65f, 0);
    private Vector3 outposition = new Vector3(11f, 0.65f, 0);
    public static bool isjumpfaze = false;
    private void Start()
    {
        StartCoroutine(TriggerAfterDelay());
    }

    private IEnumerator TriggerAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        isjumpfaze = true;
        Debug.Log($"{delayTime}秒経過！イベント発動！");
        createwall.SetActive(false);

        jumpmove.Setjumppoint(jumpposition, 2);

        yield return new WaitForSeconds(5f);
        // カメラを親オブジェクトの子にする
        targetCamera.transform.SetParent(parentObject);

        jumpmove.Setjumppoint(outposition, 1);
    }
}
