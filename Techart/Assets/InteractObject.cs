using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InteractObject : MonoBehaviour
{
    [field: SerializeField] public Transform TouchPoint { get; private set; }
    [field: SerializeField] public Outline Outline { get; private set; }

    public void SetOutlineState(bool state) => Outline.enabled = state;
}
