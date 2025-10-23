using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public GameObject hasiobj;
    public TextMeshProUGUI ScoreText;
    void Start()
    {
        ScoreText.gameObject.SetActive(false);
    }

    void Update()
    {
        float score = transform.position.x - hasiobj.transform.position.x;
        if(score > 0)
        {
            ScoreText.gameObject.SetActive(true);
                    // 四捨五入して整数に変換
            int roundedValue = Mathf.RoundToInt(score);

            // 整数を文字列に変換してTextMeshProに表示
            ScoreText.text = roundedValue.ToString();

        }
    } 
}
