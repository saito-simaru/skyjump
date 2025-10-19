using UnityEngine;
using System.Collections;
public class createwall : MonoBehaviour
{
    Vector3[] spawnpositions = new Vector3[]
    {
        new Vector3(2f, 0.6f, 0.4f),
        new Vector3(2f, 0.6f, 0.2f),
        new Vector3(2f, 0.6f, 0f),
        new Vector3(2f, 0.6f, -0.2f),
        new Vector3(2f, 0.6f, -0.4f)
    };

    [SerializeField] private GameObject prefab;  // 生成したいPrefab
    [SerializeField] private Transform parentObject; // 子にしたいオブジェクト
    [SerializeField] private speedmanager speedmanager;
    int randomValue;
    private float waittime;
    [SerializeField]private float[] waitlevels = new float[3];

    void Start()
    {
        StartCoroutine(createwalls());

    }
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
        if (speedmanager.speed > 4)
        {
            waittime = waitlevels[2];
        }
        else if (speedmanager.speed > 2)
        {
            waittime = waitlevels[1];
        }
        else
        {
            waittime = waitlevels[0];
        }
    }

    private IEnumerator createwalls()
    {
        while(true)
        {
            randomValue = Random.Range(0, 5);

            GameObject instance = Instantiate(prefab, parentObject);
            instance.transform.localPosition = spawnpositions[randomValue]; // 親を基準にした座標
            instance.transform.localRotation = Quaternion.identity;

            // --- 親のスケールや回転を引き継がないようにする場合 ---
            // instance.transform.localScale = Vector3.one;
            // instance.transform.localRotation = Quaternion.identity;

            yield return new WaitForSeconds(waittime); // 0.5秒待つ
        }

    }
}
