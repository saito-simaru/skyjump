using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "speedconfig", menuName = "Configs/speedconfig")]
public class speedmanager : ScriptableObject
{
    [Range(0.5f, 20f)]
    public float speed = 3f;

    public event Action OnChanged;

    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 20f;
    // public void SetSpeed(float changevalue)
    // {
    //     //変化量がマイナス且つ現在のスピードと足した時に下限値を下回ったら変化させない
    //     if (changevalue < 0 && speed + changevalue < minspeed) return;
    //     speed += changevalue;
    //     OnChanged?.Invoke(); // 登録された全てに通知
    // }
    public void SetAbsolute(float value)
    {
        float clamped = Mathf.Clamp(value, minSpeed, maxSpeed);
        if (Mathf.Approximately(clamped, speed)) return;

        speed = clamped;
        OnChanged?.Invoke();
    }

    // ② 差分で加算/減算
    public void AddDelta(float delta)
    {
        if (Mathf.Approximately(delta, 0f)) return;

        float before = speed;
        float after  = Mathf.Clamp(before + delta, minSpeed, maxSpeed);
        if (Mathf.Approximately(after, before)) return;

        speed = after;
        OnChanged?.Invoke();
    }
}
