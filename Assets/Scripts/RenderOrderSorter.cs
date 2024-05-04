using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrderSorter : MonoBehaviour
{
    public int sortingOrderBase = 5000;
    public int offset;
    public bool isMovingObject = false;
    
    private Renderer myRenderer;

    private void Awake() {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    private void LateUpdate() {
        myRenderer.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
        if (!isMovingObject) {
            Destroy(this);
        }
    }
}
