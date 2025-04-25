using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    private static readonly int _threshold = Shader.PropertyToID("_DissolveThreshold");
    
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float dissolveSpeed = 0.5f;

    private float _dissolveThreshold = 0f;

    private void Update()
    {
        _dissolveThreshold += Time.deltaTime * dissolveSpeed;
        dissolveMaterial.SetFloat(_threshold, _dissolveThreshold);

        if (_dissolveThreshold > 1f)
        {
            _dissolveThreshold = 0f;
        }
    }
}