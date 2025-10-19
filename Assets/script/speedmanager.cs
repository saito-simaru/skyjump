using UnityEngine;
using System;

[CreateAssetMenu(fileName = "speedconfig", menuName = "Configs/speedconfig")]
public class speedmanager : ScriptableObject
{
    [Range(0f, 20f)]
    public float speed = 3f;

    public event Action OnChanged;

    public void SetSpeed(float newSpeed)
    {
        if (Mathf.Approximately(speed, newSpeed)) return;
        speed = newSpeed;
        OnChanged?.Invoke(); // 登録された全てに通知
    }
}
