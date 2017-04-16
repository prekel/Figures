using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGeometry.Core;

namespace Figures.Core
{
	public class CircleBoby : Circle
	{
		public CircleBoby()
		{
		}

		public CircleBoby(double r) : base(r)
		{
		}

		public CircleBoby(double r, double x, double y) : base(r, x, y)
		{
		}

		public CircleBoby(double r, Point p) : base(r, p)
		{
		}
	}
}
