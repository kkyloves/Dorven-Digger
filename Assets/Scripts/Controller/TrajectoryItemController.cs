using UnityEngine;

namespace Controller
{
    public class TrajectoryItemController : MonoBehaviour
    {
        public bool m_isRevert;
        public bool IsRevert { get; set; }
    
        
        private void OnTriggerEnter2D(Collider2D col)
        {

            if (col.tag.Equals("Side Wall"))
            {
                m_isRevert = true;
            }
        }

    }
}
