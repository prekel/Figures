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
		private double _limit, _time, _atime, _ptime, _asum, _psum, _fps, _fpssum, _c, _tsum;
		private double _aver, _taver;

		public double FrameLimit { get { return _limit; } set { _limit = value; _time = 1 / value; } }
		public double TimeLimit { get { return _time; } set { _time = value; _limit = 1 / value; } }
		public double ActualTime { get { return _atime; } set { _atime = value; } }
		public double PreviousTime { get { return _ptime; } set { _ptime = value; } }
		public double ActualSumTime { get { return _asum; } set { _asum = value; } }
		public double PreviousSumTime { get { return _psum; } set { _psum = value; } }
		public double Frames { get { return _fps; } set { _fps = value; } }
		public double FrameSum { get { return _fpssum; } set { _fpssum = value; } }
		public double Capacity { get { return _c; } set { _c = value; } }
		public double FrameAverage { get { return _aver; } set { _aver = value; } }
		public double TimeAverage { get { return _taver; } set { _taver = value; } }
		public double TimeSum { get { return _tsum; } set { _tsum = value; } }

		Stopwatch Clock = new Stopwatch();
		Queue<double> TimeQueue = new Queue<double>();
		Queue<double> FrameQueue = new Queue<double>();
		private double f = Stopwatch.Frequency;

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
			ActualSumTime = Clock.ElapsedTicks / f;
			ActualTime = ActualSumTime - PreviousSumTime;
			if (ActualTime < TimeLimit * k)
			{
				try { Thread.Sleep((int)((TimeLimit * k - ActualTime) * 1000) + 1); }
				catch { /*ignored*/ };
			}
		}

		public void Calc()
		{
			//ActualSumTime = (int)Clock.ElapsedMilliseconds;
			ActualSumTime =  Clock.ElapsedTicks / f;
			ActualTime = ActualSumTime - PreviousSumTime;
			if (ActualTime < TimeLimit)
			{
				Thread.Sleep((int)((TimeLimit - ActualTime) * 1000) + 1);
			}

			ActualSumTime = Clock.ElapsedTicks / f;
			ActualTime = ActualSumTime - PreviousSumTime;
			if (ActualTime > 0)
				Frames = 1 / ActualTime;
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

			PreviousSumTime =  Clock.ElapsedTicks / f;
		}
	}
}
