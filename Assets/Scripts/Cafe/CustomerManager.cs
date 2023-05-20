using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<Customer> customers;
    public Customer CurrentCustomer { get; private set; }

    public Action<Customer> OnCustomerGenerated;

    private void Start()
    {
        
    }

    public void GetNextCustomer()
    {
        StartCoroutine(GenerateCustomerAfterDelay(1f));
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
