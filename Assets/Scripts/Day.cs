using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Day
{
    [SerializeField] private List<Visit> visits;
    public List<Visit> Visits => visits;

    public Day()
    {
        visits = new List<Visit>();
    }

}

[System.Serializable]
public class Visit
{
    
    public Customer customer;
    public Dialogue dialogue;
    public Order order;
}
