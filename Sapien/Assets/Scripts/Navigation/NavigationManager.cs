using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class NavigationManager : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private Vector3 targetPosition;
    private RectTransform pointer;
    public Transform target;
    public float border = 100f;
    public string targetName;

    private void Awake()
    {
        target = GameObject.Find(targetName).GetComponent<Transform>();
        pointer = GameObject.Find("NavigatorPointer").GetComponent<RectTransform>();
    }

    void Update()
    {
        targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
        Vector3 TargetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = TargetPositionScreenPoint.x <= 0 || TargetPositionScreenPoint.x >= Screen.width || TargetPositionScreenPoint.y <= 0 || TargetPositionScreenPoint.y >= Screen.height || targetPosition.z <= Camera.main.transform.position.z;

        Debug.Log(isOffScreen + " " + TargetPositionScreenPoint);

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = TargetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= border)
            {
                cappedTargetScreenPosition.x = border;
                Debug.Log("Right");
            }
            else if (cappedTargetScreenPosition.x >= Screen.width - border)
            {
                cappedTargetScreenPosition.x = Screen.width - border;
                Debug.Log("Left");
            }
            if (cappedTargetScreenPosition.y <= border)
            {
                cappedTargetScreenPosition.y = border;
                Debug.Log("Down");
            }
            else if (cappedTargetScreenPosition.y >= Screen.height - border)
            {
                cappedTargetScreenPosition.y = Screen.height - border;
                Debug.Log("Top");
            }

            Vector3 pointerWorldPosition = camera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointer.position = pointerWorldPosition;
            pointer.position = cappedTargetScreenPosition;
            pointer.localPosition = new Vector3(-pointer.localPosition.x, -pointer.localPosition.y, 0f);

        }
        else if (!isOffScreen)
        {
            GameObject WorldObject = target.gameObject;

            RectTransform UI_Element = pointer;

            RectTransform CanvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();


            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(WorldObject.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            UI_Element.anchoredPosition = WorldObject_ScreenPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == targetName)
                { print(targetName + " is clicked by mouse"); }
                TargetChange("SM_Veh_Car_Taxi_01");
            }
        }
    }

    public void TargetChange(string name)
    {
        targetName = name;
        target = GameObject.Find(name).GetComponent<Transform>();
        Debug.Log("Click");
    }
}
