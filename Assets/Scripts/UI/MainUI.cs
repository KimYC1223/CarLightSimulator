using UnityEngine;
using Logic;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        private readonly int openTriggerHash = Animator.StringToHash("Open");

        [SerializeField] private Animator animator;

        public void CloseUI()
        {
            animator.SetBool(openTriggerHash, false);
        }

        public void OpenUI()
        {
            animator.SetBool(openTriggerHash, true);
        }

        public void OpenDirectory()
        {
            StreamingAssetsManager.OpenStreamingAssetsDirectory();
        }

        public void ToggleHuman()
        {
            ObjectManager.Instance.ToggleHuman();
        }

        public void ToggleOtherCar()
        {
            ObjectManager.Instance.ToggleOtherCar();
        }

        public void ToggleFrontLight()
        {
            LightManager.Instance.ToggleFrontLight();
        }

        public void ToggleBackLight()
        {
            LightManager.Instance.ToggleBackLight();
        }

        public void ToggleLeftLight()
        {
            LightManager.Instance.ToggleLeftLight();
        }
        
        public void ToggleRightLight()
        {
            LightManager.Instance.ToggleRightLight();
        }
    }
}
