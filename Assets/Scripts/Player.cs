using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    private bool isWalking;

    // Public 인 경우는 코드 은닉화를 달성할 수 없음
    // 그렇다고 해서 private로 설정하면 에디터에서 옵션을 만질 수가 없음.
    // 이때 [SerializField] 를 사용하면 다른 클래스에서 사용할 수 없고 (코드 은닉) 에디터에서도 사용가능.
    [SerializeField] private float moveSpeed = 7f;
    private void Update() {

        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputVector.x += 1;
        }

        // Study: normalized.
        inputVector = inputVector.normalized;


        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        // Study Why 델타타임을 곱하지? 
        // 동일한 속도를 보장하기 위해서
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        // 회전을 부드럽게 하는 lerp, slerp
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    public bool IsWalking() {
        return isWalking;
    }
}