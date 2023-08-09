using System.Linq;
using UnityEngine;

public class LookInputWrapper
{
    public Vector2 InputDelta { get; private set; }
    public Vector2 InputDeltaNormalized { get; private set; }

    private Vector2 lastMousePosition = Vector2.zero;

    private Camera _camera;
    public Camera Camera => _camera ??= Object.FindObjectsOfType<Camera>().First(it => it.gameObject.activeSelf);

    public void UpdateInput()
    {
        InputDelta = Vector2.zero;
        InputDeltaNormalized = Vector2.zero;
        Vector2 deltaMove = Vector2.zero;

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Camera.ScreenToViewportPoint(Input.mousePosition);
            lastMousePosition -= new Vector2(0.5f, 0.5f);
        }

        if (Input.GetMouseButton(0))
        {
            deltaMove = (Vector2) Camera.ScreenToViewportPoint(Input.mousePosition) - lastMousePosition;
            deltaMove -= new Vector2(0.5f, 0.5f);

            InputDelta = deltaMove;
            InputDeltaNormalized = deltaMove.normalized;

            lastMousePosition = Camera.ScreenToViewportPoint(Input.mousePosition);
            lastMousePosition -= new Vector2(0.5f, 0.5f);
        }
    }
}