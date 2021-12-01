using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    float Sensivity;

    [SerializeField]
    InputAction upInput, downInput, leftInput, rightInput,
        interactionInput;

    [SerializeField]
    InputAction exitInput;

    static float up, down, left, right;
    public static float vertical => up + down;
    public static float horizontal => left + right;
    public static bool upDown, downDown, leftDown, rightDown;
    
    public static bool ineraction, exit;

    private void Awake()
    {
        upInput.performed += context => {
            up = 1; upDown = true;
        };
        downInput.performed += context => {
            down = -1; downDown = true;
        };
        leftInput.performed += context => {
            left = -1; leftDown = true;
        };
        rightInput.performed += context => { right = 1; rightDown = true; };

        upInput.canceled += context => { up = 0; };
        downInput.canceled += context => { down = 0; };
        leftInput.canceled += context => { left = 0; };
        rightInput.canceled += context => { right = 0; };
        

        interactionInput.performed += context => ineraction = true;
        exitInput.performed += context => exit = true;
    }

    private void OnEnable()
    {
        upInput.Enable();
        downInput.Enable();
        leftInput.Enable();
        rightInput.Enable();

        interactionInput.Enable();
        exitInput.Enable();
    }

    private void OnDisable()
    {
        upInput.Disable();
        downInput.Disable();
        leftInput.Disable();
        rightInput.Disable();

        interactionInput.Disable();
        exitInput.Disable();
    }

    private void LateUpdate()
    {
        ineraction = false;
        exit = false;

        upDown = false;
        downDown = false;
        leftDown = false;
        rightDown = false;
    }
}
