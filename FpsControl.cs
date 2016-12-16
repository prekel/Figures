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
		private int _limit, _time, _atime, _ptime, _asum, _psum, _fps, _fpssum, _c, _tsum;
		private double _aver, _taver;

		public int FrameLimit { get { return _limit; } set { _limit = value; _time = 1000 / value; } }
		public int TimeLimit { get { return _time; } set { _time = value; _limit = 1000 / value; } }
		public int ActualTime { get { return _atime; } set { _atime = value; } }
		public int PreviousTime { get { return _ptime; } set { _ptime = value; } }
		public int ActualSumTime { get { return _asum; } set { _asum = value; } }
		public int PreviousSumTime { get { return _psum; } set { _psum = value; } }
		public int Frames { get { return _fps; } set { _fps = value; } }
		public int FrameSum { get { return _fpssum; } set { _fpssum = value; } }
		public int Capacity { get { return _c; } set { _c = value; } }
		public double FrameAverage { get { return _aver; } set { _aver = value; } }
		public double TimeAverage { get { return _taver; } set { _taver = value; } }
		public int TimeSum { get { return _tsum; } set { _tsum = value; } }

		Stopwatch Clock = new Stopwatch();
		Queue<int> TimeQueue = new Queue<int>();
		Queue<int> FrameQueue = new Queue<int>();

		public FpsControl(int limit, int capacity)
		{
			FrameLimit = limit;
			for (var i = 0; i < capacity; i++)
			{
				TimeQueue.Enqueue(TimeLimit);
			}
			for (var i = 0; i < capacity; i++)
			{
				FrameQueue.Enqueue(FrameLimit);
			}
			Capacity = capacity;
			FrameSum = FrameLimit * Capacity;
			TimeSum = TimeLimit * Capacity;
			Clock.Start();
		}

		public void SleepPart(double k)
		{
			ActualSumTime = (int)Clock.ElapsedMilliseconds;
			ActualTime = ActualSumTime - PreviousSumTime;
			if (ActualTime < TimeLimit * k)
			{
				try { Thread.Sleep((int)(TimeLimit * k) - ActualTime); }
				catch { /*ignored*/ };
			}
		}

		public void Calc()
		{
			ActualSumTime = (int)Clock.ElapsedMilliseconds;
			ActualTime = ActualSumTime - PreviousSumTime;
			if (ActualTime < TimeLimit)
			{
				Thread.Sleep(TimeLimit - ActualTime);
			}

			ActualSumTime = (int)Clock.ElapsedMilliseconds;
			ActualTime = ActualSumTime - PreviousSumTime;
			if (ActualTime > 0)
				Frames = 1000 / ActualTime;
			else
				Frames = FrameLimit;

			FrameQueue.Enqueue(Frames);
			FrameSum += Frames;
			FrameSum -= FrameQueue.Dequeue();
			FrameAverage = FrameSum / Capacity;

			TimeQueue.Enqueue(ActualTime);
			TimeSum += ActualTime;
			TimeSum -= TimeQueue.Dequeue();
			TimeAverage = TimeSum / Capacity;

			PreviousSumTime = (int)Clock.ElapsedMilliseconds;
		}
	}
}
