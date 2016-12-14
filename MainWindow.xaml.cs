﻿using System;
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

			bodies[0] = new Body(200, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[0].Figure);
			bodies[1] = new Body(300, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[1].Figure);
			bodies[2] = new Body(400, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[2].Figure);
			bodies[3] = new Body(500, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[3].Figure);
			bodies[4] = new Body(600, 300, 40, 60, 1, 0.01, 100, ref bodies, Brushes.AliceBlue, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[4].Figure);
			bodies[5] = new Body(200, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[5].Figure);
			bodies[6] = new Body(300, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[6].Figure);
			bodies[7] = new Body(400, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[7].Figure);
			bodies[8] = new Body(500, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[8].Figure);
			bodies[9] = new Body(700, 700, 40, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
			Grid.Children.Add(bodies[9].Figure);



			var timer = new Timer {Interval = 1000 / 100, AutoReset = true, Enabled = true };
			timer.Elapsed += Step;

//			bodies[0] = new Body(300, 500, 100, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
//			Grid.Children.Add(bodies[0].Figure);
//			bodies[1] = new Body(600, 500, 100, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
//			Grid.Children.Add(bodies[1].Figure);
//			bodies[2] = new Body(900, 500, 100, 60, 1, 0.01, 100, ref bodies, Brushes.Tomato, Brushes.DarkOliveGreen, ref disp);
//			Grid.Children.Add(bodies[2].Figure);

			timer.Start();
		}

		public Vector Norm(Vector a)
		{
			var b = a;
			b.Normalize();
			return b;
		}

		public void Step(object sender, ElapsedEventArgs e)
		{
			foreach (var b in bodies)
			{
				b.Time += b.Spf;

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

				b.X += b.Velocity.X / b.Fps;
				b.Y += b.Velocity.Y / b.Fps;

				//Столкновение
				//var VelocityEps = Velocity.LengthSquared / Fps / KV * 3;
				//var velocityEps = Velocity.LengthSquared / Fps / KV;
				//var velocityEps = velocityD.Length;
				//var velocityEps2 = velocityD.LengthSquared;
				//var velocityEps2 = velocityEps * velocityEps;
				//var VelocityEps = 0;
				foreach (var body in bodies)
				{
					if (body != b)// && body.Momentum.LengthSquared == 0)
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
							
							//{
							//	b.Time = 0;
							//	b.Velocity0 = b.Momentum / b.Mass;
							//	b.Velocity = b.Velocity0;
							//	b.Accelerate = -b.Velocity;
							//	temp = b.Accelerate;
							//	temp.Normalize();
							//	b.Accelerate = temp;
							//	b.Accelerate *= b.Friction;
							//}
							
							//{
							//	body.Time = 0;
							//	body.Velocity0 = body.Momentum / body.Mass;
							//	body.Velocity = body.Velocity0;
							//	body.Accelerate = -body.Velocity;
							//	temp = body.Accelerate;
							//	temp.Normalize();
							//	body.Accelerate = temp;
							//	body.Accelerate *= body.Friction;
							//}
							
							//break;
							//MoveEllipse(b.Figure, new Thickness(b.X - b.Radius, 0, 0, b.Y - b.Radius));
							//return;
						}
					}
				}



				//Out.WriteLine(Velocity);
				//if (Velocity.LengthSquared < 1) {
				//	timer.Stop(); return; }

				//Velocity *= Friction;

				//X += Velocity.X / Fps;// * KV;
				//Y += Velocity.Y / Fps;// * KV;
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
