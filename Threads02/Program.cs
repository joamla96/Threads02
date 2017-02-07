using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Threads02.Exceptions;

namespace Threads02 {
	class Program {
		public static Que que = new Que();

		private static Random rnd = new Random();

		private int ServeTime = rnd.Next(1000, 1500);
		private int GrowTime = rnd.Next(500, 750);
		private int QueEmptyWait = 1000;

		private bool ShopsOpen = true;

		private Dictionary<int, int> Clerks = new Dictionary<int, int>();

		static void Main(string[] args) {
			Program a = new Program();
			a.Run();
		}

		public void Run() {
			Thread Customers = new Thread(NewCustomer);
			Thread Clerk0 = new Thread(ServeCustomer);
			Thread Clerk1 = new Thread(ServeCustomer);

			Thread UI = new Thread(ConsoleUI);

			UI.Start();
			
			Customers.Start();
			Clerk0.Start();
			Clerk1.Start();

			Console.ReadLine();
			ShopsOpen = false;
			
			Customers.Join();
			Clerk0.Join();
			Clerk1.Join();

			UI.Abort();

			Console.WriteLine("No more customers to be served...");
			Console.ReadKey();
		}
		
		public void ConsoleUI() {
			while(true) {
				Console.Clear();
				Console.WriteLine("Click enter to close up shop...\n");

				Console.WriteLine("Currently in Que: " + que.InQue);
				Console.WriteLine("Next Number Available: " + que.NextNumber);

				foreach(KeyValuePair<int, int> Clerk in Clerks) {
					Console.WriteLine("Clerk "+ Clerk.Key +" is currently serving " + Clerk.Value);
				}

				if(!ShopsOpen) {
					Console.WriteLine("\nClosing shop, no new numbers...");
				}

				Thread.Sleep(125);
			}
		}

		private void NewCustomer() {
			while (ShopsOpen) {
				int MyNumber = que.PullNumber();
				//Console.WriteLine("The next number to be picked: " + MyNumber);

				Thread.Sleep(GrowTime);
			}
		}

		private void ServeCustomer() {
			int ClerkID = RegisterClerk();
			while (ShopsOpen || que.InQue > 0) {
				try {
					int Serving = que.serveNext();
					Clerks[ClerkID] = Serving;
					//Console.WriteLine("Now handling: " + Serving);
					Thread.Sleep(ServeTime);
				} catch (QueIsEmptyException) {
					Clerks[ClerkID] = 0;
					//Console.WriteLine("Que is empty...");
					Thread.Sleep(QueEmptyWait);
				}
			}
		}

		private int RegisterClerk() {
			int newID;
			if (Clerks.Count == 0) {
				newID = 0;
			} else {
				newID = Clerks.Keys.Last() + 1;
			}
						
			Clerks.Add(newID, 0);
			return newID;
		}
	}
}
