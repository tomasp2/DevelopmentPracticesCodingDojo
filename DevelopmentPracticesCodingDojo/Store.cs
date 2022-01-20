namespace DevelopmentPracticesCodingDojo
{
    public class Store
    {
        public class Product
        {
            public string Name { get; private set; }
            public float Price { get; private set; }
            public int Tax { get; private set; }

            public Product(string name, float price, int tax)
            {
                Name = name;
                Price = price;
                Tax = tax;
            }
        }

        public class Discount
        {
            public string Name { get; private set; }
            public int Value { get; private set; }

            public Discount(string name, int value)
            {
                Name = name;
                Value = value;
            }
        }

        private Dictionary<string, Product> products;
        private Dictionary<string, Discount> discounts;
        // productName, DiscountName -> each product can have only 1 discount
        private Dictionary<string, string> bindings;

        public Store()
        {
            products = new Dictionary<string, Product>();
            discounts = new Dictionary<string, Discount>();
            bindings = new Dictionary<string, string>();
        }

        public Product? GetProduct(string name)
        {
            try
            {
                return products[name];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Discount? GetProductDiscount(string name)
        {
            try
            {
                return discounts[bindings[name]];
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        public void AddNewProduct(string name, float price, int tax)
        {
            if (AdminContext.IsInAdminMode())
            {
                if (!products.ContainsKey(name))
                {
                    products.Add(name, new Product(name, price, tax));

                    Console.WriteLine("Product added");
                }
            }
        }

        public void DeleteProduct(string name)
        {
            if (AdminContext.IsInAdminMode())
            {
                if (products.ContainsKey(name))
                {
                    products.Remove(name);
                    bindings.Remove(name);

                    Console.WriteLine("Product deleted");
                }
            }
        }

        public void AddNewDiscount(string name, string productName, int value)
        {
            if (AdminContext.IsInAdminMode())
            {
                if (!discounts.ContainsKey(name))
                {
                    discounts.Add(name, new Discount(name, value));
                    bindings.Add(productName, name);

                    Console.WriteLine("Discount added");
                }
            }
        }

        public void DeleteDiscount(string name)
        {
            if (AdminContext.IsInAdminMode())
            {
                if (discounts.ContainsKey(name))
                {
                    discounts.Remove(name);

                    var removeBindings = bindings.Where(x => x.Value == name).ToList();
                    foreach (var binding in removeBindings)
                    {
                        bindings.Remove(binding.Key);
                    }

                    Console.WriteLine("Discount deleted");
                }
            }
        }
    }
}
