using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knife : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;
    private Transform _initialParent;
    [SerializeField] private Transform dragPoint;
    [SerializeField] private float cutAnimationLength;
    private Animator _animator;

    public float CutAnimationLength => cutAnimationLength;
    
    public bool IsCutting { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsCutting)
        {
            return;
        }
        _initialParent = transform.parent;
        _initialPosition = transform.position;
        _rectTransform = GetComponent<RectTransform>();
        transform.SetParent(KitchenManager.Instance.DragCanvas.transform);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsCutting)
        {
            return;
        }
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                KitchenManager.Instance.DragCanvas.GetComponent<RectTransform>(), 
                eventData.position, 
                eventData.pressEventCamera, 
                out position))
        {
            // Calculate the difference between dragPoint and the center of your GameObject
            Vector2 difference = dragPoint.localPosition;

            // Adjust position according to difference
            position -= difference;
            _rectTransform.anchoredPosition = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsCutting)
        {
            _animator.Play("cutting");
            StartCoroutine(ResetAfterDelay(cutAnimationLength));
        }
        else
        {
            ResetKnife();
        }
        
    }

    private void ResetKnife()
    {
        transform.SetParent(_initialParent);
        transform.position = _initialPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        IsCutting = false;
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetKnife();
    }
}
