using UnityEngine;

public class Raycaster : MonoBehaviour
{
    [SerializeField] private float maxRayDistance;
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private InteractionsController controller;

    private InteractObject _lastInteracted;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hit, maxRayDistance,
                interactionLayers))
        {
            _lastInteracted = hit.collider.GetComponent<InteractObject>();

            if (!_lastInteracted)
                return;

            _lastInteracted.SetOutlineState(true);
            if (Input.GetKeyDown(KeyCode.E))
                controller.SwitchInteraction(_lastInteracted);
        }
        else if (_lastInteracted)
        {
            _lastInteracted.SetOutlineState(false);
            _lastInteracted = null;
        }
    }
}