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

    private float satisfaction;
    private float satisfactionTimer;
    private bool satisfactionTimerEnabled;

    private void Start()
    {
        
    }

    public void StartSatisfactionTimer()
    {
        satisfactionTimerEnabled = true;
    }

    public void StopSatisfactionTimer()
    {
        satisfactionTimerEnabled = false;
    }

    public void ResetSatisfactionTimer()
    {
        satisfactionTimer = 0;
    }

    private void Update()
    {
        if (satisfactionTimerEnabled && OrderManager.Instance.CurrentOrder != null)
        {
            satisfactionTimer += Time.deltaTime;
            satisfaction = 1 - (satisfactionTimer / OrderManager.Instance.CurrentOrder.TimeToMake);
            OnCustomerSatisfactionChanged?.Invoke(satisfaction);
            if (satisfactionTimer >= OrderManager.Instance.CurrentOrder.TimeToMake)
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
        DayManager.Instance.Enabled = true;
        satisfaction = 1f;
        satisfactionTimer = 0;
        OnCustomerSatisfactionChanged?.Invoke(satisfaction);
        StartCoroutine(GenerateCustomerAfterDelay(2f));
    }
    

    public IEnumerator GenerateCustomerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GenerateCustomer();
    }

    private void GenerateCustomer()
    {
        
        CurrentCustomer = RarityFunctions.GenerateItem(customers);
        OnCustomerGenerated?.Invoke(CurrentCustomer);

    }
}
