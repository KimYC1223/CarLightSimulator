using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{   
    public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private readonly int hoverTriggerHash = Animator.StringToHash("Hover");

        [SerializeField] private UnityEvent onClick;
        [SerializeField] private Animator animator;

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                return;
            }

            animator.SetBool(hoverTriggerHash, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.SetBool(hoverTriggerHash, false);
        }
    }
}
