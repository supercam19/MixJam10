using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteractable : MonoBehaviour {
    private Texture2D defaultCursor;
    public Texture2D hoverCursor;

    void Start() {
        defaultCursor = Resources.Load<Texture2D>("cursor_default");
    }
    void OnMouseOver() {
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit() {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
