using Threads02.Exceptions;

namespace Threads02 {
	class Que {
		public int Serving = 0;
		public int InQue = 0;
		public int NextNumber = 0;

		public int PullNumber() {
			InQue++;
			return ++NextNumber;
		}

		public int serveNext() {
			if(this.InQue == 0) {
				throw new QueIsEmptyException();
			}
			InQue--;
			return ++Serving;
		}
		
	}
}
