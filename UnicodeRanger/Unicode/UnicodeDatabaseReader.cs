using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UnicodeRanger.Unicode
{
	public static class UnicodeDatabaseReader
	{
		private static readonly Regex LineRegex = new Regex(@"(?<code>.+):'\[(?<points>.+)]',", RegexOptions.IgnoreCase);
		private static readonly Regex PointRegex = new Regex(@"\\u(?<point>[\da-f]+)(?<isRange>-)?", RegexOptions.IgnoreCase);

		public static UnicodeDatabase Read()
		{
			return new UnicodeDatabase(
				new Version(5, 0),
				GetClasses(GetLines()));
		}

		private static IEnumerable<string> GetLines()
		{
			var type = typeof(UnicodeDatabaseReader);

			using (var stream = type.Assembly.GetManifestResourceStream(type.Namespace + ".Unicode_5.0.txt"))
			using (var reader = new StreamReader(stream))
			{
				while (reader.Peek() >= 0)
				{
					yield return reader.ReadLine();
				}
			}
		}

		private static IEnumerable<UnicodeClass> GetClasses(IEnumerable<string> lines)
		{
			foreach (var line in lines)
			{
				var lineMatch = LineRegex.Match(line);

				if (lineMatch.Success)
				{
					var ranges = Range.Coalesce(GetRanges(PointRegex.Match(lineMatch.Groups["points"].Value)));

					yield return new UnicodeClass(
						lineMatch.Groups["code"].Value,
						ranges);
				}
			}
		}

		private static IEnumerable<Range<int>> GetRanges(Match match)
		{
			while (match.Success)
			{
				var point1 = Convert.ToInt32(match.Groups["point"].Value, 16);

				if (match.Groups["isRange"].Success)
				{
					match = match.NextMatch();

					var point2 = Convert.ToInt32(match.Groups["point"].Value, 16);

					yield return new Range<int>(point1, point2);
				}
				else
				{
					yield return new Range<int>(point1, point1);
				}

				match = match.NextMatch();
			}
		}
	}
}
