// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyGeometry.Core;

namespace Figures.Core
{
	public class Forces : Dictionary<string, Vector>, IForces
	{
		public Vector Resultant => new Vector(this.Sum(i => i.Value.X), this.Sum(i => i.Value.Y));

		public Forces()
		{
		}

		//public Forces(IEnumerable<Vector> forces) => 

		//public Forces(params Vector[] forces) => AddRange(forces);
	}
}
