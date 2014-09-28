﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IllustrationGenerator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        Map map;
        List<Ant> ants;
        DispatcherTimer mapTimer;
        DispatcherTimer antTimer;
        DispatcherTimer nestTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }

        public int PheromoneCount
        {
            get
            {
                return map.PheromeneCount;
            }
        }

        public int AntCount
        {
            get
            {
                return ants.Count;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            map = new Map(Illustration1, SimulationParameters.CITY_COUNT);
            mapTimer = new DispatcherTimer();
            mapTimer.Tick += mapTimer_Tick;
            mapTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            antTimer = new DispatcherTimer();
            antTimer.Tick += antTimer_Tick;
            antTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            nestTimer = new DispatcherTimer();
            nestTimer.Tick += nestTimer_Tick;
            nestTimer.Interval = new TimeSpan(0, 0, 0, 0 ,500);
            ants = new List<Ant>();
            SentAnts();
        }

        void mapTimer_Tick(object sender, object e)
        {
            map.RefreshPheromones();
            RaisePropertyChanged("PheromoneCount");
            RaisePropertyChanged("AntCount");
        }

        void nestTimer_Tick(object sender, object e)
        {
            SentAnts();
            RaisePropertyChanged("PheromoneCount");
            RaisePropertyChanged("AntCount");
        }

        void SentAnts()
        {
            foreach (var city in map.Cities)
            {
                ants.Add(new JustantAnt(city, map));
            }
        }

        void antTimer_Tick(object sender, object e)
        {
            // Kill all ants without pheromone
            ants.RemoveAll(a => !a.CheckPheromone());
            // Move each ant
            foreach (var ant in ants)
                ant.Move();
            // Update
            RaisePropertyChanged("PheromoneCount");
            RaisePropertyChanged("PathCount");
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            nestTimer.Start();
            antTimer.Start();
            mapTimer.Start();
        }
    }
}
