using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private List<Customer> customers;
    public Customer CurrentCustomer { get; private set; }

    private void Start()
    {
        GameManager.Instance.DialogueManager.SpeechBubble.gameObject.SetActive(false);
        GameManager.Instance.DialogueManager.CustomerImage.gameObject.SetActive(false);
        GetNextCustomer();
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
        GameManager.Instance.DialogueManager.StartDialogue(CurrentCustomer);
    }
}
