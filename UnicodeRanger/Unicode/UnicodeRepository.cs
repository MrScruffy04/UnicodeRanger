using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnicodeRanger.Unicode
{
	public class UnicodeRepository
	{
		private static readonly UnicodeDatabase Database = UnicodeDatabaseReader.Read();

		public IEnumerable<UnicodeClass> GetByCode(string code)
		{
			if (code.Length < 1 || 2 < code.Length)
			{
				throw new ArgumentException("code must be one or two characters.", "code");
			}

			if (code.Length == 1)
			{
				return Database.Classes
					.Where(cl => cl.Code.StartsWith(code, StringComparison.OrdinalIgnoreCase));
			}

			return Database.Classes
				.Where(cl => cl.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
		}
	}
}
