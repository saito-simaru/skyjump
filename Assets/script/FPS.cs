using UnityEngine;

public class FPS : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;        
    }

}
