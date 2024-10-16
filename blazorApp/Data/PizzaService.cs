namespace BlazingPizza.Data;

public class PizzaService
{
    public Task<Pizza[]> GetPizzasAsync()
    {
    // Call your data access technology here
    // Simulate sending an array of pizzas
    var pizzas = new[]
    {
        new Pizza { PizzaId = 1, Name = "Margherita", Description = "Classic cheese and tomato", Price = 8.99m, Vegetarian = true, Vegan = false },
        new Pizza { PizzaId = 2, Name = "Pepperoni", Description = "Pepperoni and cheese", Price = 9.99m, Vegetarian = false, Vegan = false },
        new Pizza { PizzaId = 3, Name = "Hawaiian", Description = "Ham and pineapple", Price = 10.99m, Vegetarian = false, Vegan = false }
    };

    return Task.FromResult(pizzas);

    }
}