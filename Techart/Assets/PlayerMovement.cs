using Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CinemachineVirtualCamera fpsCam;
    [SerializeField] private CinemachineVirtualCamera tpsCam;
    
    private Rigidbody _rb;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ReadSwitchCameraInput();
        
        var moveVector = new Vector3(Input.GetAxis("Horizontal") * speed, _rb.velocity.y, Input.GetAxis("Vertical") * speed);

        _rb.velocity = moveVector;

        // slantChain.weight = Mathf.Lerp(slantChain.weight, _isSlant ? 0.5f : 0f, Time.deltaTime * slantSpeed);
        //
        // if (!Input.GetKeyDown(KeyCode.Space))
        //     return;
        //
        // _isSlant = !_isSlant;
    }

    private void ReadSwitchCameraInput()
    {
        if (!Input.GetKeyDown(KeyCode.V))
            return;
        
        fpsCam.gameObject.SetActive(!fpsCam.gameObject.activeInHierarchy);
        tpsCam.gameObject.SetActive(!tpsCam.gameObject.activeInHierarchy);
    }
}
