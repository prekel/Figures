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
	public class Body
	{
		private Vector _f, _a, _v, _p, _v0;
		private double _x, _y, _r, _m, _t, _μ, _fps, _spf, _fr, _kv;
		private const double G = 9.80665, FK = 10e5;

		/// <summary> Масcа </summary>
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
		private readonly Dispatcher Disp;
		/// <summary> Таймер </summary>
		private Timer timer;// = new Timer();
							/// <summary> Массив с остальными телами </summary>
		private Body[] BodyList;
		/// <summary> Время </summary>
		private Stopwatch Clock = new Stopwatch();

		/// <summary> Создаёт тело </summary>
		/// <param name="x"> Абцисса центра (px) </param>
		/// <param name="y"> Ордината центра (px) </param>
		/// <param name="r"> Радиус (px) </param>
		/// <param name="fps"> Кадры в секунду </param>
		/// <param name="m"> Масса (кг) </param>
		/// <param name="mu"> Коэффициент трения </param>
		/// <param name="speed"> Коэффициент скорости </param>
		/// <param name="bodylist"> Массив с остальными телами </param>
		/// <param name="fill"></param>
		/// <param name="fillstroke"></param>
		/// <param name="disp"> (Dispatcher) </param>
		public Body(double x, double y, double r, double fps, double m, double mu, double speed, ref Body[] bodylist, Brush fill, Brush fillstroke, ref Dispatcher disp)
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
			X = x; Y = y; Radius = r; Fps = fps; Mass = m; Mu = mu; KV = speed; Disp = disp; BodyList = bodylist;
			Friction = Mu * Mass * G;
			Figure.MouseUp += StartByClick1;
		}

		/// <summary> Создаёт тело </summary>
		/// <param name="x"> Абцисса центра (px) </param>
		/// <param name="y"> Ордината центра (px) </param>
		/// <param name="r"> Радиус (px) </param>
		/// <param name="fps"> Кадры в секунду </param>
		/// <param name="m"> Масса (кг) </param>
		/// <param name="mu"> Коэффициент трения </param>
		/// <param name="speed"> Коэффициент скорости </param>
		/// <param name="bodylist"> Массив с остальными телами </param>
		/// <param name="fill"></param>
		/// <param name="fillstroke"></param>
		/// <param name="disp"> (Dispatcher) </param>
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
		public void StartByClick1(object sender, MouseButtonEventArgs e)
		{
			var coords = e.GetPosition(Figure);
			if (e.ChangedButton.ToString() == "Right")
			{
				//timer.Stop();
				return;
			}
			coords.X -= Radius;
			coords.Y = -coords.Y + Radius;

			//Time = 0;
			Velocity0 = new Vector(-coords.X, -coords.Y) * KV;
			Velocity = Velocity0;
			Accelerate = -Velocity;
			_a.Normalize();
			Accelerate *= Friction;

			Momentum = Velocity * Mass;
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

		/// <summary> Старт при нажатии </summary>
		public void StartByClick(object sender, MouseButtonEventArgs e)
		{
			var coords = e.GetPosition(Figure);
			if (e.ChangedButton.ToString() == "Right")
			{
				timer.Stop();
				return;
			}
			coords.X -= Radius;
			//Coords.Y -= Figure.Height / 2;
			coords.Y = -coords.Y + Radius;

			//var l = Math.Sqrt(Coords.X * Coords.X + Coords.Y * Coords.Y);
			//var angle = Vector.AngleBetween(new Vector(1, 0), new Vector(Coords.X, Coords.Y));

			Time = 0;

			Velocity0 = new Vector(-coords.X, -coords.Y) * KV;
			Velocity = Velocity0;
			Accelerate = -Velocity;
			_a.Normalize();
			//Friction = Mu * Mass * G;
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
			timer.Elapsed += Step0;
			timer.Start();
		}

		/// <summary> Старт при столкновени </summary>
		public void StartMoveByMomentum(Vector momentum)
		{
			Time = 0;

			Momentum = momentum;

			Velocity0 = Momentum / Mass;
			Velocity = Velocity0;
			Accelerate = -Velocity;
			_a.Normalize();
			Accelerate *= Friction;

			//Momentum = Velocity * Mass;



			//Force = Accelerate * Mass;

			//			Accelerate = new Vector(-Coords.X, -Coords.Y);
			//			Force = Accelerate * Mass;
			//			Velocity = new Vector(0, 0);
			//			Momentum = Velocity * Mass;
			//			//Friction = Force;
			//			//_fr.Normalize();
			//			Friction = Mu * Mass * g;

			try
			{
				timer.Stop();
			}
			finally
			{
				timer = new Timer { Interval = Spf, AutoReset = true, Enabled = true };
				timer.Elapsed += Step0;
				timer.Start();
			}
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


		public static void Move(object Param)
		{
			var obj = Param as object[];
			var clock = new Stopwatch();
			var bodies = obj[0] as Body[];
			double FPS = 0;
			var disp = obj[1] as Dispatcher;
			var fpsCount = obj[2] as TextBlock;
			var mspflimit = 1000 / ((int)obj[3]);
			int fpscountcapacity = 10;
			var fpsdeque = new Queue<double>(fpscountcapacity);
			var fpssum = 0.0;
			while (true)
			{
				clock.Restart();
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
						//timer.Stop();
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
				foreach (var b in bodies)
                {
                    if (b == null)
                        continue;
					if (b.Momentum.LengthSquared == 0)
						continue;
					//Столкновение
					foreach (var body in bodies)
                    {
                        if (body == null)
                            continue;
						if (body != b)// && b.Momentum.LengthSquared > 0)
						{
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
								goto jkl;
							}
						}
					}
				}

				jkl:;

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
				
				if (clock.ElapsedMilliseconds < mspflimit)
				{
					try { System.Threading.Thread.Sleep(mspflimit - (int)clock.ElapsedMilliseconds); }
					catch { /*ignored*/ };
				}

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
					FPS = 1000.0;
					//fpsdeque.Enqueue(FPS);
					//fpssum += FPS;
					//if (fpsdeque.Count > fpscountcapacity)
					//{
					//	fpssum -= fpsdeque.Dequeue();
					//}
				}
			}
		}

		/// <summary> Шаг </summary>
		public void Step0(object sender, ElapsedEventArgs e)
		{
			Time += Spf;

			var velocityD = Accelerate * (Time * Time * 0.5);
			//Остановка при маленькой скорости
			if (Velocity0.LengthSquared < velocityD.LengthSquared)
			{
				timer.Stop();
				Velocity = new Vector(0, 0);
				Momentum = Velocity;
				return;
			}

			Velocity = Momentum / Mass;
			Velocity = Velocity0 + velocityD;
			Momentum = Velocity * Mass;

			X += Velocity.X / Fps;
			Y += Velocity.Y / Fps;

			//Столкновение
			//var VelocityEps = Velocity.LengthSquared / Fps / KV * 3;
			//var velocityEps = Velocity.LengthSquared / Fps / KV;
			var velocityEps = velocityD.Length;
			var velocityEps2 = velocityD.LengthSquared;
			//var velocityEps2 = velocityEps * velocityEps;
			//var VelocityEps = 0;
			foreach (var body in BodyList)
			{
				if (body != this)// && body.Momentum.LengthSquared == 0)
				{
					//var m = Math.Sqrt((X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y));
					var m2 = (X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y);
					var r = Radius + body.Radius;
					var r2 = r * r;
					//double b = 50;
					//double b2 = 2222;
					//if (Math.Abs(m2 - r2) < b2)
					//if (Math.Sqrt(m2) - r < velocityEps && m2 - r2 > 0)
					//{
					//	var vb = new Vector(body.X - X, body.Y - Y);
					//	//var vc = new Vector(body.X - X, body.Y - Y);
					//	vb.Normalize();
					//	vb *= Math.Sqrt(m2) - r;
					//	vb *= 1.001;
					//	X += vb.X;
					//	Y += vb.Y;
					//	MoveEllipse(Figure, new Thickness(X - Radius, 0, 0, Y - Radius));
					//	return;
					//}
					if (m2 <= r2)
					{
						var vb = new Vector(body.X - X, body.Y - Y);
						vb.Normalize();
						vb *= Math.Sqrt(m2) - r;
						X += vb.X;
						Y += vb.Y;
						body.Momentum = new Vector(body.X - X, body.Y - Y);
						body._p.Normalize();
						body.Momentum *= Momentum.Length * Math.Abs(Math.Cos(Vector.AngleBetween(Momentum, body.Momentum) * 180 / Math.PI));
						Momentum = Momentum - body.Momentum;
						MoveEllipse(Figure, new Thickness(X - Radius, 0, 0, Y - Radius));
						body.StartMoveByMomentum(body.Momentum);
						return;
					}
				}
			}



			//Out.WriteLine(Velocity);
			//if (Velocity.LengthSquared < 1) {
			//	timer.Stop(); return; }

			//Velocity *= Friction;

			//X += Velocity.X / Fps;// * KV;
			//Y += Velocity.Y / Fps;// * KV;
			MoveEllipse(Figure, new Thickness(X - Radius, 0, 0, Y - Radius));

		}

		/// <summary> Перемещает фигуру </summary>
		public void MoveEllipse(Ellipse el, Thickness margin)
		{
			Action action = () => el.Margin = margin;
			try { Disp.Invoke(action); }
			catch { /*ignored*/ }
		}
	}
}
