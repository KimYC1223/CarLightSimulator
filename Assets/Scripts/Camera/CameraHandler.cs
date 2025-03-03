using UnityEngine;
using Utils;

namespace Camera
{
    public class CameraHandler : MonoBehaviour
    {
        private const string MouseScrollWheelAxisName = "Mouse ScrollWheel";
        private const float MouseScrollWheelSensitivity = 1f;
        private const float LeftInputScale = 0.01f;
        private const float RightInputScale = 0.001f;

        [Header("트랜스폼")]
        [SerializeField] private Transform cameraAnchor;
        [SerializeField] private Transform mainCamera;

        [Header("Damping System 최소 / 최대값")] [Space(15)]
        [SerializeField] private float verticalRotationMin = 1f;
        [SerializeField] private float verticalRotationMax = 89f;
        [SerializeField] private float zoomMax = -3f;
        [SerializeField] private float zoomMin = -20f;
        [SerializeField] private float moveAreaSize = 1f;

        [Header("마우스 민감도")]
        [SerializeField] private float mouseSensitivity = 30f;

        private DampingSystem<float> horizontalRotationDampingSystem;
        private DampingSystem<float> verticalRotationDampingSystem;
        private DampingSystem<float> zoomDampingSystem;
        private DampingSystem<Vector2> moveAreaDampingSystem;

        private float horizontalRotation;
        private float verticalRotation;
        private float tempHorizontalRotation;
        private float tempVerticalRotation;
        private bool isLeftMouseButtonClicking;
        private bool isRightMouseButtonClicking;
        private Vector2 leftMousePosition;
        private Vector2 rightMousePosition;
        private float zoom;
        private Vector2 initialPosition;
        private Vector2 currentPosition;
        private Vector2 tempPosition;

        private void OnEnable()
        {
            isLeftMouseButtonClicking = false;
            isRightMouseButtonClicking = false;
            InitDampingSystem();
        }
        
        private void InitDampingSystem()
        {
            horizontalRotation = cameraAnchor.localEulerAngles.y;
            verticalRotation = cameraAnchor.localEulerAngles.x;
            zoom = mainCamera.localPosition.z;
            initialPosition = new Vector2(transform.position.x, transform.position.z);
            currentPosition = initialPosition;

            tempHorizontalRotation = 0f;
            tempVerticalRotation = 0f;
            tempPosition = Vector2.zero;

            horizontalRotationDampingSystem = new DampingSystemFloat(
                frequnecy: 5f,
                dampingRatio: 2f,
                initialResponse: 0.0f,
                initialCondition: horizontalRotation);
            verticalRotationDampingSystem = new DampingSystemFloat(
                frequnecy: 5f,
                dampingRatio: 2f,
                initialResponse: 0.0f,
                initialCondition: verticalRotation);
            zoomDampingSystem = new DampingSystemFloat(
                frequnecy: 1f,
                dampingRatio: 1f,
                initialResponse: 0.0f,
                initialCondition: zoom);
            moveAreaDampingSystem = new DampingSystemVector2(
                frequnecy: 5f,
                dampingRatio: 2f,
                initialResponse: 0.0f,
                initialCondition: initialPosition);
        }

        private void Update()
        {
            ManageLeftMouseClick();
            ManageRightMouseClick();
            ManageMouseWheel();

            UpdateCameraTransform();
        }

        private void ManageLeftMouseClick()
        {
            var isMouseDown = Input.GetMouseButton(0);

            if(isMouseDown)
            {
                if(isLeftMouseButtonClicking == false)
                {
                    isLeftMouseButtonClicking = true;
                    OnLeftMouseButtonDown();
                }

                tempHorizontalRotation = 
                    (Input.mousePosition.x - leftMousePosition.x) * mouseSensitivity * LeftInputScale;
                tempVerticalRotation = 
                    (Input.mousePosition.y - leftMousePosition.y) * mouseSensitivity * LeftInputScale;
            }
            else if (isMouseDown == false && isLeftMouseButtonClicking)
            {
                isLeftMouseButtonClicking = false;

                OnLeftMouseButtonUp();
            }
        }

