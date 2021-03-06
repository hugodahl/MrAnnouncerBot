﻿using System;
using System.Linq;

namespace DndCore
{
	public class DndAlarm
	{
		public DndAlarm(DndTimeClock dndTimeClock, DateTime triggerTime, string name, Character player, object data = null)
		{
			Player = player;
			Data = data;
			Name = name;
			TriggerTime = triggerTime;
			SetTime = dndTimeClock.Time;
		}

		public DateTime SetTime { get; set; }
		public DateTime TriggerTime { get; set; }
		public string Name { get; set; }
		public object Data { get; set; }
		public Character Player { get; set; }

		public void FireAlarm(DndTimeClock dndTimeClock)
		{
			AlarmFired?.Invoke(this, new DndTimeEventArgs(dndTimeClock, this));
		}

		public delegate void DndTimeEventHandler(object sender, DndTimeEventArgs ea);
		public event DndTimeEventHandler AlarmFired;
	}
}

