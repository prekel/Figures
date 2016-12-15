using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
using System.Diagnostics;

namespace Figures
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        int N = 500, n = 0;
		Body[] bodies = new Body[500];
        bool F;

		//Stopwatch clock;

		public MainWindow()
		{
			InitializeComponent();

            //Grid.MouseUp += Grid_MouseUp;
            MouseUp += MainWindow_MouseUp;

			//var disp = Dispatcher;

			//var r = (1280 / 325.0) * 25 / 2;
			var r = 50;
			var m = 10 / 1000.0;
			var mu = 0.1;
			var ck = 40;
            var M = 40;
			//var n = 12;
            //var N = 50;
			//var bodies = new Body[N];

			for (var i = 0; i < M / 2; i++)
			{
				bodies[i] = new Body(150 + i * 105, 150, r, m, mu, ck, Brushes.AliceBlue, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
                n++;
			}

			for (var i = M / 2; i < M; i++)
			{
				bodies[i] = new Body(150 + (i - M / 2) * 105, 650, r, m, mu, ck, Brushes.Tomato, Brushes.DarkOliveGreen);
				Grid.Children.Add(bodies[i].Figure);
                n++;
			}

			var fps = 100;
			//clock = new Stopwatch();
			//clock.Start();

			var cycle = new Thread(new ParameterizedThreadStart(Body.Move));
			cycle.Start(new object[] { bodies, Dispatcher, fpsCount, fps } );

		}

        void MainWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (F)
            {
                bodies[n] = new Body(e.GetPosition(Grid).X, e.GetPosition(Grid).Y, 50, 10 / 1000.0, 0.1, 40, Brushes.Cyan, Brushes.DarkOliveGreen);
                Grid.Children.Add(bodies[n].Figure);
                n++;
            }
        }

        //void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (F)
        //    {
        //        bodies[n] = new Body(e.GetPosition(Grid).X, e.GetPosition(Grid).Y, 50, 10/1000.0, 0.1, 40, Brushes.Cyan, Brushes.DarkOliveGreen);
        //        Grid.Children.Add(bodies[n].Figure);
        //        n++;
        //    }
        //}

		public Vector Norm(Vector a)
		{
			var b = a;
			b.Normalize();
			return b;
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            F = (F)?false:true;
        }

		//public void Step(object sender, ElapsedEventArgs e)
		//public void Step()
		//{
		//	var spf = 15.0;
		//	var fps = 1000.0 / spf;
		//	foreach (var b in bodies)
		//	{
		//		if (b.Momentum.LengthSquared == 0)
		//		//if (b.Velocity.LengthSquared >= b.Velocity0.LengthSquared)
		//		{
		//			continue;
		//		}
		//		b.Time += spf;

		//		var velocityD = b.Accelerate * (b.Time * b.Time * 0.5);
		//		//Остановка при маленькой скорости
		//		if (b.Velocity0.LengthSquared < velocityD.LengthSquared)
		//		{
		//			//timer.Stop();
		//			b.Velocity = new Vector(0, 0);
		//			b.Momentum = b.Velocity;
		//			continue;
		//		}

		//		b.Velocity = b.Momentum / b.Mass;
		//		b.Velocity = b.Velocity0 + velocityD;
		//		b.Momentum = b.Velocity * b.Mass;

		//		b.X += b.Velocity.X / fps;
		//		b.Y += b.Velocity.Y / fps;
		//	}
		//	foreach (var b in bodies)
		//	{
		//		if (b.Momentum.LengthSquared == 0)
		//		//if (b.Velocity.LengthSquared >= b.Velocity0.LengthSquared)
		//		{
		//			continue;
		//		}
		//		//Столкновение
		//		//var VelocityEps = Velocity.LengthSquared / Fps / KV * 3;
		//		//var velocityEps = Velocity.LengthSquared / Fps / KV;
		//		//var velocityEps = velocityD.Length;
		//		//var velocityEps2 = velocityD.LengthSquared;
		//		//var velocityEps2 = velocityEps * velocityEps;
		//		//var VelocityEps = 0;
		//		foreach (var body in bodies)
		//		{
		//			if (body != b)// && b.Momentum.LengthSquared > 0)
		//			{
		//				//var m = Math.Sqrt((X - body.X) * (X - body.X) + (Y - body.Y) * (Y - body.Y));
		//				var m2 = (b.X - body.X) * (b.X - body.X) + (b.Y - body.Y) * (b.Y - body.Y);
		//				var r = b.Radius + body.Radius;
		//				var r2 = r * r;
		//				if (m2 <= r2)
		//				{
		//					var vb = new Vector(body.X - b.X, body.Y - b.Y);
		//					vb.Normalize();
		//					vb *= Math.Sqrt(m2) - r;
		//					b.X += vb.X;
		//					b.Y += vb.Y;
		//					body.Momentum = new Vector(body.X - b.X, body.Y - b.Y);

		//					var temp = body.Momentum;
		//					temp.Normalize();
		//					body.Momentum = temp;

		//					body.Momentum *= b.Momentum.Length * Math.Abs(Math.Cos(Vector.AngleBetween(b.Momentum, body.Momentum) * 180 / Math.PI));

		//					b.Momentum = b.Momentum - body.Momentum;

		//					b.StartMoveByMomentum1(b.Momentum);
		//					body.StartMoveByMomentum1(body.Momentum);
		//					//break;
		//					goto jkl;
		//				}
		//			}
		//		}
		//	}

		//	jkl:;
		//	foreach (var b in bodies)
		//	{
		//		if (b.Momentum.LengthSquared < 1)
		//		{
		//			continue;
		//		}
		//		MoveEllipse(b.Figure, new Thickness(b.X - b.Radius, 0, 0, b.Y - b.Radius));
		//	}
		//}
		//public void MoveEllipse(Ellipse el, Thickness margin)
		//{
		//	Action action = () => el.Margin = margin;
		//	try { Dispatcher.Invoke(action); }
		//	catch { /*ignored*/ }
		//}
	}
}