        private void OnLeftMouseButtonDown()
        {
            leftMousePosition = Input.mousePosition;
        }

        private void OnLeftMouseButtonUp()
        {
            var clampedVerticalRotation = 
                Mathf.Clamp(verticalRotation - tempVerticalRotation, verticalRotationMin, verticalRotationMax);

            horizontalRotation += tempHorizontalRotation;
            verticalRotation = clampedVerticalRotation;

            leftMousePosition = Vector2.zero;
            tempHorizontalRotation = 0f;
            tempVerticalRotation = 0f;
        }

         private void ManageRightMouseClick()
        {
            var isMouseDown = Input.GetMouseButton(1);

            if(isMouseDown)
            {
                if(isRightMouseButtonClicking == false)
                {
                    isRightMouseButtonClicking = true;
                    OnRightMouseButtonDown();
                }

                var inputVec = new Vector2(
                    x : (rightMousePosition.x - Input.mousePosition.x) * mouseSensitivity * RightInputScale,
                    y : (rightMousePosition.y - Input.mousePosition.y) * mouseSensitivity * RightInputScale);

                var cos = Mathf.Cos((cameraAnchor.localEulerAngles.y - 180f) * Mathf.Deg2Rad);
                var sin = Mathf.Sin((cameraAnchor.localEulerAngles.y - 180f) * Mathf.Deg2Rad);

                tempPosition = new Vector2(
                    x : - inputVec.y * sin + inputVec.x * cos,
                    y : - inputVec.y * cos + inputVec.x * sin);

            }
            else if (isMouseDown == false && isRightMouseButtonClicking)
            {
                isRightMouseButtonClicking = false;

                OnRightMouseButtonUp();
            }
        }

        private void OnRightMouseButtonDown()
        {
            rightMousePosition = Input.mousePosition;
        }

        private void OnRightMouseButtonUp()
        {
            var clampedX = Mathf.Clamp(currentPosition.x + tempPosition.x, 
                        initialPosition.x - moveAreaSize, 
                        initialPosition.x + moveAreaSize);
            var clampedY = Mathf.Clamp(currentPosition.y + tempPosition.y, 
                        initialPosition.y - moveAreaSize, 
                        initialPosition.y + moveAreaSize);

            currentPosition = new Vector2(clampedX, clampedY);

            leftMousePosition = Vector2.zero;
            tempPosition = Vector2.zero;
        }

        private void ManageMouseWheel()
        {
            var mouseWheel = Input.GetAxis(MouseScrollWheelAxisName);
            switch(mouseWheel)
            {
                case < 0f:
                    zoom -= MouseScrollWheelSensitivity;
                    zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
                    break;
                case > 0f:
                    zoom += MouseScrollWheelSensitivity;
                    zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
                    break;
                default:
                    break;
            }
        }

        private void UpdateCameraTransform()
        {
            var clampedVerticalRotation = Mathf.Clamp(
                value : verticalRotation - tempVerticalRotation, 
                min : verticalRotationMin, 
                max : verticalRotationMax);
            var clampedX = Mathf.Clamp(
                value : currentPosition.x + tempPosition.x, 
                min : initialPosition.x - moveAreaSize, 
                max : initialPosition.x + moveAreaSize);
            var clampedY = Mathf.Clamp(
                value : currentPosition.y + tempPosition.y, 
                min : initialPosition.y - moveAreaSize, 
                max : initialPosition.y + moveAreaSize);

            var currentHorizontalRotation = horizontalRotationDampingSystem.Calculate(horizontalRotation + tempHorizontalRotation);
            var currentVerticalRotation = verticalRotationDampingSystem.Calculate(clampedVerticalRotation);
            var currentZoom = zoomDampingSystem.Calculate(zoom);
            var currentAnchorPosition = moveAreaDampingSystem.Calculate(new Vector2(clampedX, clampedY));

            cameraAnchor.position = new Vector3(currentAnchorPosition.x, 0f, currentAnchorPosition.y);
            cameraAnchor.localEulerAngles = new Vector3(currentVerticalRotation, currentHorizontalRotation, 0f);
            mainCamera.localPosition = new Vector3(0f, 0f, currentZoom);
        }
    }
}