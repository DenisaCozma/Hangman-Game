using Spanzuratoare.ViewModels;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spanzuratoare.Views
{
    public partial class Statistics : Window
    {
        public Statistics(MainWindowMV mainMV)
        {
            InitializeComponent();
            DataContext = new StatisticsMV(mainMV);
        }
    }
}
