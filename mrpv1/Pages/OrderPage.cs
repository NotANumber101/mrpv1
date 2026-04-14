using System;
using Spectre;
using Spectre.Console;
using mrpv1.Helpers;
using mrpv1.Queries;
using Npgsql;
using mrpv1.Controllers;
using mrpv1.Models;

namespace mrpv1.Pages;

public class OrderPage() : Page

{
    // readonly OrderController orderController = new();
    public async Task Display()
    {
        await DisplayOrders();
    }
    public async Task DisplayOrders()
    {
        Console.WriteLine("not implemented");
    }
    public async Task CreateOrder()
    {
        throw new NotImplementedException();
    }
}