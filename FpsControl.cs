using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Figures
{
	public class FpsControl
	{
		private int _limit, _time, _atime, _ptime, _asum, _psum, _fps, _fpssum, _c;
		private double _aver;

		public int FrameLimit { get { return _limit; } set { _limit = value; _time = 1000 / value; } }
		public int TimeLimit { get { return _time; } set { _time = value; _limit = 1000 / value; } }
		public int Actual { get { return _atime; } set { _atime = value; } }
		public int Previous { get { return _ptime; } set { _ptime = value; } }
		public int ActualSum { get { return _asum; } set { _asum = value; } }
		public int PreviousSum { get { return _psum; } set { _psum = value; } }
		public int Fps { get { return _fps; } set { _fps = value; } }
		public int FpsSum { get { return _fpssum; } set { _fpssum = value; } }
		public int Capacity { get { return _c; } set { _c = value; } }
		public double FpsAverage { get { return _aver; } set { _aver = value; } }



		Stopwatch Clock = new Stopwatch();
		Queue<int> FpsQueue = new Queue<int>();

		public FpsControl(int limit, int capacity)
		{
			FrameLimit = limit;
			for (var i = 0; i < capacity; i++)
			{
				FpsQueue.Enqueue(TimeLimit);
			}
			Capacity = capacity;
			FpsSum = TimeLimit * Capacity;
			Clock.Start();
		}

		public void SleepHalf()
		{
			ActualSum = (int)Clock.ElapsedMilliseconds;
			Actual = ActualSum - PreviousSum;
			if (Actual < TimeLimit / 5 * 4)
			{
				try { Thread.Sleep(TimeLimit / 5 * 4 - Actual); }
				catch { /*ignored*/ };
			}
		}

		public void Calc()
		{
			ActualSum = (int)Clock.ElapsedMilliseconds;
			Actual = ActualSum - PreviousSum;
			if (Actual < TimeLimit)
			{
				Thread.Sleep(TimeLimit - Actual);
				//try {  }
				//catch { /*ignored*/ };
			}

			ActualSum = (int)Clock.ElapsedMilliseconds;
			Actual = ActualSum - PreviousSum;
			if (Actual > 0)
				Fps = 1000 / Actual;
			else
				Fps = 1000;
			FpsQueue.Enqueue(Fps);
			FpsSum += Fps;
			FpsSum -= FpsQueue.Dequeue();
			FpsAverage = FpsSum / Capacity;

			PreviousSum = (int)Clock.ElapsedMilliseconds;
			//Debug.WriteLine("");
			//Debug.Write(Actual);
		}
	}
}
