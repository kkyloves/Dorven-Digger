using UnityEngine;

namespace View
{
    public class BubbleShooterView
    {
        private const float X_AND_Y_ROTATION_CONSTANT_VALUE = 0f;
        private readonly Transform m_bubbleShooterTransform;

        public Quaternion GetCurrentRotation()
        {
            return m_bubbleShooterTransform.localRotation;
        }

        public BubbleShooterView(Transform p_transform)
        {
            m_bubbleShooterTransform = p_transform;
        }

        public void AdjustBubbleShooterRotation(float p_zValue)
        {
            m_bubbleShooterTransform.localRotation = Quaternion.Euler(X_AND_Y_ROTATION_CONSTANT_VALUE, X_AND_Y_ROTATION_CONSTANT_VALUE, p_zValue);
        }
    }
}
