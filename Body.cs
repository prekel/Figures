using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Timers;
using System.Windows.Threading;
using System.IO;

namespace Figures
{
	public class Body
	{
		Vector _f, _a, _v, _p, _v0;
		double _x, _y, _r, _m, _t, _μ, _fps, _spf, _fr, _kv;
		const double g = 9.80665;

		/// <summary> Масса </summary>
		public double Mass { get { return _m; } set { _m = value; } }
		/// <summary> Абцисса центра </summary>
		public double X { get { return _x; } set { _x = value; } }
		/// <summary> Ордината центра </summary>
		public double Y { get { return _y; } set { _y = value; } }
		/// <summary> Радиус </summary>
		public double Radius { get { return _r; } set { _r = value; } }
		/// <summary> Время с начала движения </summary>
		public double Time { get { return _t; } set { _t = value; } }
		/// <summary> Коэффициент трения </summary>
		public double Mu { get { return _μ; } set { _μ = value; } }
		/// <summary> Трение </summary>
		public double Friction { get { return _fr; } set { _fr = value; } }
		/// <summary> Кадры в секунду </summary>
		public double Fps { get { return _fps; } set { _fps = value; _spf = 1000 / _fps; } }
		/// <summary> Миллисекунды в кадре </summary>
		public double Spf { get { return _spf; } set { _spf = value; _fps = 1000 / _spf; } }
		/// <summary> Коэффициент скорости </summary>
		public double KV { get { return _kv; } set { _kv = value; } }
		/// <summary> Вектор силы </summary>
		public Vector Force { get { return _f; } set { _f = value; } }
		/// <summary> Вектор ускорения </summary>
		public Vector Accelerate { get { return _a; } set { _a = value; } }
		/// <summary> Вектор скорости </summary>
		public Vector Velocity { get { return _v; } set { _v = value; } }
		/// <summary> Вектор импульса </summary>
		public Vector Momentum { get { return _p; } set { _p = value; } }
		//public Vector Friction { get { return _fr; } set { _fr = value; } }
		/// <summary> Вектор начальной скорости </summary>
		public Vector Velocity0 { get { return _v0; } set { _v0 = value; } }

		/// <summary> Фигура </summary>
		public Ellipse Figure;

		/// <summary> Диспетчер </summary>
		Dispatcher Disp;
		/// <summary> Таймер </summary>
		Timer timer = new Timer();
		/// <summary> Массив с остальными телами </summary>
		Body[] BodyList;

		//StreamWriter Out = new StreamWriter("v.txt");

		/// <summary> Создаёт тело </summary>
		/// <param name="x"> Абцисса центра (px) </param>
		/// <param name="y"> Ордината центра (px) </param>
		/// <param name="r"> Радиус (px) </param>
		/// <param name="fps"> Кадры в секунду </param>
		/// <param name="m"> Масса (кг) </param>
		/// <param name="speed"> Коэффициент скорост и</param>
		/// <param name="bodylist"> Массив с остальными телами </param>
		/// <param name="disp"> (Dispatcher) </param>
		public Body(double x, double y, double r, double fps, double m, double mu, double speed, ref Body[] bodylist, ref Dispatcher disp)
		{
			Figure = new Ellipse
			{
				Margin = new Thickness(x - r, 0, 0, y - r),
				Fill = Brushes.AliceBlue,
				StrokeThickness = 1,
				Stroke = Brushes.DarkOliveGreen,
				Width = r * 2,
				Height = r * 2,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Bottom
			};
			X = x; Y = y; Radius = r; Fps = fps; Mass = m; Mu = mu; KV = speed; Disp = disp; BodyList = bodylist;
			Figure.MouseUp += MouseUp;
		}

		/// <summary> Происходит при нажатии </summary>
		public void MouseUp(object sender, MouseButtonEventArgs e)
		{
			var Coords = e.GetPosition(Figure);
			if (e.ChangedButton.ToString() == "Right")
			{
				timer.Stop();
				return;
			}
			Coords.X -= Radius;
			//Coords.Y -= Figure.Height / 2;
			Coords.Y = -Coords.Y + Radius;

			//var l = Math.Sqrt(Coords.X * Coords.X + Coords.Y * Coords.Y);
			//var angle = Vector.AngleBetween(new Vector(1, 0), new Vector(Coords.X, Coords.Y));

			Time = 0;

			Velocity0 = new Vector(-Coords.X, -Coords.Y);
			Velocity = Velocity0;
			Accelerate = -Velocity;
			_a.Normalize();
			Friction = Mu * Mass * g;
			Accelerate *= Friction;

			Momentum = Velocity * Mass;



			//Force = Accelerate * Mass;

			//			Accelerate = new Vector(-Coords.X, -Coords.Y);
			//			Force = Accelerate * Mass;
			//			Velocity = new Vector(0, 0);
			//			Momentum = Velocity * Mass;
			//			//Friction = Force;
			//			//_fr.Normalize();
			//			Friction = Mu * Mass * g;

			timer.Stop();
			timer = new Timer { Interval = Spf, AutoReset = true, Enabled = true };
			timer.Elapsed += Move;
			timer.Start();
		}

		/// <summary> Шаг </summary>
		public void Move(object sender, ElapsedEventArgs e)
		{
			Time += Spf;
			var t = Time;

			//Velocity = Accelerate * (t * t * 0.5);
			//Velocity = Accelerate * (t * t * 0.5);
			//if (Velocity - Accelerate) { timer.Stop(); return; }
			//Velocity += Accelerate;
			//Accelerate += Friction * (Spf / 1000);
			//Accelerate *= 1 - Friction * (Spf / 1000);
			//if (Accelerate.LengthSquared < 0.001) { timer.Stop(); return; }
			//if (Velocity.LengthSquared < 0.001) { timer.Stop(); return; }
			//Force = Accelerate * Mass;
			//Momentum = Velocity * Mass;
			//Friction -= Friction * (Spf / 1000);

			Momentum = Velocity * Mass;

			var VelocityD = Accelerate * (t * t * 0.5);
			Velocity = Velocity0 + VelocityD;
			if (Velocity0.LengthSquared < VelocityD.LengthSquared)
			{
				timer.Stop(); return;
			}

			var VelocityEps = Velocity.LengthSquared / Fps * KV;
			foreach (var body in BodyList)
			{
				if (body != this)
				{
					//var m = Math.Sqrt((X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y));
					var m2 = (X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y);
					var r = (Radius + body.Radius);
					var r2 = r * r;
					//double b = 50;
					//double b2 = 2222;
					//if (Math.Abs(m2 - r2) < b2)
					if (m2 <= r2 + VelocityEps)
					{
						timer.Stop(); return;
					}
				}
			}

			//Out.WriteLine(Velocity);
			//if (Velocity.LengthSquared < 1) { 
			//	timer.Stop(); return; }

			//Velocity *= Friction;

			X += Velocity.X / Fps * KV;
			Y += Velocity.Y / Fps * KV;
			Action action = () => Figure.Margin = new Thickness(X - Radius, 0, 0, Y - Radius);
			try { Disp.Invoke(action); }
			catch { return; }
			//MoveEllipse(Figure, new Thickness(X, 0, 0, Y));

		}

		/// <summary> Перемещает фигуру </summary>
		public void MoveEllipse(Ellipse el, Thickness margin)
		{
			Action action = () => el.Margin = margin;
			try { Disp.Invoke(action); }
			catch { return; }
		}
	}
}
