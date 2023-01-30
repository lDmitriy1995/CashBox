using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashBox.lib;

public partial class Product
{
    public Product(string name, int amount, double price, double sum)
    {
        Name = name;
        Amount = amount;
        Price = price;
        Sum = sum;
    }
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Amount { get; set; }

    public double Price { get; set; }

    public double Sum { get; set; }
}
