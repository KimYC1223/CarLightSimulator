using UnityEngine;

namespace Logic
{
    public class ObjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject human;
        [SerializeField] private GameObject otherCar;

        public static ObjectManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ToggleHuman()
        {
            human.SetActive(!human.activeSelf);
        }

        public void ToggleOtherCar()
        {
            otherCar.SetActive(!otherCar.activeSelf);
        }
    }
}