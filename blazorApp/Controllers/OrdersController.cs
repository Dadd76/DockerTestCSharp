﻿namespace blazorApp;

using BlazingPizza;
using BlazingPizza.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("orders")]
[ApiController]
public class OrdersController : Controller
{
    private readonly PizzaStoreContext _db;

    public OrdersController(PizzaStoreContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
    {
        var orders = await _db.Orders
 	    .Include(o => o.Pizzas).ThenInclude(p => p.Special)
 	    .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
 	    .OrderByDescending(o => o.CreatedTime)
 	    .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderWithStatus>> GetOrderWithStatus(int orderId)
        {
            var order = await _db.Orders
                    .Where(o => o.OrderId == orderId)
                    // .Where(o => o.UserId == PizzaApiExtensions.GetUserId(HttpContext))
                    //.Include(o => o.DeliveryLocation)
                    .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                    .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                    .SingleOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return OrderWithStatus.FromOrder(order);
        }


    [HttpPost]
    public async Task<ActionResult<int>> PlaceOrder([FromBody] Order order)
    {
         Console.WriteLine("PlaceOrder method invoked"); 
        order.CreatedTime = DateTime.Now;

        // Enforce existence of Pizza.SpecialId and Topping.ToppingId
        // in the database - prevent the submitter from making up
        // new specials and toppings
        foreach (var pizza in order.Pizzas)
        {
            pizza.SpecialId = pizza.Special.Id;
            pizza.Special = null;
        }

        _db.Orders.Attach(order);
        
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"An error occurred while saving the order: {ex.Message}");
            return StatusCode(500, "An error occurred while saving the order.");
        }

        return order.OrderId;
    }


    [HttpPost("test")]
    public IActionResult TestPost([FromBody] int value)
    {
        // Juste pour vérifier que la valeur a bien été reçue
        Console.WriteLine($"Received integer: {value}");

        // Vous pouvez retourner la valeur pour la tester
        return Ok(value);
}
}