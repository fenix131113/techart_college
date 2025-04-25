using UnityEngine;

namespace Light_Hearted_Tech.SentryRobot.Prefabs
{
    public class FootMover : MonoBehaviour
    {
        public Vector3 NewTarget { get; set; }

        [SerializeField] private Transform targetPoint;
        [SerializeField] private float distance;
        [SerializeField] private float maxHeightDistance;

        [SerializeField] private float countLerpPosition = 0.4f;
        [SerializeField] private float countLerpHeight = 0.5f;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float amplitude = 0.4f;

        private float _currentTime = 1f;

        private void Start()
        {
            NewTarget = targetPoint.position;
        }

        private void Update()
        {
            if (Physics.Raycast(transform.position, -transform.up, out var hit,
                    maxHeightDistance))
            {
                if (Vector3.Distance(hit.point, targetPoint.position) > distance)
                {
                    _currentTime = 0;
                    NewTarget = hit.point;
                }

                if (_currentTime < 1)
                {
                    var footPos = Vector3.Lerp(targetPoint.position, hit.point, countLerpPosition);
                    footPos.y = Mathf.Lerp(footPos.y, hit.point.y, countLerpHeight) +
                                Mathf.Sin(_currentTime * Mathf.PI) * amplitude;

                    targetPoint.position = footPos;
                    
                    _currentTime += Time.deltaTime * speed;
                }
            }
        }
    }
}