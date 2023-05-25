using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<Customer> customers;
    public Customer CurrentCustomer { get; private set; }

    public Action<Customer> OnCustomerGenerated;

    public Action<float> OnCustomerSatisfactionChanged;

    private float _satisfaction;
    private float _satisfactionTimer;
    private bool _satisfactionTimerEnabled;

    private int _customerIndex;

    private void Start()
    {
        
    }

    public void StartSatisfactionTimer()
    {
        _satisfactionTimerEnabled = true;
    }

    public void StopSatisfactionTimer()
    {
        _satisfactionTimerEnabled = false;
    }

    public void ResetSatisfactionTimer()
    {
        _satisfactionTimer = 0;
    }

    private void Update()
    {
        if (_satisfactionTimerEnabled && OrderManager.Instance.CurrentOrder != null)
        {
            _satisfactionTimer += Time.deltaTime;
            _satisfaction = 1 - (_satisfactionTimer / OrderManager.Instance.CurrentOrder.TimeToMake);
            OnCustomerSatisfactionChanged?.Invoke(_satisfaction);
            if (_satisfactionTimer >= OrderManager.Instance.CurrentOrder.TimeToMake)
            {
                GameSceneManager.Instance.OpenCafe();
                DepartCustomer();
                GetNextCustomer();
            }
        }
       

    }


    public void DepartCustomer()
    {
        GameSceneManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
        GameSceneManager.Instance.DialogueManager.CustomerImage.gameObject.SetActive(false);
    }

    public void GetNextCustomer()
    {
        DayCycleManager.Instance.Enabled = true;
        _satisfaction = 1f;
        _satisfactionTimer = 0;
        OnCustomerSatisfactionChanged?.Invoke(_satisfaction);
        StartCoroutine(CreateCustomerAfterDelay(2f));
    }
    

    public IEnumerator CreateCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateCustomer();
    }

    private void CreateCustomer()
    {
        if (Utilities.IsIndexValid(GameManager.Instance.CurrentDay().Visits, _customerIndex))
        {
            Visit currentVisit = GameManager.Instance.CurrentDay().Visits[_customerIndex];
            
            if (currentVisit.customer != null)
            {
                CurrentCustomer = Instantiate(currentVisit.customer);
                CurrentCustomer.Order = currentVisit.order;
                CurrentCustomer.Dialogue = currentVisit.dialogue;
                OnCustomerGenerated?.Invoke(CurrentCustomer);
                _customerIndex++;
                return;
            }
        }
        
        
        CurrentCustomer = Instantiate(RarityFunctions.GenerateItem(customers));
        OnCustomerGenerated?.Invoke(CurrentCustomer);
        _customerIndex++;

    }
}
