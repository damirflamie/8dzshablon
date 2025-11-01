using System;

namespace DesignPatternsDemo
{

    public interface IBeverage
    {
        string GetDescription();
        double Cost();
    }

    public class Espresso : IBeverage
    {
        public string GetDescription() => "Espresso";
        public double Cost() => 800;
    }

    public class Tea : IBeverage
    {
        public string GetDescription() => "Tea";
        public double Cost() => 500;
    }

    public class Latte : IBeverage
    {
        public string GetDescription() => "Latte";
        public double Cost() => 1000;
    }

    public class Mocha : IBeverage
    {
        public string GetDescription() => "Mocha";
        public double Cost() => 1200;
    }

    public abstract class BeverageDecorator : IBeverage
    {
        protected IBeverage _beverage;
        public BeverageDecorator(IBeverage beverage) => _beverage = beverage;

        public virtual string GetDescription() => _beverage.GetDescription();
        public virtual double Cost() => _beverage.Cost();
    }

    public class Milk : BeverageDecorator
    {
        public Milk(IBeverage beverage) : base(beverage) { }
        public override string GetDescription() => _beverage.GetDescription() + ", Milk";
        public override double Cost() => _beverage.Cost() + 200;
    }

    public class Sugar : BeverageDecorator
    {
        public Sugar(IBeverage beverage) : base(beverage) { }
        public override string GetDescription() => _beverage.GetDescription() + ", Sugar";
        public override double Cost() => _beverage.Cost() + 100;
    }

    public class WhippedCream : BeverageDecorator
    {
        public WhippedCream(IBeverage beverage) : base(beverage) { }
        public override string GetDescription() => _beverage.GetDescription() + ", Whipped Cream";
        public override double Cost() => _beverage.Cost() + 300;
    }

    public class Caramel : BeverageDecorator
    {
        public Caramel(IBeverage beverage) : base(beverage) { }
        public override string GetDescription() => _beverage.GetDescription() + ", Caramel";
        public override double Cost() => _beverage.Cost() + 250;
    }

    public interface IPaymentProcessor
    {
        void ProcessPayment(double amount);
    }

    public class PayPalPaymentProcessor : IPaymentProcessor
    {
        public void ProcessPayment(double amount)
        {
            Console.WriteLine($"[PayPal] Processing payment of {amount} тг...");
        }
    }

    public class StripePaymentService
    {
        public void MakeTransaction(double totalAmount)
        {
            Console.WriteLine($"[Stripe] Transaction completed: {totalAmount} тг");
        }
    }

    public class StripePaymentAdapter : IPaymentProcessor
    {
        private readonly StripePaymentService _stripeService;
        public StripePaymentAdapter(StripePaymentService stripeService)
        {
            _stripeService = stripeService;
        }

        public void ProcessPayment(double amount)
        {
            _stripeService.MakeTransaction(amount);
        }
    }

    public class QiwiPaymentSystem
    {
        public void Pay(double sum)
        {
            Console.WriteLine($"[Qiwi] Оплата прошла успешно: {sum} тг");
        }
    }

    public class QiwiPaymentAdapter : IPaymentProcessor
    {
        private readonly QiwiPaymentSystem _qiwiSystem;
        public QiwiPaymentAdapter(QiwiPaymentSystem qiwiSystem)
        {
            _qiwiSystem = qiwiSystem;
        }

        public void ProcessPayment(double amount)
        {
            _qiwiSystem.Pay(amount);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Демонстрация структурных паттернов ===");
            Console.WriteLine("1 — Система заказов в кафе (Декоратор)");
            Console.WriteLine("2 — Система оплаты (Адаптер)");
            Console.Write("\nВыберите модуль: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunDecoratorDemo();
                    break;
                case "2":
                    RunAdapterDemo();
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        static void RunDecoratorDemo()
        {
            Console.WriteLine("\n=== Кафе: система заказов (Паттерн Декоратор) ===");

            IBeverage drink1 = new Espresso();
            drink1 = new Milk(drink1);
            drink1 = new Sugar(drink1);
            Console.WriteLine($"{drink1.GetDescription()} -> {drink1.Cost()} тг");

            IBeverage drink2 = new Latte();
            drink2 = new Caramel(new WhippedCream(drink2));
            Console.WriteLine($"{drink2.GetDescription()} -> {drink2.Cost()} тг");

            IBeverage drink3 = new Mocha();
            drink3 = new Milk(new Milk(new Sugar(drink3)));
            Console.WriteLine($"{drink3.GetDescription()} -> {drink3.Cost()} тг");
        }

        static void RunAdapterDemo()
        {
            Console.WriteLine("\n=== Интернет-магазин: система оплаты (Паттерн Адаптер) ===");

            IPaymentProcessor paypal = new PayPalPaymentProcessor();
            IPaymentProcessor stripe = new StripePaymentAdapter(new StripePaymentService());
            IPaymentProcessor qiwi = new QiwiPaymentAdapter(new QiwiPaymentSystem());

            paypal.ProcessPayment(5000);
            stripe.ProcessPayment(7500);
            qiwi.ProcessPayment(3000);
        }
    }
}
