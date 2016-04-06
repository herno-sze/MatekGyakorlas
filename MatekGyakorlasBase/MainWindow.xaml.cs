using System;
using System.Collections.Generic;
using System.IO;
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



namespace MatekGyakorlas
{
    public partial class MainWindow : Window
    {
        int ParameterNum = 2;
        char Operator = '+';
        string Name = "";
        int EqNum = 10;
        int EqCorrect = 0;
        int[] Results;
        Equation[] Equations;
        TextBox[] TxResults;


        public MainWindow()
        {
            InitializeComponent();
            TabJatek.Visibility = Visibility.Collapsed;
            TabBeallitasok.Visibility = Visibility.Collapsed;
            TabEredmenyek.Visibility = Visibility.Collapsed;
            TabStart.Visibility = Visibility.Collapsed;
            if (Name.Equals(""))
                TabMain.SelectedIndex = 0;
            else
                TabMain.SelectedIndex = 1;
            try
            {
                ResultsFromFile.Text = File.ReadAllText(@"eredmeny.txt");             
            }
            catch
            {
                try
                {
                    using (StreamWriter sw = File.CreateText(@"eredmeny.txt")) ;
                }
                catch
                {
                    MessageBox.Show("Az eredményeket nem tudtuk menteni.");
                }
            }
            NewGame();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {

        }

        private void SliderPossOpt_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                // vm.MyPossibleOpt = Int32.Parse(SliderPossOpt.Value.ToString());
                LabelPossibleOpt.Content = SliderPossOpt.Value.ToString();
                EqNum = (int)SliderPossOpt.Value;
                LabelStatus.Content = "A játék " + EqNum + " egyenletet fog genráni.";
            }
            catch
            {
                //vm.MyPossibleOpt = 4;
            }
        }

        private void MenuItem_NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void MenuItem_Result_Click(object sender, RoutedEventArgs e)
        {
            TabMain.SelectedIndex = 2;
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            TabMain.SelectedIndex = 3;
        }

        private void ButtonGoToGame_Click(object sender, RoutedEventArgs e)
        {
            TabMain.SelectedIndex = 1;
        }

        private void MenuItem_Credits(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("\u00a9 Horváthné Fejes Katalin \nachat.hu/suli");
        }

        private void MenuItem_Tipps(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nyomd a TAB billenytűt, hogy az egyik gyorsabban írhass egymás után eredményeket.");
        }

        void NewGame()
        {
            EqColumn1.Children.Clear();
            EqColumn2.Children.Clear();
            EqColumn3.Children.Clear();
            EqCorrect = 0;
            LabelStatus.Background = Brushes.DarkMagenta;
            Equations = new Equation[EqNum];
            Results = new int[EqNum];
            TxResults = new TextBox[EqNum];
            for (var i = 0; i < Equations.Length; i++)
            {
                Equations[i] = new Equation();
                Label l = new Label();
                StackPanel s = new StackPanel();
                TxResults[i] = new TextBox();
                s.Orientation = Orientation.Horizontal;
                TxResults[i].Width = 80;
                l.Content = Equations[i].NewEquation(2, '+');
                Results[i] = Equations[i].Result;
                s.Children.Add(l);
                s.Margin = new Thickness(5);
                s.Children.Add(TxResults[i]);
                TxResults[i].KeyUp += TxRestult_KeyUp;
                if (i >= 0 && i <= 4)
                    EqColumn1.Children.Add(s);
                else if (i >= 5 && i <= 9)
                    EqColumn2.Children.Add(s);
                else
                    EqColumn3.Children.Add(s);
            }
            TxResults[0].Focus();
        }

        void TxRestult_KeyUp(object sender, KeyEventArgs e)
        {
            int CurrentEq = 0;
            TextBox t = (TextBox)sender;
            for (var i = 0; i < Equations.Length; i++)
                if (t == TxResults[i])
                    CurrentEq = i;
            try
            {
                if (Results[CurrentEq] == Int32.Parse(t.Text))
                {
                    EqCorrect++;
                    t.Background = Brushes.MediumSeaGreen;
                    t.Foreground = Brushes.White;
                }
                else
                {
                    t.Background = Brushes.Firebrick;
                    t.Foreground = Brushes.White;
                }
            }
            catch { }
            if (EqCorrect == EqNum)
            {
                LabelStatus.Background = Brushes.MediumSeaGreen;
                MessageBox.Show("Gartulálok " + Name + "!\nSzép eredmény!");
                string fileName = @"eredmeny.txt";
                try
                {
                    string content = File.ReadAllText(fileName);
                    content = DateTime.Now.ToString("yyyy.MM.dd HH:mm ") + Name + " eredménye " + EqNum + "/" + EqCorrect + "\n" + content;
                    File.WriteAllText(fileName, content);
                    ResultsFromFile.Text = content;
                }
                catch
                {
                    MessageBox.Show("Az eredményeket nem tudtuk menteni.");
                }


                NewGame();
            }
            LabelStatus.Content = "Aktuális eredményed: " + EqNum + "/" + EqCorrect;
        }

        private void TxtNev_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtNev.Text.Equals(""))
                ButtonNevOk.Visibility = Visibility.Hidden;
            else
                ButtonNevOk.Visibility = Visibility.Visible;

        }

        private void TxtNev_KeyUp(object sender, KeyEventArgs e)
        {
            if (!TxtNev.Text.Equals("") && e.Key == Key.Enter)
            {
                TabMain.SelectedIndex = 1;
                Name = TxtNev.Text;
                LabelStatus.Content = "Üdvözöllek " + Name + "!";
            }
        }

        private void ButtonNevOk_Click(object sender, RoutedEventArgs e)
        {
            TabMain.SelectedIndex = 1;
            Name = TxtNev.Text;
            LabelStatus.Content = "Üdvözöllek " + Name + "!";
        }
    }
}
