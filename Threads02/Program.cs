using System;
using System.Threading;

using Threads02.Exceptions;

namespace Threads02 {
	class Program {
		public static Que que = new Que();

		private static Random rnd = new Random();

		private const int ServeTime = 1000;
		private const int GrowTime = 500;
		private const int QueEmptyWait = 2500;

		private bool ShopsOpen = true;
		static void Main(string[] args) {
			Program a = new Program();
			a.Run();
		}

		public void Run() {
			Thread Customers = new Thread(NewCustomer);
			Thread Clerk0 = new Thread(ServeCustomer);
			Thread Clerk1 = new Thread(ServeCustomer);

			Console.WriteLine("Click enter to close up shop...");

			Customers.Start();
			Clerk0.Start();
			Clerk1.Start();

			Console.ReadLine();
			ShopsOpen = false;
			Console.WriteLine("Closing shop, no new numbers...");

			Customers.Join();
			Clerk0.Join();
			Clerk1.Join();

			Console.WriteLine("No more customers to be served...");
			Console.ReadKey();
		}
		
		private void ConsoleUI() {

		}

		private void NewCustomer() {
			while (ShopsOpen) {
				int MyNumber = que.PullNumber();
				Console.WriteLine("The next number to be picked: " + MyNumber);

				Thread.Sleep(GrowTime);
			}
		}

		private void ServeCustomer() {
			while (ShopsOpen || que.InQue > 0) {
				try {
					int Serving = que.serveNext();
					Console.WriteLine("Now handling: " + Serving);
					Thread.Sleep(ServeTime);
				} catch (QueIsEmptyException) {
					Console.WriteLine("Que is empty...");
					Thread.Sleep(QueEmptyWait);
				}
			}
		}
	}
}
