using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {

        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded.");
            }
        }


        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
        {
            new()
            {
                UserName = "ify",
                FirstName = "Ifeanyi",
                LastName = "Nwachukwu",
                EmailAddress = "ifenwachukwu7@outlook.com",
                AddressLine = "Lagos",
                Country = "Nigeria",
                TotalPrice = 750,
                State = "LG",
                ZipCode = "001148",

                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "Ifeanyi",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "Ifeanyi",
                LastModifiedDate = new DateTime(),
            }
        };
        }

    }
}
