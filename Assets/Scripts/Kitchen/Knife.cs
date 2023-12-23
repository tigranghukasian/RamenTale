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
    private Coroutine _resetCoroutine;
    private bool isDragging;

    public float CutAnimationLength => cutAnimationLength;
    
    public bool IsCutting { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _initialPosition = transform.position;
        _initialParent = transform.parent;
    }
    


    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        if (IsCutting)
        {
            return;
        }

        if (GameManager.Instance.IsTutorialActive && GameSceneManager.Instance.TutorialManager.GetCurrentStepInteractableObject() != gameObject)
        {
            return;
        }
        
        _rectTransform = GetComponent<RectTransform>();
        transform.SetParent(KitchenManager.Instance.DragCanvas.transform);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsCutting || !isDragging)
        {
            return;
        }
        if (GameManager.Instance.IsTutorialActive && GameSceneManager.Instance.TutorialManager.GetCurrentStepInteractableObject() != gameObject)
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
        if (GameManager.Instance.IsTutorialActive && GameSceneManager.Instance.TutorialManager.GetCurrentStepInteractableObject() != gameObject)
        {
            return;
        }
        if (IsCutting)
        {
            _animator.Play("cutting");
            _resetCoroutine = StartCoroutine(ResetAfterDelay(cutAnimationLength));
        }
        else
        {
            ResetKnife();
        }

        isDragging = false;

    }

    public void ResetKnife()
    {
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
        }
        transform.SetParent(_initialParent);
        transform.position = _initialPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        IsCutting = false;
        _animator.Play("Idle");
        isDragging = false;
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetKnife();
    }
}
