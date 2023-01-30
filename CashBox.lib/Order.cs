using System;
using System.Collections.Generic;

namespace CashBox.lib;

public partial class Order
{
    public Order(DateTime creationDate, double orderPrice, string client)
    {
        CreationDate = creationDate;
        OrderPrice = orderPrice;
        Client = client;
    }

    public int OrderId { get; set; }

    public DateTime CreationDate { get; set; }

    public double OrderPrice { get; set; }

    public string Client { get; set; } = null!;
}
