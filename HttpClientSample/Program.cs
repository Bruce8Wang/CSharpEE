using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public static string Id => null;
    }

    internal static class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        private static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.Name}\tPrice: " + $"{product.Price}\tCategory: {product.Category}");
        }

        private static async Task<Uri> CreateProductAsync(Product product)
        {
            var response = await Client.PostAsJsonAsync("api/products", product);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        private static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            var response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }

            return product;
        }

        private static async Task<Product> UpdateProductAsync(Product product)
        {
            var response = await Client.PutAsJsonAsync($"api/products/{Product.Id}", product);
            response.EnsureSuccessStatusCode();

            product = await response.Content.ReadAsAsync<Product>();
            return product;
        }

        private static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            var response = await Client.DeleteAsync($"api/products/{id}");
            return response.StatusCode;
        }


        private static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            Client.BaseAddress = new Uri("http://localhost:64195/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var product = new Product
                {
                    Name = "Gizmo",
                    Price = 100,
                    Category = "Widgets"
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine($"Created at {url}");

                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                Console.WriteLine("Updating price...");
                product.Price = 80;
                await UpdateProductAsync(product);

                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                var statusCode = await DeleteProductAsync(Product.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int) statusCode})");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}