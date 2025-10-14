using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class move : MonoBehaviour
{
    private int movepositonindex = 2;
    
    //プレイヤーの移動できるポイントを５つ設定
    Vector3[] movepositions = new Vector3[]
    {
        new Vector3(-0.35f, 0.6f, 0.4f),
        new Vector3(-0.35f, 0.6f, 0.2f),
        new Vector3(-0.35f, 0.6f, 0f),
        new Vector3(-0.35f, 0.6f, -0.2f),
        new Vector3(-0.35f, 0.6f, -0.4f)
    };

    void Start()
    {
        gameObject.transform.localPosition = movepositions[2];
    }



    public void Leftmove(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            Debug.Log("Left");
            if (movepositonindex > 0)
            {
                movepositonindex -= 1;
            }

            gameObject.transform.localPosition = movepositions[movepositonindex];
        }

    }
    public void Rightmove(InputAction.CallbackContext context)
    {
        Debug.Log("i");
        if (context.performed)
        {
            Debug.Log("right");
            if (movepositonindex < 4)
            {
                movepositonindex += 1;
            }

            gameObject.transform.localPosition = movepositions[movepositonindex];
        }
    }
}
