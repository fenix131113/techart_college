using UnityEngine;

public class ColorHover : MonoBehaviour
{
    private static readonly int _hoverPos = Shader.PropertyToID("_HoverPos");
    private static readonly int _hover = Shader.PropertyToID("_Hover");
    private static readonly int _color = Shader.PropertyToID("_Color");
    private Material _mat;
    private float _hoverValue;
    private bool _isHovered;
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private Color objectColor = Color.white;

    private void Start()
    {
        _mat = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = _mat;
        _mat.SetFloat(_hover, 0f);
        _mat.SetColor(_color, objectColor);
        _mat.SetVector(_hoverPos, new Vector4(-1, -1, 0, 0));
    }

    private void Update()
    {
        var targetValue = _isHovered ? 1f : 0f;
        _hoverValue = Mathf.Lerp(_hoverValue, targetValue, transitionSpeed * Time.deltaTime);

        if (!_isHovered && _hoverValue <= 0.01f)
            _hoverValue = 0f;

        _mat.SetFloat(_hover, _hoverValue);

        if (_isHovered)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject == gameObject)
            {
                var uv = hit.textureCoord;
                _mat.SetVector(_hoverPos, new Vector4(uv.x, uv.y, 0, 0));
            }
        }

        if (!_isHovered && _hoverValue <= 0)
            _mat.SetVector(_hoverPos, new Vector4(-1, -1, 0, 0));
    }

    private void OnMouseEnter()
    {
        _isHovered = true;
    }

    private void OnMouseExit()
    {
        _isHovered = false;
    }
}