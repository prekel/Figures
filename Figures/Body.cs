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
using System.Diagnostics;

namespace Figures
{
	/// <summary> Тело в виде Ellipse </summary>
	public class Body
	{
		/// <summary> Ускорение свободного падения </summary>
		public const double G = 9.80665;
		/// <summary> Множитель для увелечения силы трения </summary>
		private const double FK = 1000;

		private Vector _f, _a, _v, _p, _v0, _s0, _s;
		private double _x, _y, _r, _m, _μ, _fr, _kv, _pt;

		public delegate void StringContainer(string s);
		public static event StringContainer NewLog;
		public static event StringContainer MomentumChange;

		/// <summary> Номер </summary>
		public int Number { get; set; }
		/// <summary> Масcа </summary>
		public double Mass { get { return _m; } set { _m = value; } }
		/// <summary> Абцисса центра </summary>
		public double X { get { return _x; } set { _x = value; } }
		/// <summary> Ордината центра </summary>
		public double Y { get { return _y; } set { _y = value; } }
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

		/// <summary> Фигура </summary>
		public Ellipse Figure;
		/// <summary> Время </summary>
		public Stopwatch Clock = new Stopwatch();

		/// <summary> Создаёт тело </summary>
		/// <param name="n"> Номер </param>
		/// <param name="x"> Абцисса центра (px) </param>
		/// <param name="y"> Ордината центра (px) </param>
		/// <param name="r"> Радиус (px) </param>
		/// <param name="m"> Масса (кг) </param>
		/// <param name="mu"> Коэффициент трения </param>
		/// <param name="speed"> Коэффициент скорости </param>
		/// <param name="fill"> Кисть заполнения </param>
		/// <param name="fillstroke"> Кисть границы </param>
		public Body(int n, double x, double y, double r, double m, double mu, double speed, Brush fill, Brush fillstroke)
		{
			Figure = new Ellipse
			{
				Margin = new Thickness(x - r, 0, 0, y - r),
				Fill = fill,
				StrokeThickness = 1,
				Stroke = fillstroke,
				Width = r * 2,
				Height = r * 2,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Bottom
			};
			X = x; Y = y; Radius = r; Mass = m; Mu = mu; KV = speed; Number = n;
			Friction = Mu * Mass * G * FK;
			Figure.MouseUp += StartByClick;
		}

		/// <summary> Старт при нажатии </summary>
		public void StartByClick(object sender, MouseButtonEventArgs e)
		{
			var coords = e.GetPosition(Figure);
			if (e.ChangedButton.ToString() == "Right")
			{
				StartMoveByMomentum(new Vector(0, 0));
			}
			else
			{
				coords.X -= Radius;
				coords.Y = -coords.Y + Radius;
				Momentum = new Vector(-coords.X, -coords.Y) * KV * Mass;
				StartMoveByMomentum(Momentum);
			}

			if (MomentumChange != null)
				MomentumChange("$momentumchange " + Number + " " + X + " " + Y + " " + Momentum.X + " " + Momentum.Y);
		}

		/// <summary> Старт при изменении импульса </summary>
		public void StartMoveByMomentum(Vector momentum)
		{
			Momentum = momentum;

			Shift0 = new Vector(X, Y);

			Velocity0 = Momentum / Mass;
			Velocity = Velocity0;

			Accelerate = -Velocity;
			_a.Normalize();
			Accelerate *= Friction / Mass;

			Clock.Restart();
			PTime = 0;
		}

