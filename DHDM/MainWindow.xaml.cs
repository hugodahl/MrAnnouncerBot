﻿using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using DndCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeLineControl;
using DndUI;

namespace DHDM
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ScrollPage activePage = ScrollPage.main;
		public MainWindow()
		{
			InitializeComponent();
			FocusHelper.FocusedControlsChanged += FocusHelper_FocusedControlsChanged;
		}

		public int PlayerID
		{
			get
			{
				return tabPlayers.SelectedIndex;
			}
		}

		private void FocusHelper_FocusedControlsChanged(object sender, FocusedControlsChangedEventArgs e)
		{
			foreach (StatBox statBox in e.Active)
			{
				HubtasticBaseStation.FocusItem(PlayerID, activePage, statBox.FocusItem);
			}

			foreach (StatBox statBox in e.Deactivated)
			{
				HubtasticBaseStation.UnfocusItem(PlayerID, activePage, statBox.FocusItem);
			}
		}

		private void TabControl_PlayerChanged(object sender, SelectionChangedEventArgs e)
		{
			activePage = ScrollPage.main;
			FocusHelper.ClearActiveStatBoxes();
			HubtasticBaseStation.PlayerDataChanged(PlayerID, activePage, string.Empty);
		}

		void ConnectToHub()
		{

		}

		private void CharacterSheets_PageChanged(object sender, RoutedEventArgs ea)
		{
			if (sender is CharacterSheets characterSheets && activePage != characterSheets.Page)
			{
				activePage = characterSheets.Page;
				HubtasticBaseStation.PlayerDataChanged(tabPlayers.SelectedIndex, activePage, string.Empty);
			}
		}

		private void btnSampleEffect_Click(object sender, RoutedEventArgs e)
		{
			AnimationEffect animationEffect = new AnimationEffect("DenseSmoke", new VisualEffectTarget(960, 1080), 0, 220, 100, 100);
			string serializedObject = JsonConvert.SerializeObject(animationEffect);
			HubtasticBaseStation.TriggerEffect(serializedObject);
		}

		private void HandleCharacterChanged(object sender, RoutedEventArgs e)
		{
			if (sender is CharacterSheets characterSheets)
			{
				string character = characterSheets.GetCharacter();
				HubtasticBaseStation.PlayerDataChanged(tabPlayers.SelectedIndex, activePage, character);
			}
		}

		Effect GetEffect(EffectEntry effectEntry)
		{
			if (effectEntry == null)
				return null;
			if (effectEntry.EffectKind == EffectKind.Animation)
				return effectEntry.AnimationEffect;
			if (effectEntry.EffectKind == EffectKind.Emitter)
				return effectEntry.EmitterEffect;
			if (effectEntry.EffectKind == EffectKind.SoundEffect)
				return effectEntry.SoundEffect;
			return null;
		}

		private void BtnTestGroupEffect_Click(object sender, RoutedEventArgs e)
		{
			EffectGroup effectGroup = new EffectGroup();
			foreach (TimeLineEntry timeLineEntry in groupEffectBuilder.Entries)
			{
				Effect effect = null;

				if (timeLineEntry.Data is EffectEntry entry)
					effect = GetEffect(entry);
				else if (timeLineEntry.Data is EffectPlaceholderEntry effectPlaceholder)
					effect = new PlaceholderEffect(effectPlaceholder.Name, effectPlaceholder.Type);

				if (effect != null)
				{
					effect.timeOffsetMs = (int)Math.Round(timeLineEntry.Start.TotalMilliseconds);
					effectGroup.Add(effect);
				}
			}

			string serializedObject = JsonConvert.SerializeObject(effectGroup);
			HubtasticBaseStation.TriggerEffect(serializedObject);
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.RemovedItems != null && e.RemovedItems.Count > 0)
				if (e.RemovedItems[0] is TabItem tabItem)
					if (tabItem == tbEffects)
						effectsList.Save();
					else if (tabItem == tbItems)
						lstItems.Save();
		}
	}
}
