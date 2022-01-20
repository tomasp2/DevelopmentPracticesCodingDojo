namespace DevelopmentPracticesCodingDojo
{
    public class Basket
    {
        // productName, productCount
        private static Dictionary<string, int> products = new Dictionary<string, int>();

        public static void AddProduct(string productName, Store store)
        {
            if (store.GetProduct(productName) == null)
            {
                Console.WriteLine("Product does not exists");
                return;
            }

            if (products.ContainsKey(productName))
            {
                products[productName]++;
            }
            else
            {
                products.Add(productName, 1);
            }

            Console.WriteLine("Product added to the basket");
        }

        public static void RemoveProduct(string productName)
        {
            if (products.ContainsKey(productName))
            {
                products[productName]--;
            }
            else
            {
                products.Remove(productName);
            }

            Console.WriteLine("Product removed to the basket");
        }

        public static void ListBasket(Store store)
        {
            foreach(var product in products)
            {
                var storedProduct = store.GetProduct(product.Key);
                Console.WriteLine($"Product: {storedProduct.Name} quantity: {product.Value} price: {storedProduct.Price} sum: {product.Value * storedProduct.Price}");
            }
        }

    }
}
