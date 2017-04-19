// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGeometry.Core;

namespace Figures.Core
{
	public class CircleBody : Circle, IMovable, ICloneable, IFigure
	{
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

		/// <summary> Вектор импульса </summary>
		public Vector Momentum { get { return _p; } set { _p = value; } }

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
			set { Forces["Friction"] = value; }
		}

		public Forces Forces { get; set; } = new Forces();

		public GravityForces GravityForces { get; set; } = new GravityForces();

		public Vector DVelocity { get; set; } = new Vector();

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

		public void Move()
		{
			base.Move(DVelocity);
		}

		public void Step()
		{
			Move(Velocity);
		}

		public void Step(double dt)
		{
			foreach (var i in Scene)
			{
				if (!(i is CircleBody))
					continue;
				var j = (CircleBody)i;
				if (j == this)
					continue;
				var f = (G * Mass * j.Mass) / DistanceSquared(j);
				Forces[j.Number.ToString()] = Vector.Normalize(new Vector(this, j)) * f;
			}

			//var res = (Forces.Resultant + GravityForces.Resultant + Friction);
			//var res = (Forces.Resultant + GravityForces.Resultant);// + Friction);
			var res = Forces.Resultant;
			//Friction = res * -Mu;

			var v2 = (res) * dt / Mass;

			//var v1 = Accelerate * dt;
			//Velocity += v1;
			Velocity += v2;
			//if (Velocity.X != 0 && Velocity.Y != 0)
			//	Friction = Vector.Normalize(Velocity) * Mass * g * -Mu / 100;
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
