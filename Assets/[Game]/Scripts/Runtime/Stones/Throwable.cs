using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Vector3 throwVector;
    private Rigidbody _rb;
    private LineRenderer _lineRenderer;
    [SerializeField] private float maxForce; 
    [SerializeField] public float minForce;  
    [SerializeField] public float forceMultiplier; 

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default")); 
        //Renkleri daha belirgin hale getirebilirsin yada direk ok koy
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
    }

    void OnMouseDrag()
    {
        ThrowVector();
        ArrowSet();
    }

    void ThrowVector()
    {
        // fare pozisyonuna göre throw linerendererın iyileştirmeye ihtiyacı var
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z; 
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 distance = worldMousePos - transform.position;
        float distanceMagnitude = distance.magnitude; 
        
        float clampedForce = Mathf.Clamp(distanceMagnitude * forceMultiplier, minForce, maxForce);
        throwVector = -distance.normalized * clampedForce; 
    }

    void ArrowSet()
    {
        _lineRenderer.positionCount = 2;
        
        Vector3 startPoint = transform.position + transform.forward * 0.5f; 
        Vector3 endPoint = startPoint + throwVector.normalized * (throwVector.magnitude / maxForce * 2); 
        // Linerenderer offset ayarları şuan içn 0/1 arasında tut
        _lineRenderer.SetPosition(0, startPoint);
        _lineRenderer.SetPosition(1, endPoint);
        _lineRenderer.enabled = true;
    }

    void OnMouseUp()
    {
        RemoveArrow();
        Throw();
    }

    void RemoveArrow()
    {
        _lineRenderer.enabled = false;
    }

    public void Throw()
    {
        _rb.AddForce(throwVector, ForceMode.Impulse);
    }
}
