using System;
using Controller;
using Manager;
using UnityEngine;
using View;

namespace Model
{
    public class BubbleShooterModel
    {
        private const float ADDITIONAL_Z_VALUE = 0.2f;
        private const float LEFT_Z_VALUE_LIMIT = 40f;
        private const float RIGHT_Z_VALUE_LIMIT = -40f;

        private readonly BubbleShooterView m_bubbleShooterView;
        private readonly BubbleShooterController m_bubbleShooterController;
        private readonly Transform m_shootLauncherPoint;
        
        private float m_currentZValue;
        public bool CanShoot {get; set;}
        
        public BubbleShooterModel(BubbleShooterController p_bubbleShooterController, BubbleShooterView p_bubbleShooterView , InputManager p_inputManager)
        {
            m_bubbleShooterView = p_bubbleShooterView;
            m_bubbleShooterController = p_bubbleShooterController;

            p_inputManager.ArrowDelegate += mode =>
            {
                AdjustBubbleShooterRotation(mode);
                return false;
            };
            
            p_inputManager.ShootDelegate += () =>
            {
                ShootBubbles();
                return false;
            };
        }

        private void AdjustBubbleShooterRotation(RotationMode p_mode)
        {
            switch (p_mode)
            {
                case RotationMode.Left: 
                    m_currentZValue += ADDITIONAL_Z_VALUE;
                    if (m_currentZValue > LEFT_Z_VALUE_LIMIT)
                    {
                        m_currentZValue = LEFT_Z_VALUE_LIMIT;
                    }
                    break;
                
                case RotationMode.Right: 
                    m_currentZValue -= ADDITIONAL_Z_VALUE; 
                    if (m_currentZValue < RIGHT_Z_VALUE_LIMIT)
                    {
                        m_currentZValue = RIGHT_Z_VALUE_LIMIT;
                    }
                    break;
                
                default: throw new ArgumentOutOfRangeException(nameof(p_mode), p_mode, null);
            }

            m_bubbleShooterView.AdjustBubbleShooterRotation(m_currentZValue);
        }

        private void ShootBubbles()
        {
            if (CanShoot)
            {
                CanShoot = false;
                m_bubbleShooterController.ShootBubbles();
            }
        }
        
        public float GetShootingRotation()
        {
            var shooterRotation = m_bubbleShooterView.GetCurrentRotation().eulerAngles.z;
            
            float ballRotation = 90;
            if (shooterRotation <= 360 &&  shooterRotation >= 270.0)
            {
                ballRotation = shooterRotation - 270;
            }
            if (shooterRotation is <= 90 and >= 0){
                ballRotation = 90 + shooterRotation;
            }
            
            return ballRotation;
        }
    }
}
