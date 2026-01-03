using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Enums;

public enum OrderStatus
{
    Submitted = 1,      
    StockReserved = 2,  
    PaymentFailed = 3,  
    PaymentCompleted = 4, 
    Completed = 5,      
    Canceled = 6,       
    Failed = 7          
}