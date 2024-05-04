using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour {

    public Texture2D defaultCursor;
    public Texture2D interactableCursor;

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject.CompareTag("Interactable")) {
                Cursor.SetCursor(interactableCursor, Vector2.zero, CursorMode.Auto);
            }
            else {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}
