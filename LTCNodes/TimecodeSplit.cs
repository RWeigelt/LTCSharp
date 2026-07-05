using System.Collections.Generic;
using LTCSharp;

namespace LTC.Nodes
{
	public class TimecodeSplit
	{
		public List<string> OutTimeZone { get; private set; } = new List<string>();
		public List<int> OutYear { get; private set; } = new List<int>();
		public List<int> OutMonth { get; private set; } = new List<int>();
		public List<int> OutDay { get; private set; } = new List<int>();
		public List<int> OutHours { get; private set; } = new List<int>();
		public List<int> OutMinutes { get; private set; } = new List<int>();
		public List<int> OutSeconds { get; private set; } = new List<int>();
		public List<int> OutFrame { get; private set; } = new List<int>();

		public void Split(IList<Timecode> timecodes)
		{
			int count = timecodes.Count;

			OutTimeZone = new List<string>(count);
			OutYear    = new List<int>(count);
			OutMonth   = new List<int>(count);
			OutDay     = new List<int>(count);
			OutHours   = new List<int>(count);
			OutMinutes = new List<int>(count);
			OutSeconds = new List<int>(count);
			OutFrame   = new List<int>(count);

			for (int i = 0; i < count; i++)
			{
				var timecode = timecodes[i];

				if (timecode == null)
				{
					OutTimeZone.Add(""); OutYear.Add(0);  OutMonth.Add(0);   OutDay.Add(0);
					OutHours.Add(0);    OutMinutes.Add(0); OutSeconds.Add(0); OutFrame.Add(0);
				}
				else
				{
					OutTimeZone.Add(timecode.TimeZone); OutYear.Add(timecode.Years);
					OutMonth.Add(timecode.Months);      OutDay.Add(timecode.Days);
					OutHours.Add(timecode.Hours);       OutMinutes.Add(timecode.Minutes);
					OutSeconds.Add(timecode.Seconds);   OutFrame.Add(timecode.Frame);
				}
			}
		}
	}
}

