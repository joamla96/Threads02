using System;
using Threads02.Exceptions;

namespace Threads02 {
	class Que {
		public int Serving = 0;
		public int InQue = 0;
		public int NextNumber = 0;

		private object lockNewNumber = new object();
		private object lockServeNext = new object();

		public int PullNumber() {
			lock (lockNewNumber) {
				InQue++;
				return ++NextNumber;
			}
		}

		public int serveNext() {
			lock (lockServeNext) {
				if (this.InQue == 0) {
					throw new QueIsEmptyException();
				}
				InQue--;
				return ++Serving;
			}
		}
		
	}
}
