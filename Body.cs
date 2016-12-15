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
		private const double FK = 10e5;

		private Vector _f, _a, _v, _p, _v0;
		private double _x, _y, _r, _m, _t, _μ, _fr, _kv;

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
		/// <summary> Кадры в секунду </summary>
		public double KV { get { return _kv; } set { _kv = value; } }
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

		/// <summary> Фигура </summary>
		public Ellipse Figure;
		/// <summary> Время </summary>
		public Stopwatch Clock = new Stopwatch();

		/// <summary> Создаёт тело </summary>
		/// <param name="x"> Абцисса центра (px) </param>
		/// <param name="y"> Ордината центра (px) </param>
		/// <param name="r"> Радиус (px) </param>
		/// <param name="m"> Масса (кг) </param>
		/// <param name="mu"> Коэффициент трения </param>
		/// <param name="speed"> Коэффициент скорости </param>
		/// <param name="fill"> Кисть заполнения </param>
		/// <param name="fillstroke"> Кисть границы </param>
		public Body(double x, double y, double r, double m, double mu, double speed, Brush fill, Brush fillstroke)
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
			X = x; Y = y; Radius = r; Mass = m; Mu = mu; KV = speed;
			Friction = Mu * Mass * G * FK;
			Figure.MouseUp += StartByClick2;
		}

		/// <summary> Старт при нажатии </summary>
		public void StartByClick2(object sender, MouseButtonEventArgs e)
		{
			var coords = e.GetPosition(Figure);
			if (e.ChangedButton.ToString() == "Right")
			{
				Momentum = new Vector(0, 0);
			}
			else
			{
				coords.X -= Radius;
				coords.Y = -coords.Y + Radius;
				Momentum = new Vector(-coords.X, -coords.Y) * KV * Mass;
			}
			StartMoveByMomentum1(Momentum);
		}

		/// <summary> Старт при столкновени </summary>
		public void StartMoveByMomentum1(Vector momentum)
		{
			Clock.Restart();
			Momentum = momentum;
			Velocity0 = Momentum / Mass;
			Velocity = Velocity0;
			Accelerate = -Velocity;
			_a.Normalize();
			Accelerate *= Friction;
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
			double FPS = 0;
			var disp = obj[1] as Dispatcher;
			var fpsCount = obj[2] as TextBlock;
			var mspflimit = 1000 / ((int)obj[3]);
			int fpscountcapacity = 20;
			var fpsdeque = new Queue<double>(fpscountcapacity);
			var fpssum = 0.0;
			while (true)
			{
				clock.Restart();

				//Проверка на движение и движение
				foreach (var b in bodies)
				{
					if (b == null)
						continue;
					if (b.Momentum.LengthSquared == 0)
						continue;

					var t = b.Clock.ElapsedMilliseconds / 1000.0;
					var velocityD = b.Accelerate * (t * t * 0.5);

					//Остановка при маленькой скорости
					if (b.Velocity0.LengthSquared < velocityD.LengthSquared)
					{
						b.Velocity = new Vector(0, 0);
						b.Momentum = b.Velocity;
						continue;
					}

					b.Velocity = b.Momentum / b.Mass;
					b.Velocity = b.Velocity0 + velocityD;
					b.Momentum = b.Velocity * b.Mass;

					b.X += b.Velocity.X / (fpssum / fpsdeque.Count);
					b.Y += b.Velocity.Y / (fpssum / fpsdeque.Count);
				}

				//Проверка на столкновение и столкновение
				foreach (var b in bodies)
				{
					if (b == null)
						continue;
					if (b.Momentum.LengthSquared == 0)
						continue;
					foreach (var body in bodies)
					{
						if (body == null)
							continue;
						if (body == b)
							continue;

						// m2 - Квадрат расстояния, r2 - квадрат суммы радиусов
						//var m = Math.Sqrt((X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y));
						var m2 = (b.X - body.X) * (b.X - body.X) + (b.Y - body.Y) * (b.Y - body.Y);
						var r = b.Radius + body.Radius;
						var r2 = r * r;
						if (m2 <= r2)
						{
							var vb = new Vector(body.X - b.X, body.Y - b.Y);
							vb.Normalize();
							vb *= Math.Sqrt(m2) - r;
							b.X += vb.X;
							b.Y += vb.Y;
							body.Momentum = new Vector(body.X - b.X, body.Y - b.Y);

							var temp = body.Momentum;
							temp.Normalize();
							body.Momentum = temp;

							body.Momentum *= b.Momentum.Length * Math.Abs(Math.Cos(Vector.AngleBetween(b.Momentum, body.Momentum) * 180 / Math.PI));

							b.Momentum = b.Momentum - body.Momentum;

							b.StartMoveByMomentum1(b.Momentum);
							body.StartMoveByMomentum1(body.Momentum);

							//break;
							goto draw;
						}

					}
				}

				draw:;
				// Приостановка потока на половину отведенного времени для кадра
				if (clock.ElapsedMilliseconds < mspflimit / 2)
				{
					try { System.Threading.Thread.Sleep(mspflimit / 2 - (int)clock.ElapsedMilliseconds); }
					catch { /*ignored*/ };
				}

				// Проверка на движение и рисование в основном потоке
				Action action = () =>
				{
					foreach (var b in bodies)
					{
						if (b == null)
							continue;
						if (b.Momentum.LengthSquared == 0)
							continue;
						b.Figure.Margin = new Thickness(b.X - b.Radius, 0, 0, b.Y - b.Radius);
						//fpsCount.Text = FPS.ToString();
						fpsCount.Text = ((int)(fpssum / fpsdeque.Count)).ToString();
					}
				};
				try { disp.Invoke(action); }
				catch { return; }

				// Приостановка потока для лимита фпс 
				if (clock.ElapsedMilliseconds < mspflimit)
				{
					try { System.Threading.Thread.Sleep(mspflimit - (int)clock.ElapsedMilliseconds); }
					catch { /*ignored*/ };
				}

				// Вычисление фпс
				try
				{
					FPS = 1000.0 / clock.ElapsedMilliseconds;
					fpsdeque.Enqueue(FPS);
					fpssum += FPS;
					if (fpsdeque.Count > fpscountcapacity)
					{
						fpssum -= fpsdeque.Dequeue();
					}
				}
				catch
				{
					FPS = 0;
				}
			}
		}
	}
}
