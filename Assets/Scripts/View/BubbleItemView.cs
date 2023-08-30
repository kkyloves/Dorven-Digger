using UnityEngine;

namespace View
{
    public class BubbleItemView
    {
        private const float MOVE_DOWN_AMOUNT = 0.4f;
        
        private readonly Transform m_bubbleItemTransform;
        private readonly SpriteRenderer m_spriteRenderer;
        private readonly Animator m_animator;
        
        private static readonly int PopTriggerName = Animator.StringToHash("Pop");

        public BubbleItemView(Transform p_transform, SpriteRenderer p_spriteRenderer, Animator p_animator)
        {
            m_bubbleItemTransform = p_transform;
            m_spriteRenderer = p_spriteRenderer;
            m_animator = p_animator;
        }

        public void Init(Sprite p_sprite)
        {
            m_spriteRenderer.sprite = p_sprite;
        }
        
        public void MoveDown()
        {
            m_bubbleItemTransform.position = new Vector2(m_bubbleItemTransform.position.x, m_bubbleItemTransform.position.y - MOVE_DOWN_AMOUNT);
        }

        public void Pop()
        {
            //m_animator.enabled = true;
           //m_animator.SetTrigger(PopTriggerName);
        }
    }
}
