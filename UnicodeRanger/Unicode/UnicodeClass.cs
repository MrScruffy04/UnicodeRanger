using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnicodeRanger.Unicode
{
	public class UnicodeClass
	{
		public UnicodeClass(string code, IEnumerable<Range<int>> ranges)
		{
			Code = code;

			Ranges = ranges.ToList();
		}

		public string Code { get; private set; }

		public IList<Range<int>> Ranges { get; private set; }
	}
}
