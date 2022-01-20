using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevelopmentPracticesCodingDojo
{
    internal class Program
    {
        private static readonly Store store = new();

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            Start();

            //StartWithDependencyInjection(host.Services);
        }

        #region DI code skeleton

        public static void StartWithDependencyInjection(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            //var fooService = provider.GetRequiredService<IFooService>();

        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args);

            //host.ConfigureServices((_, services) =>
            //    services.AddTransient<IFooService, FooService>());

            return host;
        }

        #endregion

        public static void Start()
        {
            var commands = LoadCommands();
            var entities = LoadEntities();

            do
            {
                var input = Console.ReadLine();

                if (input != null)
                {
                    var parsedInput = input.Split(' ');

                    if (parsedInput.Length > 0)
                    {
                        var command = parsedInput[0];

                        if (commands.Contains(command))
                        {
                            if (command == Commands.End) break;
                            else if (command == Commands.Admin) ProcessAdminCommand(parsedInput);
                            else if (command == Commands.Add) ProcessAddCommand(parsedInput);
                            else if (command == Commands.Delete) ProcessDeleteCommand(parsedInput);
                            else if (command == Commands.Update) ProcessUpdateCommand(parsedInput);
                            else if (command == Commands.List) ProcessListCommand(parsedInput);
                            else if (command == Commands.Pay) ProcessPayCommand(parsedInput);
                            else Console.WriteLine("Incorrect command, try again");
                        }
                        else
                        {
                            Console.WriteLine("Unknown command, try again");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Incorrect input, try again");
                    }
                }

            } while (true);
        }


        public static void ProcessAdminCommand(string[] args)
        {
            if (AdminContext.IsInAdminMode())
            {
                AdminContext.SetAdminMode(false);
                Console.WriteLine("Leaving admin mode");
            }
            else
            {
                Console.WriteLine("Please enter password:");
                var password = Console.ReadLine();

                if (!AdminContext.Authenticate(password))
                {
                    Console.WriteLine("Incorrect password, try again.");
                    Console.WriteLine("Please enter password:");
                    password = Console.ReadLine();

                    if (!AdminContext.Authenticate(password))
                    {
                        Console.WriteLine("Incorrect password, try again.");
                        Console.WriteLine("Please enter password:");
                        password = Console.ReadLine();

                        if (!AdminContext.Authenticate(password))
                        {
                            Console.WriteLine("No more attempts to enter the password.");
                            return;
                        }
                    }
                }

                AdminContext.SetAdminMode(true);
                Console.WriteLine("Entering admin mode");
            }
        }

        public static void ProcessAddCommand(string[] args)
        {
            var entity = args[1];

            if (entity == Entities.Basket)
            {
                if (args.Length < 3) Console.WriteLine("Incorrect number of parameters to add to the basket");

                var productName = args[2];

                Basket.AddProduct(productName, store);

            }

            if (entity == Entities.Product)
            {
                if (args.Length < 5) Console.WriteLine("Incorrect number of parameters to add a product to the store");

                var productName = args[2];
                var price = args[3];
                var tax = args[4];

                store.AddNewProduct(productName, float.Parse(price), int.Parse(tax));
            }

            if (entity == Entities.Discount)
            {
                if (args.Length < 5) Console.WriteLine("Incorrect number of parameters to add a discount to the store");

                var discountName = args[2];
                var productName = args[3];
                var discountValue = args[4];

                store.AddNewDiscount(discountName, productName, int.Parse(discountValue));
            }
        }

        public static void ProcessDeleteCommand(string[] args)
        {
            var entity = args[1];

            if (entity == Entities.Basket)
            {
                if (args.Length < 3) Console.WriteLine("Incorrect number of parameters to delete a product from the basket");

                var productName = args[2];

                Basket.RemoveProduct(productName);
            }

            if (entity == Entities.Product)
            {
                if (args.Length < 3) Console.WriteLine("Incorrect number of parameters to delete a product from the store");

                var productName = args[2];

                store.DeleteProduct(productName);
            }

            if (entity == Entities.Discount)
            {
                if (args.Length < 3) Console.WriteLine("Incorrect number of parameters to delete a discount from the store");

                var discountName = args[2];

                store.DeleteDiscount(discountName);
            }
        }

        public static void ProcessUpdateCommand(string[] args)
        {
            // note: we do not support update yet, skip this command
            throw new NotSupportedException("Update operation is not supported");
        }

        public static void ProcessListCommand(string[] args)
        {
            var entity = args[1];

            if (entity == Entities.Basket)
            {
                Basket.ListBasket(store);
            }

            if (entity == Entities.Product)
            {
                throw new NotImplementedException("List stored products command not implemented yet");
            }

            if (entity == Entities.Discount)
            {
                throw new NotImplementedException("List stored discounts command not implemented yet");
            }

        }

        public static void ProcessPayCommand(string[] args)
        {
            throw new NotImplementedException("Pay command not implemented yet");
        }

        public static List<string> LoadCommands()
        {
            var commands = new List<string>();
            commands.Add(Commands.Admin);
            commands.Add(Commands.Add);
            commands.Add(Commands.Delete);
            commands.Add(Commands.Update);
            commands.Add(Commands.List);
            commands.Add(Commands.Pay);
            commands.Add(Commands.End);

            return commands;
        }

        public static List<string> LoadEntities()
        {
            var entities = new List<string>();
            entities.Add(Entities.Product);
            entities.Add(Entities.Basket);
            entities.Add(Entities.Discount);

            return entities;
        }
    }

    // lets say commands are API request to our e-shop backend
    internal static class Commands
    {
        // type admin to open "admin interface" with correct password "password"
        // type admin in opened "admin interface" to close the admin interface
        public static readonly string Admin = "admin";

        // add product name price tax(%) - admin
        // add discount name productName discountValue(%) - admin
        // add basket productId -> to add a product to the basket
        public static readonly string Add = "add";

        // delete product productName - admin -> if a product is in a basket, remove it as well
        // delete discount discountName - admin -> deleted discount cannot be used
        // delete basket productId
        public static readonly string Delete = "delete";

        // update product productName newPrice newTax(%) - admin
        // update discount discountName newDiscountValue(%) - admin
        public static readonly string Update = "update";

        // list product -> list all products in the store
        // list discount -> list all discounts in the store
        // list basket -> list all products in the basket with count, price with and without tax (prices with discounts if available) and the sum of all the products in basket
        public static readonly string List = "list";

        // pay for the items in basket -> list all the bought items with its prices and discounts, total sum with and without tax, apply discounts
        // n/a in admin mode -> error
        public static readonly string Pay = "pay";

        // to leave the eshop
        public static readonly string End = "end";
    }

    internal static class Entities
    {
        public static readonly string Product = "product";

        public static readonly string Basket = "basket";

        public static readonly string Discount = "discount";
    }
}