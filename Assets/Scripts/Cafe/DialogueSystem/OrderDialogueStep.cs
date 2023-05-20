using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderDialogueStep : DialogueStep
{
    private Order _order;
    
    public OrderDialogueStep(Order order)
    {
        _order = order;
        StepText = order.orderText;
    }
    


}
