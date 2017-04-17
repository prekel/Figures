// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGeometry.Core;

namespace Figures.Core
{
	public class Forces : List<Vector>, IForces
	{
		public Vector Resultant => new Vector(this.Sum(i => i.X), this.Sum(i => i.Y));

		public Forces()
		{
		}

		public Forces(IEnumerable<Vector> forces) => AddRange(forces);

		public Forces(params Vector[] forces) => AddRange(forces);
	}
}
