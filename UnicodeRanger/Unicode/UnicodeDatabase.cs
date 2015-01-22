using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnicodeRanger.Unicode
{
	public class UnicodeDatabase
	{
		public UnicodeDatabase(Version version, IEnumerable<UnicodeClass> classes)
		{
			Version = version;

			Classes = classes.ToList();
		}

		public Version Version { get; private set; }

		public IList<UnicodeClass> Classes { get; private set; }
	}
}
