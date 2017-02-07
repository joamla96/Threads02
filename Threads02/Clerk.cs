using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Threads02.Exceptions;

namespace Threads02 {
	class Clerk {
		public static Dictionary<int, Clerk> Clerks = new Dictionary<int, Clerk>();

		private Thread MyThead;
		private bool working = true;

		public int ID { get; set; }
		public int Serving { get; set; }

		private Random rnd = new Random();

		private const int ServeTimeCeil = 750;
		private const int ServeTimeFloor = 500;

		public Clerk(int newID) {
			this.ID = newID;

			Clerks.Add(this.ID, this);
		}

		public void GoWork() {
			MyThead = new Thread(Work);
			MyThead.Start();
		}

		public void GoHome() {
			working = false;
			MyThead.Join();
			Clerks[this.ID].Serving = -1;
		}
		private void Work() {
			while (working) {
				try {
					this.Serving = Program.que.serveNext();
					
					Thread.Sleep(rnd.Next(ServeTimeFloor, ServeTimeCeil));
				} catch (QueIsEmptyException) {
					this.Serving = 0;
					Thread.Sleep(Program.QueEmptyWait);
				}
			}
		}

		public static void CallIn() {
			int newID;
			if (Clerks.Count == 0) {
				newID = 0;
			} else {
				newID = Clerks[Clerks.Count - 1].ID + 1;
			}
			
			Clerk thisClerk = new Clerk(newID);
			thisClerk.GoWork();
		}

	}
}