		/// <summary> 
		/// Основной цикл движения;
		/// { Body[] Массив с телами,
		/// Dispatcher Диспетчер от основного потока,
		/// TextBlock Счётчик фпс, 
		/// int лимит фпс }
		/// </summary>
		public static void Move(object Param)
		{
			var obj = Param as object[];
			var clock = new Stopwatch();
			var bodies = obj[0] as Body[];
			var disp = obj[1] as Dispatcher;
			var fpsCount = obj[2] as TextBlock;

			var FPS = new FpsControl((int)obj[3], (int)obj[4]);

			while (true)
			{
				//Проверка на движение и движение
				foreach (var b in bodies)
				{
					if (b == null)
						break;
					if (b.Momentum.LengthSquared == 0)
						continue;

					//var t = b.Clock.ElapsedMilliseconds / 1000.0;
					//var t = b.Clock.ElapsedTicks / 10000000.0;
					//var t = b.Clock.ElapsedMilliseconds / 1000.0;
					//var f = (double)Stopwatch.Frequency;
					//var t = b.Clock.ElapsedTicks / f;
					var dt = FPS.ActualTime;
					//Debug.WriteLine(b.Clock.ElapsedMilliseconds + " " + b.Clock.ElapsedTicks / f + " " + f);
					//var dt =  t - b.PTime;
					//b.PTime = t;
					//var t = b.Clock.Elapsed.TotalSeconds;
					//var velocityD = b.Accelerate * (t * t * 0.5);
					//var velocityD = b.Accelerate * t;
					var v1 = b.Accelerate * dt;
					if (v1.LengthSquared > b.Velocity.LengthSquared)
					{
						b.StartMoveByMomentum(new Vector(0, 0));
						b.Clock.Stop();
						continue;
					}
					b.Velocity += v1;
					//var dv = b.Velocity0 + v1;
					var ds = b.Velocity * dt;

					b.Momentum = b.Velocity * b.Mass;

					b.X += ds.X;
					b.Y += ds.Y;

					//var ShiftD1 = b.Velocity0 * t;
					//var ShiftD2 = b.Accelerate * (t * t * 0.5);

					//b.Shift = b.Shift0 + ShiftD1 + ShiftD2;

					//b.X = b.Shift.X;
					//b.Y = b.Shift.Y;

					//if (ShiftD1.LengthSquared < (ShiftD2 * 2).LengthSquared)
					//{
					//	b.StartMoveByMomentum(new Vector(0, 0));
					//b.Clock.Stop();
					//	continue;
					//}

					//b.Velocity = b.Momentum / b.Mass;
					//b.Velocity = b.Velocity0 + b.Accelerate * t;
					//b.Momentum = b.Velocity * b.Mass;

					//b.Velocity = b.Velocity0 + velocityD;
					//b.X += b.Velocity.X * FPS.ActualTime / 1000;
					//b.Y += b.Velocity.Y * FPS.ActualTime / 1000;*/
				}


				//Проверка на столкновение и столкновение
				foreach (var b in bodies)
				{
					if (b == null)
						break;
					if (b.Momentum.LengthSquared == 0)
						continue;
					foreach (var body in bodies)
					{
						if (body == null)
							break;
						if (body == b)
							continue;
						if (body.Momentum.LengthSquared > 0)
							continue;

						// m2 - Квадрат расстояния, r2 - квадрат суммы радиусов
						//var m = Math.Sqrt((X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y));
						var m2 = (b.X - body.X) * (b.X - body.X) + (b.Y - body.Y) * (b.Y - body.Y);
						var r = b.Radius + body.Radius;
						var r2 = r * r;
						if (m2 < r2)
						{
							var ds = b.Velocity * FPS.ActualTime;
							b.X -= ds.X;
							b.Y -= ds.Y;
							m2 = (b.X - body.X) * (b.X - body.X) + (b.Y - body.Y) * (b.Y - body.Y);

							var vb = new Vector(body.X - b.X, body.Y - b.Y);
							vb.Normalize();
							vb *= Math.Sqrt(m2) - r;
							Debug.WriteLine(b.Number + " " + body.Number + " " + FPS.ActualSumTime + " " + FPS.ActualTime + " " + ds + " " + Math.Sqrt(m2));
							Debug.WriteLine(b.Momentum + " " + body.Momentum + " " + vb);
							body.Momentum = new Vector(body.X - b.X, body.Y - b.Y);
							b.X += vb.X;
							b.Y += vb.Y;

							Debug.WriteLine(b.Momentum + " " + body.Momentum);

							var temp = body.Momentum;
							temp.Normalize();
							body.Momentum = temp;

							body.Momentum *= b.Momentum.Length * Math.Abs(Math.Cos(Vector.AngleBetween(b.Momentum, body.Momentum) * 180 / Math.PI));

							b.Momentum = b.Momentum - body.Momentum;
							Debug.WriteLine(b.Momentum + " " + body.Momentum + '\n');

							b.StartMoveByMomentum(b.Momentum);
							body.StartMoveByMomentum(body.Momentum);

							if (NewLog != null) NewLog(b.Momentum + "\n" + body.Momentum + "\n");

							//break;
							goto draw;
						}
					}
				}

				draw:;

				// Приостановка потока на часть отведенного времени для кадра
				//FPS.SleepPart(0.1);

				// Проверка на движение и рисование в основном потоке
				Action action = () =>
				{
					foreach (var b in bodies)
					{
						if (b == null)
							break;
						if (b.Momentum.LengthSquared == 0)
							continue;
						b.Figure.Margin = new Thickness(b.X - b.Radius, 0, 0, b.Y - b.Radius);
					}
					fpsCount.Text = ((int)FPS.FrameAverage).ToString();
					//Debug.WriteLine(FPS.FrameAverage);
				};
				try { disp.Invoke(action); }
				catch { return; }

				// Приостановка потока для лимита фпс 
				// Вычисление фпс
				FPS.Calc();
			}
		}
	}
}
