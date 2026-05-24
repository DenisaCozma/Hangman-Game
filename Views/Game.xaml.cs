using Spanzuratoare.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spanzuratoare.Views
{
    public partial class Game : Window
    {
        public Game(MainWindowMV mainMV)
        {
            InitializeComponent();
            DataContext = new GameMV(mainMV);
        }
    }
}
