using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using System.ComponentModel; // 忘れずに追記
using System.Collections;
public class ScoreManager : MonoBehaviour
{
    public GameObject hasiobj;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI RestartText;
    public createwall createwall;
    public buttobi buttobi;
    private Vector2 resultpos = new Vector2(37 , 115);
    private bool valiableReset = false;
    void Start()
    {
        ScoreText.gameObject.SetActive(false);
        RestartText.gameObject.SetActive(false);
    }

    void Update()
    {
        float score = transform.position.x - hasiobj.transform.position.x;
        if (score > 0)
        {
            ScoreText.gameObject.SetActive(true);
            // 四捨五入して整数に変換
            int roundedValue = Mathf.RoundToInt(score);

            // 整数を文字列に変換してTextMeshProに表示
            ScoreText.text = roundedValue.ToString();

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "underground")
        {
            ScoreText.rectTransform.anchoredPosition = resultpos;
            RestartText.gameObject.SetActive(true);
            
            ScoreText.fontSize = 80;
            Debug.Log("ゲーム終了");
            valiableReset = true;
        }
    }

    // public void OnRestart()
    // {
    //     if (valiableReset)
    //     {
    //         jumpmove.isjumping = false;
    //         buttobi.isfinish = false;

    //         Rigidbody rigidbody = GetComponent<Rigidbody>();
    //         rigidbody.isKinematic = true;
    //         rigidbody.useGravity = false;

    //         createwall.enabled = true;
    //         StartCoroutine(Reset());
    //     }

    // }


    // private IEnumerator Reset()
    // {
    //     yield return null;
    //     // 1. 現在アクティブなシーンを取得
    //     Scene currentScene = SceneManager.GetActiveScene();

    //     // 2. そのシーンを再ロード
    //     SceneManager.LoadScene(currentScene.name);
    // }
}
