using UnityEngine;

public class Wobble : MonoBehaviour
{
    private static readonly int _wobbleX = Shader.PropertyToID("_WobbleX");
    private static readonly int _wobbleZ = Shader.PropertyToID("_WobbleZ");
    [SerializeField] private float maxWobble = 1f;
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float recover = 1;
    
    private Renderer _rend;
    private Vector3 _lastPos;
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    private Quaternion _lastRot;
    private float _wobbleAmountX;
    private float _wobbleAmountZ;
    private float _wobbleAmountToAddX;
    private float _wobbleAmountToAddZ;
    private float _pulse;
    private float _time = 0.5f;

    private void Start()
    {
        _rend = GetComponent <Renderer>();
        _lastPos = transform.position;
        _lastRot = transform.rotation;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        
        _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0, Time.deltaTime * recover);
        _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0, Time.deltaTime * recover);
        
        _pulse = 2 * Mathf.PI * wobbleSpeed;
        
        _wobbleAmountX = _wobbleAmountToAddX * Mathf.Sin(_pulse * _time);
        _wobbleAmountZ = _wobbleAmountToAddZ * Mathf.Sin(_pulse * _time);
        
        _rend.material.SetFloat(_wobbleX, _wobbleAmountX);
        _rend.material.SetFloat(_wobbleZ, _wobbleAmountZ);
        
        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _angularVelocity = (transform.rotation.eulerAngles - _lastRot.eulerAngles) / Time.deltaTime;
        
        _wobbleAmountToAddX += Mathf.Clamp((_velocity.x + _angularVelocity.z * 0.2f) * maxWobble, -maxWobble, maxWobble); 
        _wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + _angularVelocity.x * 0.2f) * maxWobble, -maxWobble, maxWobble);
        
        _lastPos = transform.position;
        _lastRot = transform.rotation;
    }
}
