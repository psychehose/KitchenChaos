using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {


    public static Player Instance { get; set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs: EventArgs {
        public ClearCounter selectedCounter;
    }

    private bool isWalking;

    private Vector3 lastInteractDir;

    private ClearCounter selectedCounter;

    private KitchenObject kitchenObject;

    // Public 인 경우는 코드 은닉화를 달성할 수 없음
    // 그렇다고 해서 private로 설정하면 에디터에서 옵션을 만질 수가 없음.
    // 이때 [SerializField] 를 사용하면 다른 클래스에서 사용할 수 없고 (코드 은닉) 에디터에서도 사용가능.
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private Transform kitchenObjectHoldPoint;



    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Player가 한명보다 많습니다.");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }


    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                SetSelectedCounter(clearCounter);

            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }

    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        // Study Why 델타타임을 곱하지? 
        // 동일한 속도를 보장하기 위해서

        float moveDistance = Time.deltaTime * moveSpeed;

        float playerRadius = .7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance
        );

        if (!canMove) {
            // 움직일 수 없을 때
            
            // X 축으로만 누를 때

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;

            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance
            );

            if (canMove) {
                moveDir = moveDirX;
            } else {
                // x축으로 움직일 수 없음

                // z축 으로 움직임 시도

                Vector3 moveDirZ = new Vector3(0,0,moveDir.z).normalized;

                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance
                );

                if (canMove) {
                    moveDir = moveDirZ;
                } else {
                    // 어떤 방향으로도 이동 불가
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }



        isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;
        // 회전을 부드럽게 하는 lerp, slerp
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { 
            selectedCounter = selectedCounter
            });
    }


    // Implement: IKitchenObjectParent


    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }

    public bool hasKitchenObject() {
        return kitchenObject != null;
    }


}