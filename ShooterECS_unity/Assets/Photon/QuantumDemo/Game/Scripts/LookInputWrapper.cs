using System.Linq;
using UnityEngine;

public class LookInputWrapper
{
    public Vector2 InputDelta { get; private set; }

    private Vector2 lastMousePosition = Vector2.zero;

    private Camera _camera;
    public Camera Camera => _camera ??= Object.FindObjectsOfType<Camera>().First(it => it.gameObject.activeSelf);

    public void UpdateInput()
    {
#if UNITY_EDITOR
        HandleInput(Input.mousePosition, Input.GetMouseButtonDown(0), Input.GetMouseButton(0));
#elif UNITY_ANDROID || UNITY_IOS
        foreach (var touch in Input.touches)
        {
            HandleInput(touch.position, touch.phase == TouchPhase.Began, touch.phase == TouchPhase.Moved);
        }
#endif
    }

    private void HandleInput(Vector2 position, bool isClick, bool isMove)
    {
        if (position.x < Screen.width / 2)
        {
            InputDelta = Vector2.zero;
            return;
        }
        
        InputDelta = Vector2.zero;
        Vector2 deltaMove = Vector2.zero;

        if (isClick)
        {
            lastMousePosition = Camera.ScreenToViewportPoint(position);
            lastMousePosition -= new Vector2(0.5f, 0.5f);
        }

        if (isMove)
        {
            deltaMove = (Vector2) Camera.ScreenToViewportPoint(position) - lastMousePosition;
            deltaMove -= new Vector2(0.5f, 0.5f);

            InputDelta = deltaMove;

            lastMousePosition = Camera.ScreenToViewportPoint(position);
            lastMousePosition -= new Vector2(0.5f, 0.5f);
        }
    }
}