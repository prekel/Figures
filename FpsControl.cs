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
				FpsQueue.Enqueue(limit);
			}
			Capacity = capacity;
			Clock.Start();
		}

		public void SleepHalf()
		{

			if (Clock.ElapsedMilliseconds < FrameLimit / 2)
			{
				try { Thread.Sleep(FrameLimit / 2 - (int)clock.ElapsedMilliseconds); }
				catch { /*ignored*/ };
			}
		}

		public void Calc()
		{
			ActualSum = (int)Clock.ElapsedMilliseconds;
			Actual = ActualSum - PreviousSum;
			PreviousSum = ActualSum;
			if (Actual < TimeLimit)
			{
				try { Thread.Sleep(TimeLimit - Actual); }
				catch { /*ignored*/ };
			}

			Actual = (int)Clock.ElapsedMilliseconds;
			try
			{
				Fps = 1000 / Actual;
			}
			catch
			{
				Fps = 1000;
			}
			FpsQueue.Enqueue(Fps);
			FpsSum += Fps;
			FpsSum -= FpsQueue.Dequeue();
			FpsAverage = FpsSum / (double)Capacity;
		}
	}
}
