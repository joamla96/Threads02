using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Threads02.Exceptions;

namespace Threads02 {
	class Program {
		public static Que que = new Que();

		private static Random rnd = new Random();

		public const int GrowTimeCeil = 750;
		public const int GrowTimeFloor = 500;

		public static int QueEmptyWait = 1000;

		public static bool ShopsOpen = true;

		static void Main(string[] args) {
			Program a = new Program();
			a.Run();
		}

		public void Run() {
			Thread Customers = new Thread(NewCustomer);
			Thread Manager = new Thread(WatchQue);

			Thread UI = new Thread(ConsoleUI);

			UI.Start();
			
			Customers.Start();
			Manager.Start();

			Console.ReadLine();
			ShopsOpen = false;
			
			Customers.Join();
			Manager.Join();

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

				foreach(KeyValuePair<int, Clerk> Clerk in Clerk.Clerks) {
					string Serving;
					if(Clerk.Value.Serving == 0) {
						Serving = "waiting...";
					} else if(Clerk.Value.Serving < 0) {
						Serving = "home";
					}
					else {
						Serving = "serving " + Clerk.Value.Serving;
					}
					Console.WriteLine("Clerk "+ Clerk.Key +" is currently " + Serving);
				}

				if(!ShopsOpen) {
					Console.WriteLine("\nClosing shop, no new numbers...");
				}

				Thread.Sleep(125);
			}
		}

		private void WatchQue() {
			Clerk.CallIn();
			Clerk.CallIn();

			while(ShopsOpen || que.InQue > 0) {
				if(que.InQue > 5) {
					// When que is too big add clerks...
				}  else {
					// else send them home...
				}
			}
		}

		private void NewCustomer() {
			while (ShopsOpen) {
				int MyNumber = que.PullNumber();
				Thread.Sleep(rnd.Next(GrowTimeFloor, GrowTimeCeil));
			}
		}

	}
}
