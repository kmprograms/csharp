using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace _04_Observer
{
    enum Category
    {
        A, B, C, D
    }

    interface IObservable<T>
    {
        void Subscribe(params T[] observer);
        void Unsubscribe(params T[] observer);
        void Notify();
    }

    interface IObserver<T>
    {
        void Update(T product);
    }

    class Product : IObservable<Customer>
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }

        private decimal discount;
        public decimal Discount
        {            
            get { return discount; }
            set
            {
                // kiedy znizka jest wieksza od poprzedniej
                if (value > discount)
                {
                    Notify();
                }
                discount = value;
            }
        }

        // --------------------------------------------------------------

        private readonly List<Customer> observers = new List<Customer>();

        public void Notify()
        {
            observers.ForEach(observer => observer.Update(this));
        }

        public void Subscribe(params Customer[] newObservers)
        {
            observers.AddRange(newObservers);
        }

        public void Unsubscribe(params Customer[] observersToRemove)
        {
            // podaj minimum dwa inne sposoby na usuniecie z kolekcji observers
            // elementow z observersToRemove
            observers.RemoveAll(observersToRemove.Contains);
        }        
    }

    class Customer : IObserver<Product>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Category> FavouriteCategories { get; set; }

        public void Update(Product product)
        {
            if (FavouriteCategories.Contains(product.Category))
            {
                Console.WriteLine($"{product.Name} with price {product.Price * (1 - product.Discount)} was sent to {Email}");
                // przygotuj kod ktory wysle email
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // customers
            var customers = new List<Customer>()
            {
                new Customer()
                {
                    Name = "JAN",
                    Email = "jan@gmail.com",
                    FavouriteCategories = new List<Category> {Category.A, Category.C}
                },
                new Customer()
                {
                    Name = "ADAM",
                    Email = "adam@gmail.com",
                    FavouriteCategories = new List<Category> {Category.B, Category.D}
                }
            };

            // products
            var products = new List<Product>()
            {
                new Product()
                {
                    Name = "PR1",
                    Category = Category.A,
                    Price = 12.2m,
                    Discount = 0.1m
                },
                new Product()
                {
                    Name = "PR2",
                    Category = Category.B,
                    Price = 15.2m,
                    Discount = 0.11m
                },
                new Product()
                {
                    Name = "PR3",
                    Category = Category.C,
                    Price = 16.2m,
                    Discount = 0.15m
                },
                new Product()
                {
                    Name = "PR4",
                    Category = Category.D,
                    Price = 12.6m,
                    Discount = 0.14m
                }
            };

            // add new observers            
            products.ForEach(product => product.Subscribe(customers.ToArray()));
            
            Console.WriteLine("\nFIRST DISCOUNT CHANGE");
            products.ForEach(product => product.Discount += 0.1m);

            Console.WriteLine("\nSECOND DISCOUNT CHANGE");
            products.ForEach(product => product.Discount -= 0.1m);

            // products.ForEach(product => product.Unsubscribe(customers.ToArray()));
            products.ForEach(product => product.Unsubscribe(customers[0]));

            Console.WriteLine("\nTHIRD DISCOUNT CHANGE");
            products
                .Where(product => product.Category == Category.A)
                .ToList() // jak dokonac zmiany wartości znizki po zakomentowaniu tej linii?
                .ForEach(product => product.Discount += 0.1m);                                                     
        }
    }
}
