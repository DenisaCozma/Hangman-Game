using Spanzuratoare.Commands;
using Spanzuratoare.Models;
using Spanzuratoare.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Spanzuratoare.Models;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using static System.Net.Mime.MediaTypeNames;

namespace Spanzuratoare.ViewModels
{
    public class StatisticsMV : BaseClass
    {
        public ObservableCollection<StatItems> Stats { get; set; }

        public StatisticsMV(MainWindowMV mainMV)
        {
            Stats = new ObservableCollection<StatItems>();

            foreach (var user in mainMV.Users)
            {
                foreach (var category in user.GamesPerCategory.Keys)
                {
                    Stats.Add(new StatItems
                    {
                        UserName = user.Name,
                        Category = category,
                        Games = user.GamesPerCategory[category],
                        Wins = user.WinsPerCategory.ContainsKey(category)
                            ? user.WinsPerCategory[category]
                            : 0
                    });
                }
            }
        }
    }
}
