using System;

namespace SERVER
{
	public class Point
	{
		public int X,Y;

		public Point ()
		{

		}

		public Point (int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Point Zero ()
		{
			return new Point(0, 0);
		}
	}
}

