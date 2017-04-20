// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGeometry.Core;

using NLog;

namespace Figures.Core
{
	public class CircleBody : Circle, IMovable, ICloneable, IFigure
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		/// <summary> Ускорение свободного падения </summary>
		public const double g = 9.80665;

		/// <summary> Гравитационная постоянная </summary>
		public const double G = 6.674083131e-11;

		private Vector _f, _a, _v, _p, _v0, _s0, _s;
		private double _x, _y, _r, _m, _μ = 0.1, _fr, _kv, _pt;

		#region Others

		/// <summary> Радиус </summary>
		public double Radius { get { return _r; } set { _r = value; } }

		/// <summary> Коэффициент трения </summary>
		public double Mu { get { return _μ; } set { _μ = value; } }

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

		///// <summary> Вектор импульса </summary>
		//public Vector Momentum { get { return _p; } set { _p = value; } }

		/// <summary> Вектор начальной скорости </summary>
		public Vector Velocity0 { get { return _v0; } set { _v0 = value; } }

		/// <summary> Вектор начального перемещения </summary>
		public Vector Shift0 { get { return _s0; } set { _s0 = value; } }

		/// <summary> Вектор перемещения </summary>
		public Vector Shift { get { return _s; } set { _s = value; } }

		#endregion

		/// <summary> Номер </summary>
		public int Number { get; set; }

		/// <summary> Масcа </summary>
		//public double Mass { get { return _m; } set { _m = value; } }
		public double Mass { get; set; }

		/// <summary> Трение </summary>
		public Vector Friction
		{
			get
			{
				if (!Forces.ContainsKey("Friction"))
					Forces.Add("Friction", new Vector(0, 0));
				return Forces["Friction"];
			}
			set => Forces["Friction"] = value;
		}

		public Forces Forces { get; set; } = new Forces();

		public GravityForces GravityForces { get; set; } = new GravityForces();

		public Vector DVelocity { get; set; } = new Vector();

		public Vector Momentum
		{
			get => Velocity * Mass;
			set => Velocity = value / Mass;
		}

		public Vector NextMomentum { get; set; }

		private void Init()
		{
			Force = new Vector();
			Accelerate = new Vector();
			Velocity = new Vector();
			//Momentum = new Vector();
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

		public void Move()
		{
			base.Move(DVelocity);
		}

		public void Step()
		{
			Move(Velocity);
		}

		public void Step0()
		{
			foreach (var i in Scene)
			{
				if (!(i is CircleBody))
					continue;
				var b = (CircleBody)i;
				if (b == this)
					continue;

				var m2 = DistanceSquared(b);
				var f = (G * Mass * b.Mass) / m2;
				Forces["Gravity " + b.Number.ToString()] = Vector.Normalize(new Vector(this, b)) * f;

				var r = b.R + R;
				var r2 = r * r;

				//if (m2 > r2)
				if (Distance(b) > b.R + R)
					NextMomentum = null;


				if (NextMomentum != null)
					continue;


				if (Momentum.LengthSquared < b.Momentum.LengthSquared)
					continue;


				//if (b.Momentum.LengthSquared == 0)
				//	continue;


				if (m2 > r2)
					continue;

				//var vb = new Vector(X - b.X, Y - b.Y);
				//vb.Normalize();
				//vb *= Math.Sqrt(m2) - r;

				var m = new Vector(this, b);

				//Log.Trace(this);
				//Log.Trace(b);

				//b.X += vb.X;
				//b.Y += vb.Y; 

				//var temp = m;
				//temp.Normalize();
				//m = temp;

				m.Normalize();

				//m *= b.Momentum.Length * Math.Abs(Math.Cos(Vector.AngleBetween(b.Momentum, m) * 180 / Math.PI));
				m *= Momentum.Length * Math.Abs(Vector.CosBetween(b.Momentum, m));

				//Log.Trace(m);

				//b.Momentum = b.Momentum - m;
				//Momentum = m;

				//b.Momentum = b.Momentum - m;

				b.NextMomentum = m;
				NextMomentum = Momentum - m;
				//Momentum = -m;
			}
		}

		public void Step(double dt)
		{
			Momentum = NextMomentum ?? Momentum;
			//NextMomentum = null;

			//var res = (Forces.Resultant + GravityForces.Resultant + Friction);
			//var res = (Forces.Resultant + GravityForces.Resultant);// + Friction);
			var res = Forces.Resultant;
			//Friction = res * -Mu;

			var v2 = (res) * dt / Mass;

			//var v1 = Accelerate * dt;
			//Velocity += v1;
			Velocity += v2;
			if (Velocity.X != 0 && Velocity.Y != 0)
				Friction = Vector.Normalize(Velocity) * Mass * g * -Mu / 10;
			DVelocity = Velocity * dt;
			//Move(Velocity * dt);
		}

		public void Click(Point p, double r)
		{
			//var v = new Vector(this, p);
			//Velocity = -v;
			Velocity = new Vector(p, this) * 4;
		}

		public void Click(double x, double y, double r)
		{
			throw new NotImplementedException();
		}
	}
}
