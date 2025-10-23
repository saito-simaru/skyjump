using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public jumpmove jumpmove;
    public buttobi buttobi;
    public CinemachineCamera cam1;
    public CinemachineCamera cam2;
    public CinemachineCamera cam3;
    public sinmove sinmove;
    void Start()
    {
        ActivateCamera(cam1);
    }
    void Reset()
    {
        ActivateCamera(cam1);
    }
    void Update()
    {
        if (jumpmove.isjumping == true)
        {
            ActivateCamera(cam2);
            // jumpmove.isjumping = false;
        }
        if (buttobi.isfinish == true)
        {
            ActivateCamera(cam3);
            // buttobi.isfinish = false;
        }
    }

    void ActivateCamera(CinemachineCamera activeCam)
    {
        // すべてのカメラのPriorityをリセット
        cam1.Priority = 0;
        cam2.Priority = 0;
        cam3.Priority = 0;

        // 指定カメラだけを高優先度に
        activeCam.Priority = 10;
    }
}
