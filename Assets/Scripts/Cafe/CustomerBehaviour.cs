using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomerBehaviour : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image customerImage;
    [SerializeField] private Image mouthImage;
    [SerializeField] private List<Sprite> mouthSpriteSheet;

    private List<Sprite> _blinkSpriteSheet;

    private int currentBlinkingFrame;
    private int currentMouthFrame;
    private float blinkingTimer;
    private float blinkingFrameRate = 0.05f;
    private float mouthTimer;
    private float mouthFrameRate = 0.2f;
    
    private float blinkAnimateTimer = 0f; 
    private float blinkInterval = 5f;
    private bool isAnimatingBlinking = false;
    private bool isAnimatingMouth = false;
    

    private void Update()
    {
        BlinkingAnimatorLogic();
        MouthAnimatorLogic();
    }

    private void BlinkingAnimatorLogic()
    {
        blinkAnimateTimer += Time.deltaTime;
        
        if (blinkAnimateTimer >= blinkInterval && !isAnimatingBlinking)
        {
            isAnimatingBlinking = true; 
            isAnimatingBlinking = true; 
            blinkAnimateTimer = 0f; 
        }
        
        if (isAnimatingBlinking)
        {
            blinkingTimer += Time.deltaTime;

            if (blinkingTimer >= blinkingFrameRate)
            {
                blinkingTimer -= blinkingFrameRate;
                currentBlinkingFrame = (currentBlinkingFrame + 1) % _blinkSpriteSheet.Count;
                customerImage.sprite = _blinkSpriteSheet[currentBlinkingFrame];
                if (currentBlinkingFrame == 0)
                {
                    isAnimatingBlinking = false; 
                }
            }
        }
    }

    private void MouthAnimatorLogic()
    {
        if (isAnimatingMouth)
        {
            mouthTimer += Time.deltaTime;

            if (mouthTimer >= mouthFrameRate)
            {
                mouthTimer -= mouthFrameRate;
                currentMouthFrame = (currentMouthFrame + 1) % mouthSpriteSheet.Count;
                mouthImage.sprite = mouthSpriteSheet[currentMouthFrame];
                if (currentMouthFrame == 0)
                {
                    isAnimatingMouth = false; 
                }
            }
        }
    }

    public void SetMouthAnimate()
    {
        isAnimatingMouth = true;
        currentMouthFrame = 0;
    }
    
    public void SetSpriteSheet(List<Sprite> blinkSpriteSheet)
    {
        isAnimatingBlinking = false;
        blinkAnimateTimer = 0;
        _blinkSpriteSheet = blinkSpriteSheet;
        customerImage.sprite = _blinkSpriteSheet[0];
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped.TryGetComponent(out Dish dish))
        {
            OrderManager.Instance.ServeDish(dish);
            
            Destroy(dish.gameObject);
        }
    }
}
