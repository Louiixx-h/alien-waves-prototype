using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts
{
    public class DashButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; set; }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
        }
    }
}