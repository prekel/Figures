using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGeometry.Core;

namespace Figures.Core
{
	public class CircleBody : Circle, IMovable, ICloneable
	{
		private Vector _f, _a, _v, _p, _v0, _s0, _s;
		private double _x, _y, _r, _m, _μ, _fr, _kv, _pt;

		/// <summary> Номер </summary>
		public int Number { get; set; }

		/// <summary> Масcа </summary>
		public double Mass { get { return _m; } set { _m = value; } }

		/// <summary> Радиус </summary>
		public double Radius { get { return _r; } set { _r = value; } }

		/// <summary> Коэффициент трения </summary>
		public double Mu { get { return _μ; } set { _μ = value; } }

		/// <summary> Трение </summary>
		public double Friction { get { return _fr; } set { _fr = value; } }

		/// <summary> Коэффициент скорости </summary>
		public double KV { get { return _kv; } set { _kv = value; } }

		/// <summary> Кадры в секунду </summary>
		public double PTime { get { return _pt; } set { _pt = value; } }

		/// <summary> Вектор силы </summary>
		public Vector Force { get { return _f; } set { _f = value; } }

		/// <summary> Вектор ускорения </summary>
		public Vector Accelerate { get { return _a; } set { _a = value; } }

		/// <summary> Вектор скорости </summary>
		public Vector Velocity { get { return _v; } set { _v = value; } }

		/// <summary> Вектор импульса </summary>
		public Vector Momentum { get { return _p; } set { _p = value; } }

		/// <summary> Вектор начальной скорости </summary>
		public Vector Velocity0 { get { return _v0; } set { _v0 = value; } }

		/// <summary> Вектор начального перемещения </summary>
		public Vector Shift0 { get { return _s0; } set { _s0 = value; } }

		/// <summary> Вектор перемещения </summary>
		public Vector Shift { get { return _s; } set { _s = value; } }

		private void Init()
		{
			Force = new Vector();
			Accelerate = new Vector();
			Velocity = new Vector();
			Momentum = new Vector();
			Velocity0 = new Vector();
			Shift0 = new Vector();
			Shift = new Vector();
		}

		public CircleBody()
		{
			Init();
		}

		public CircleBody(double r) : base(r)
		{
			Init();
		}

		public CircleBody(double r, double x, double y) : base(r, x, y)
		{
			Init();
		}

		public CircleBody(double r, Point p) : base(r, p)
		{
			Init();
		}

		public new void Move(Vector v)
		{
			base.Move(v);
		}

		public void Step()
		{
			Move(Velocity);
		}

		public void Step(double time)
		{
			Move(Velocity * time);
		}
	}
}
