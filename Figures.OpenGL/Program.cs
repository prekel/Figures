// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using MyGeometry.Core;
using MyGeometry.Draw;

using Figures.Core;

using NLog;

namespace MyGeometry.Draw.Example
{
	public class Program
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private GameWindow game;
		private readonly Stopwatch t;
		public double l = 0.01;
		private double c => t.Elapsed.TotalSeconds * l;
		//private double c { get; set; }
		private double r = 1d;
		private int k = 4;
		double[] listX;
		double[] listY;
		double x, y;
		double rotate_x, rotate_y;
		DrawScene s;
		CircleBody Cir, BigCir;

		private void OnGameOnLoad(object sender, EventArgs e)
		{
			game.VSync = VSyncMode.On;

			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.Enable(EnableCap.Multisample);

			GL.ClearColor(System.Drawing.Color.White);
		}

		private void OnGameOnResize(object sender, EventArgs e)
		{
			GL.Viewport(0, 0, game.Width, game.Height);
		}

		private void OnGameOnUpdateFrame(object sender, FrameEventArgs e)
		{
			if (game.Keyboard[Key.Escape])
			{
				game.Exit();
			}
			if (game.Keyboard[Key.Up])
			{
				k++;
				listX = new double[k];
				listY = new double[k];
			}
			if (game.Keyboard[Key.Down])
			{
				k = Math.Max(3, k - 1);
				listX = new double[k];
				listY = new double[k];
			}
			if (game.Keyboard[Key.W])
			{
				y += 0.01;
				//p.Y += 0.01;
				//((CircleBody)s[2]).Velocity.Y += l;
				//((CircleBody)s[2]).Accelerate.Y += l;
				Cir.Forces["Main"].Y += l;
			}
			if (game.Keyboard[Key.S])
			{
				y -= 0.01;
				//p.Y -= 0.01;
				//((CircleBody)s[2]).Velocity.Y -= l;
				//((CircleBody)s[2]).Accelerate.Y -= l;
				Cir.Forces["Main"].Y -= l;
			}
			if (game.Keyboard[Key.A])
			{
				x -= 0.01;
				//p.X -= 0.01;
				//((CircleBody)s[2]).Velocity.X -= l;
				//((CircleBody)s[2]).Accelerate.X -= l;
				Cir.Forces["Main"].X -= l;
			}
			if (game.Keyboard[Key.D])
			{
				x += 0.01;
				//p.X += 0.01;
				//((CircleBody)s[2]).Velocity.X += l;
				//((CircleBody)s[2]).Accelerate.X += l;
				Cir.Forces["Main"].X += l;
			}
			if (game.Keyboard[Key.M])
			{
				l += 0.0001; //p.Size += 0.1f;
			}
			if (game.Keyboard[Key.N])
			{
				l -= 0.0001; //p.Size -= 0.1f;
			}

			Cir.Step0();
			BigCir.Step0();
			Cir.Step(game.UpdatePeriod);
			BigCir.Step(game.UpdatePeriod);
			Cir.Move();
			BigCir.Move();

			for (var i = 0; i < k; i++)
			{
				listX[i] = r * Math.Cos(c + 2 * Math.PI / k * i);
				listY[i] = r * Math.Sin(c + 2 * Math.PI / k * i);
				//pol[i].X = r * Math.Cos(c + 2 * Math.PI / k * i);
				//pol[i].Y = r * Math.Sin(c + 2 * Math.PI / k * i);
			}

			//if (game.UpdateFrequency > 100)
			//{
			//	Debug.WriteLine(game.UpdateFrequency);
			//}

			//c += l / 10;
		}

		private void OnGameOnRenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			//GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
			GL.Ortho(s.Left, s.Right, s.Bottom, s.Top, 0.0, 4.0);

			DrawScene.Draw(s);

			game.Title = $"{game.RenderFrequency:N15} {game.UpdatePeriod:N15}";
			//game.Title = game.RenderFrequency.ToString(CultureInfo.InvariantCulture);
			//game.Title = $"{game.RenderTime:N8} {game.UpdateTime:N8}";
			//if (game.Title.Length > 25)
			//{
			//	//Debug.WriteLine(game.Title);
			//}

