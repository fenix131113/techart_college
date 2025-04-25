using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InteractionsController : MonoBehaviour
{
    [SerializeField] private float slantSpeed;
    [SerializeField] private float breakSlantDistance;
    [SerializeField] private ChainIKConstraint slantChain;
    [SerializeField] private Transform pointTarget;

    private bool _isInteracting;
    private InteractObject _currentTarget;

    private void Update()
    {
        slantChain.weight = Mathf.Lerp(slantChain.weight, _isInteracting ? 1f : 0f, Time.deltaTime * slantSpeed);
        
        if(_currentTarget && Vector3.Distance(transform.position, _currentTarget.TouchPoint.position) > breakSlantDistance)
            StopInteract();
    }

    public void SwitchInteraction(InteractObject target)
    {
        if(_isInteracting)
            StopInteract();
        else
            StartInteract(target);
    }

    public void StartInteract(InteractObject target)
    {
        _isInteracting = true;
        pointTarget.position = target.TouchPoint.position;
        _currentTarget = target;
        Debug.Log($"Starting interaction with \"{target.gameObject.name}\"");
    }

    public void StopInteract()
    {
        _isInteracting = false;
        _currentTarget = null;
    }
}