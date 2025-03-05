using UnityEngine;

namespace Logic
{
    public class LightManager : MonoBehaviour
    {
        private const string FrontImageName = "front.png";
        private const string BackImageName = "back.png";
        private const string LeftImageName = "left.png";
        private const string RightImageName = "right.png";

        [SerializeField] private SpriteRenderer frontLight;
        [SerializeField] private SpriteRenderer backLight;
        [SerializeField] private SpriteRenderer leftLight;
        [SerializeField] private SpriteRenderer rightLight;

        public static LightManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ToggleFrontLight()
        {
            frontLight.gameObject.SetActive(!frontLight.gameObject.activeSelf);
            if(frontLight.gameObject.activeSelf)
            {
                frontLight.sprite = StreamingAssetsManager.GetSprite(FrontImageName);
            }
            else
            {
                frontLight.sprite = null;
            }
        }

        public void ToggleBackLight()
        {
            backLight.gameObject.SetActive(!backLight.gameObject.activeSelf);
            if(backLight.gameObject.activeSelf)
            {
                backLight.sprite = StreamingAssetsManager.GetSprite(BackImageName);
            }
            else
            {
                backLight.sprite = null;
            }
        }

        public void ToggleLeftLight()
        {
            leftLight.gameObject.SetActive(!leftLight.gameObject.activeSelf);
            if(leftLight.gameObject.activeSelf)
            {
                leftLight.sprite = StreamingAssetsManager.GetSprite(LeftImageName);
            }
            else
            {
                leftLight.sprite = null;
            }
        }

        public void ToggleRightLight()
        {
            rightLight.gameObject.SetActive(!rightLight.gameObject.activeSelf);
            if(rightLight.gameObject.activeSelf)
            {
                rightLight.sprite = StreamingAssetsManager.GetSprite(RightImageName);
            }
            else
            {
                rightLight.sprite = null;
            }
        }
    }
}