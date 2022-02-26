using System;
using System.Collections.Generic;
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

namespace ChiaK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            foreach (var item in Utils.GetAllDrives())
            {
                DriveList.Items.Add(item);
            }
        }

        private void DriveList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolutionK.Content = Utils.GetSolution(DriveList.SelectedIndex);
        }

        private void K33CountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            K33CountLabel.Content = ((int)e.NewValue).ToString();
            Utils.K33count = (int)e.NewValue;
            SolutionK.Content = Utils.GetSolution(DriveList.SelectedIndex);
        }

        private void K34CountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            K34CountLabel.Content = ((int)e.NewValue).ToString();
            Utils.K34count = (int)e.NewValue;
            SolutionK.Content = Utils.GetSolution(DriveList.SelectedIndex);
        }

        private void K32CountSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            K32CountLabel.Content = ((int)e.NewValue).ToString();
            Utils.K32count = (int)e.NewValue;
            SolutionK.Content = Utils.GetSolution(DriveList.SelectedIndex);
        }
    }
}
