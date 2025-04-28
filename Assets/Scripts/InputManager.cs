using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private bool isBlockingInput;
    private float lastMoveX, lastMoveY;

    public Vector2 nextPos;

    public static InputManager Instance { get; private set;} // Singletone

    // Input Manager의 모든 프로퍼티는 읽기 전용
    public bool IsBlockingInput
    {
        get {
            return isBlockingInput;
        }
        private set {
            isBlockingInput = value;
        }
    }

    public bool Action { get; set; }
    public bool Dash { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 프로퍼티 초기화
        X = 0.0f;
        Y = 0.0f;
        Action = false;
        Dash = false;
        IsBlockingInput = false;

        
    }

    // 플레이어 입력을 sec초 동안 막는다.
    public IEnumerator BlockInput(float sec)
    {
        IsBlockingInput = true;
        lastMoveX = X;
        lastMoveY = Y;

        yield return new WaitForSecondsRealtime(sec);
        
        Action = false;
        Dash = false;
        IsBlockingInput = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsTalking || context.canceled)
        {
            X = 0; Y = 0;
            return;
        }

        if (!IsBlockingInput)
        {
            Vector2 input = context.ReadValue<Vector2>();
            X = input.x;
            Y = input.y;
        }
        else 
        {
            X = lastMoveX;
            Y = lastMoveY;
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.started && !IsBlockingInput)
        {
            Action = true;
            GameManager.Instance.HandleAction();
        }

        if(context.canceled)
        {
            Action = false;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !Dash && !IsBlockingInput && !GameManager.Instance.IsTalking)
            Dash = true;

        if(context.canceled)
            Dash = false;
    }

    public void OnEscape()
    {
        if (!IsBlockingInput && !GameManager.Instance.IsTalking)
            GameManager.Instance.EscapeControl();
    }

    public void OnDamaged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.player.GetDamaged(10);
            UIManager.Instance.UpdateEtherUI();
            Debug.Log("10의 데미지를 입었다!");
        }
    }
}