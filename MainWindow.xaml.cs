using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Figures
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Body[] bodies = new Body[10];


		public MainWindow()
		{
			InitializeComponent();

			var disp = Dispatcher;

			{
				//			bodies[0] = new Body(330, 170, 70, 100, 0.1, 0.001, 20, ref bodies, ref disp);
				//			Grid.Children.Add(bodies[0].Figure);
				//
				//			bodies[1] = new Body(330, 370, 70, 100, 0.1, 0.001, 50, ref bodies, ref disp);
				//			Grid.Children.Add(bodies[1].Figure);
				//
				//			bodies[2] = new Body(600, 500, 100, 100, 0.5, 0.002, 10, ref bodies, ref disp);
				//			Grid.Children.Add(bodies[2].Figure);
				//
				//			bodies[3] = new Body(900, 500, (25.0 / 2) * (1280 / 325.0), 100, 6.45 / 1000, 0.005, 30, ref bodies, ref disp);
				//			Grid.Children.Add(bodies[3].Figure);
				//
				//			bodies[4] = new Body(1000, 400, 50, 100, 6.45 / 1000, 0.01, 30, ref bodies, ref disp);
				//			Grid.Children.Add(bodies[4].Figure);
				//			bodies[0] = new Body(200, 300, 100, 100, 1, 0.01, 30, ref bodies, Brushes.Blue, Brushes.DarkOliveGreen, ref disp);
				//			Grid.Children.Add(bodies[0].Figure);
				//
				//			bodies[1] = new Body(500, 300, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//			Grid.Children.Add(bodies[1].Figure);
				//
				//			bodies[2] = new Body(800, 300, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//			Grid.Children.Add(bodies[2].Figure);
				//
				//			bodies[3] = new Body(800, 600, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//			Grid.Children.Add(bodies[3].Figure);
				//
				//			bodies[4] = new Body(500, 600, 100, 100, 1, 0.01, 30, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//			Grid.Children.Add(bodies[4].Figure);
				//bodies[0] = new Body(200, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[0].Figure);
				//bodies[1] = new Body(300, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[1].Figure);
				//bodies[2] = new Body(400, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[2].Figure);
				//bodies[3] = new Body(500, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[3].Figure);
				//bodies[4] = new Body(600, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[4].Figure);
				//bodies[5] = new Body(200, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[5].Figure);
				//bodies[6] = new Body(300, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[6].Figure);
				//bodies[7] = new Body(400, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[7].Figure);
				//bodies[8] = new Body(500, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[8].Figure);
				//bodies[9] = new Body(700, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[9].Figure);
			}

			for (var i = 0; i < 10; i++)
			{
				bodies[i] = new Body(200 + i * 100 - (i + 1) / 5 * 400, 300 + (i + 1) / 5 * 300, 40, 1, 0.01, 70, Brushes.Tomato, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
			}

			var fps = 50.0;

			var timer = new Timer { Interval = 1000 / fps, AutoReset = true, Enabled = true };
			timer.Elapsed += Step;
			timer.Start();

			{
				//bodies[0] = new Body(300, 500, 100, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[0].Figure);
				//bodies[1] = new Body(600, 500, 100, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[1].Figure);
				//bodies[2] = new Body(900, 500, 100, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
				//Grid.Children.Add(bodies[2].Figure);
			}
		}

		public Vector Norm(Vector a)
		{
			var b = a;
			b.Normalize();
			return b;
		}

		public void Step(object sender, ElapsedEventArgs e)
		{
			var spf = (sender as Timer).Interval;
			var fps = 1000.0 / spf;
			foreach (var b in bodies)
			{
				if (b.Momentum.LengthSquared == 0)
				//if (b.Velocity.LengthSquared >= b.Velocity0.LengthSquared)
				{
					continue;
				}
				b.Time += spf;

				var velocityD = b.Accelerate * (b.Time * b.Time * 0.5);
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

				b.X += b.Velocity.X / fps;
				b.Y += b.Velocity.Y / fps;
			}
			foreach (var b in bodies)
			{
				if (b.Momentum.LengthSquared == 0)
				//if (b.Velocity.LengthSquared >= b.Velocity0.LengthSquared)
				{
					continue;
				}
				//Столкновение
				//var VelocityEps = Velocity.LengthSquared / Fps / KV * 3;
				//var velocityEps = Velocity.LengthSquared / Fps / KV;
				//var velocityEps = velocityD.Length;
				//var velocityEps2 = velocityD.LengthSquared;
				//var velocityEps2 = velocityEps * velocityEps;
				//var VelocityEps = 0;
				foreach (var body in bodies)
				{
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
			foreach (var b in bodies)
			{
				if (b.Momentum.LengthSquared < 1)
				{
					continue;
				}
				MoveEllipse(b.Figure, new Thickness(b.X - b.Radius, 0, 0, b.Y - b.Radius));
			}
		}
		public void MoveEllipse(Ellipse el, Thickness margin)
		{
			Action action = () => el.Margin = margin;
			try { Dispatcher.Invoke(action); }
			catch { /*ignored*/ }
		}
	}
}
