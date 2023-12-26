using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningUIManager : MonoBehaviour
{

    
    [SerializeField] private GameObject openSignTextOpen;
    [SerializeField] private GameObject openSignTextClosed;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnOpenSignTapped()
    {
        animator.Play("OpeningSignFlip");
        StartCoroutine(CloseViewAfterDelay());
    }

    IEnumerator ChangeSignAfterDelay()
    {
        yield return null;
    }

    IEnumerator CloseViewAfterDelay()
    {
        yield return new WaitForSeconds(AnimationConstants.CLOSE_OPENING_VIEW_DURATION);
        gameObject.SetActive(false);
        GameSceneManager.Instance.StartDay();
    }
}
