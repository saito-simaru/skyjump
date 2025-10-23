using UnityEngine;
using UnityEngine.SceneManagement; // 必須

public class SceneSwitcher : MonoBehaviour
{
    [Tooltip("遷移先のシーン名（Build Settingsに登録されている名前）")]
    public string targetSceneName = "GameScene"; // 例としてデフォルトを設定

    /// <summary>
    /// 指定されたシーン名へ遷移を開始します。
    /// </summary>
    public void GoToTargetScene()
    {
        // SceneManager.LoadSceneにシーン名を渡す
        SceneManager.LoadScene(targetSceneName);
    }

    /// <summary>
    /// シーン名を引数として受け取る汎用メソッド（ボタンイベントなどに便利）
    /// </summary>
    /// <param name="sceneName">遷移先のシーン名</param>
    public void GoToSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}