			game.SwapBuffers();
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			//Debug.WriteLine($"{e.Mouse.X} {e.Mouse.Y}");
			//var x1 = e.Mouse.X - game.Width / 2;
			//var y1 = -e.Mouse.Y + game.Height / 2;
			//Debug.WriteLine($"{x1} {y1}");
			//var x2 = x1 / (game.Width / 2.0);
			//var y2 = y1 / (game.Height / 2.0);
			//Debug.WriteLine($"{x2} {y2}");
			var p = IntCoordsToDouble(game.Mouse.X, game.Mouse.Y, game.Width, game.Height);
			s.Add(new Point
			{
				X = p.X,
				Y = p.Y,
				Color = System.Drawing.Color.Black,
				Size = 3
			});
			foreach (var i in s)
			{
				if (i is CircleBody j)
				{
					var r = j.Distance(p);
					if (r <= j.R)
					{
						if (e.Mouse.RightButton == ButtonState.Pressed)
						{
							j.Forces["Main"] = new Vector(0, 0);
							j.Velocity = new Vector(0, 0);
							//j.GravityForces = new GravityForces();
						}
						else
						{
							j.Click(p, r);
						}
					}
				}
			}
		}

		public static Point IntCoordsToDouble(int x, int y, int width, int height)
		{
			var x1 = x - width / 2;
			var y1 = -y + height / 2;
			//Debug.WriteLine($"{x1} {y1}");
			var x2 = x1 / (width / 2.0);
			var y2 = y1 / (height / 2.0);
			//Debug.WriteLine($"{x2} {y2}");
			return new Point(x2, y2);
		}

		public static void Main()
		{
			// ReSharper disable once UnusedVariable
			var program = new Program();
		}

		public Program()
		{
			var starttime = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff");
			LogManager.Configuration.Variables["starttime"] = starttime;

			game = new GameWindow(700, 700, new GraphicsMode(32, 24, 4, 1));

			game.Load += OnGameOnLoad;

			game.Resize += OnGameOnResize;

			game.UpdateFrame += OnGameOnUpdateFrame;

			game.RenderFrame += OnGameOnRenderFrame;

			game.MouseMove += OnGameMouseMove;

			game.MouseDown += OnMouseDown;

			t = new Stopwatch();
			t.Start();

			s = new DrawScene();

			var p = new Point
			{
				X = 0.6,
				Y = -0.2,
				Size = 6,
				Color = System.Drawing.Color.BlueViolet,
				Scene = s
			};

			var pol = new Polygon(4)
			{
				[0] = new Point(0.5, 0.5),
				[1] = new Point(-0.5, 0.5),
				[2] = new Point(-0.5, -0.5),
				[3] = new Point(0.5, -0.5),
				OutlineWidth = 2,
				IsOutline = true,
				ColorOutline = System.Drawing.Color.Navy,
				IsFill = true,
				ColorFill = System.Drawing.Color.CornflowerBlue,
				Scene = s
			};

			var cir = new CircleBody(0.1, 0.8, 0)
			{
				IsFill = true,
				ColorFill = System.Drawing.Color.LightGray,
				IsOutline = true,
				ColorOutline = System.Drawing.Color.DarkGray,
				OutlineWidth = 1,
				Mass = 1,
				Forces = new Forces
				{
					["Main"] = new Vector(0, 0),
					//["Friction"] = new Vector(0, 0)
				},
				Number = 1
			};

			var bc = new CircleBody(0.03, 0, 0)
			{
				IsFill = true,
				ColorFill = System.Drawing.Color.DarkSeaGreen,
				IsOutline = true,
				ColorOutline = System.Drawing.Color.DeepSkyBlue,
				OutlineWidth = 3,
				Mass = 10e8,
				Forces = new Forces
				{
					["Main"] = new Vector(0, 0),
					//["Friction"] = new Vector(0, 0)
				},
				Number = 2
			};

			var c1 = new CircleBody(0.2, -0.8, 0)
			{
				IsFill = true,
				ColorFill = System.Drawing.Color.LightGray,
				IsOutline = true,
				ColorOutline = System.Drawing.Color.DarkGray,
				OutlineWidth = 1,
				Mass = 1,
				Forces = new Forces
				{
					["Main"] = new Vector(0, 0),
					//["Friction"] = new Vector(0, 0)
				},
				Number = 1
			};

			var c2 = new CircleBody(0.2, 0.8, 0)
			{
				IsFill = true,
				ColorFill = System.Drawing.Color.DarkSeaGreen,
				IsOutline = true,
				ColorOutline = System.Drawing.Color.DeepSkyBlue,
				OutlineWidth = 1,
				Mass = 1,
				Forces = new Forces
				{
					["Main"] = new Vector(0, 0),
					//["Friction"] = new Vector(0, 0)
				},
				Number = 2
			};

			Cir = c1;
			BigCir = c2;
			//Cir = cir;
			//BigCir = bc;

			s.Add(p);
			s.Add(pol);
			s.Add(Cir);
			s.Add(BigCir);

			s.Left *= 1;
			s.Right *= 1;
			s.Bottom *= 1;
			s.Top *= 1;

			listX = new double[k];
			listY = new double[k];

			//game.Run(60, 60);
			game.Run(200);//, 200);
		}

		private void OnGameMouseMove(object sender, MouseMoveEventArgs e)
		{
			//rotate_x -= e.YDelta;
			//rotate_y += e.XDelta;
			//cir.X += e.XDelta / 500.0;
			//cir.Y -= e.YDelta / 500.0;
		}
	}
}