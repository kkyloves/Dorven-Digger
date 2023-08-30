using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Controller
{
    public class TrajectoryShooterController : MonoBehaviour
    {
        [SerializeField] private GameObject m_trajectoryPointPrefab;
        [SerializeField] private int m_numberOfTrajectoryPoints;
        [SerializeField] private float m_spacesBetweenTrajectoryPoints;

        [SerializeField] private Transform m_launcherPosition;
        [SerializeField] private Transform m_directionPosition;

        private readonly List<TrajectoryItemController> m_trajectoryPoints = new();
        private bool m_isRevert;
        
        public void StartTrajectoryPoints()
        {
            for (var i = 0; i < m_numberOfTrajectoryPoints; i++)
            {
                var point = Instantiate(m_trajectoryPointPrefab, m_launcherPosition.position, Quaternion.identity);
                m_trajectoryPoints.Add(point.GetComponent<TrajectoryItemController>());
                point.transform.SetParent(transform);
            }

            UpdateTrajectoryPoints();
        }

        public void Init(InputManager p_inputManager)
        {
            p_inputManager.ArrowDelegate += mode =>
            {
                UpdateTrajectoryPoints();
                return false;
            };
        }

        private void UpdateTrajectoryPoints()
        {
            var m_adjustCounter = 0;

            for (var i = 0; i < m_numberOfTrajectoryPoints; i++)
            {
                var position = GetPointPosition(i * m_spacesBetweenTrajectoryPoints);
                //   m_trajectoryPoints[i].transform.position = new Vector3(position.x, position.y, 0f);

                if (position.x < -2.4f)
                {
                    var adjustment = 0.03f * m_adjustCounter++;
                    m_trajectoryPoints[i].transform.position = new Vector3(position.x + adjustment, position.y, 0f);
                    m_trajectoryPoints[i].IsRevert = true;
                }
                else
                {
                    m_trajectoryPoints[i].transform.position = new Vector3(position.x, position.y, 0f);
                }
            }
        }

        private Vector2 GetPointPosition(float t)
        {
            var position = (Vector2)m_launcherPosition.position +
                           (Vector2)m_directionPosition.transform.up.normalized * 50f * t +
                           0.5f * Physics2D.gravity * (t * t);
            return position;
        }
    }
}