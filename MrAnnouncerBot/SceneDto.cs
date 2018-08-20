﻿using System;

namespace MrAnnouncerBot
{
	public class SceneDto
	{
		double? minMinutesToSame;
		public string ChatShortcut { get; set; }
		public string AlternateShortcut { get; set; }
		public string Category { get; set; }
		public string SceneName { get; set; }
		public string LevelStr { get; set; }
		public string MinSpanToSameStr { get; set; }
		public string LimitToUser { get; set; }

		public double MinMinutesToSame { get
			{
				if (minMinutesToSame.HasValue)
					return minMinutesToSame.Value;
				double result;
				if (double.TryParse(MinSpanToSameStr, out result))
				{
					minMinutesToSame = result;
					return minMinutesToSame.Value;
				}

				return 0.5;
			}
		}

		public bool Matches(string command)
		{
			return string.Compare(ChatShortcut, command, StringComparison.OrdinalIgnoreCase) == 0;
		}
	}
}