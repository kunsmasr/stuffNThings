using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuffNThings.Utilities
{
	public static class ExtensionMethods
	{
		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			return !source.Any();
		}
	}
}