using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicodeRanger.Unicode;

namespace UnicodeRanger
{
	class Program
	{
		private static UnicodeRepository _repo = new UnicodeRepository();

		[STAThread]
		static void Main(string[] args)
		{
			Console.Write("Enter class codes (one or two characters, case-insensitive, comma separated): ");

			var codes = (Console.ReadLine() ?? String.Empty).Trim().Split(',', ' ');

			var ranges = 
				CoalesceAdjacentRanges(
				Range.Sort(
				Range.Coalesce(
					codes
						.SelectMany(code => _repo.GetByCode(code))
						.SelectMany(cl => cl.Ranges))));

			System.Windows.Forms.Clipboard.SetText(BuildRangeOutput(ranges));

			Console.WriteLine("The unicode character points have been copied to the clipboard.");

			Console.ReadKey(true);
		}

		private static IEnumerable<Range<int>> CoalesceAdjacentRanges(IEnumerable<Range<int>> ranges)
		{
			/*
			 * This method only works because we understand the integral nature of the Unicode character points.
			 * Range<T> can't do this because it doesn't know the range is integral, and it would have no clean 
			 * way to achieve step 1 below.
			 * 
			 * 1. Given a Range<T> (range1), find the value immediately after range1.UpperBound
			 * 2. Given the next Range<T> (range2), check if that value equals range2.LowerBound
			 * 3. If the values are equal, the ranges can be combined
			 */

			var list = ranges.ToList();

			var dirty = false;

			for (int i = 0, len = list.Count; i < len; i++)
			{
				if (i + 1 < len)
				{
					var range1 = list[i];
					var range2 = list[i + 1];

					if (range1.UpperBound + 1 == range2.LowerBound)
					{
						list[i] = new Range<int>(range1.LowerBound, range2.LowerBound);

						dirty = true;
					}
				}
			}

			if (!dirty)
			{
				return list;
			}

			return Range.Coalesce(list);
		}

		private static string BuildRangeOutput(IEnumerable<Range<int>> ranges)
		{
			var list = ranges.ToList();

			var sb = new StringBuilder(list.Count);

			foreach (var range in list)
			{
				if (range.LowerBound == range.UpperBound)
				{
					sb.Append(GetPointString(range.LowerBound));
				}
				else
				{
					sb.AppendFormat("{0}-{1}", GetPointString(range.LowerBound), GetPointString(range.UpperBound));
				}
			}

			return sb.ToString();
		}

		private static string GetPointString(int point)
		{
			return @"\u" + point.ToString("x4");
		}
	}
}
