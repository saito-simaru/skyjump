using UnityEngine;
using System.Collections;

public class fazecontroller : MonoBehaviour
{
    [SerializeField] private float delayTime = 30f;
    [SerializeField] private GameObject createwall;
    private void Start()
    {
        StartCoroutine(TriggerAfterDelay());
    }

    private IEnumerator TriggerAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log($"{delayTime}秒経過！イベント発動！");
        createwall.SetActive(false);
    }
}
