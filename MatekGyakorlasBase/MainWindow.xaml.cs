using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Linq;

namespace MatekGyakorlas
{
    public partial class MainWindow : Window
    {
        int ParameterNum = 2;
        char Operator = '+';
        string Name = "";
        int EqNum = 10;
        int MaxResult = 100;
        bool[] EqCorrect;
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
            EqCorrect = new bool[EqNum];
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
                l.Content = Equations[i].NewEquation(ParameterNum, Operator, MaxResult);
                Results[i] = Equations[i].Result;
                s.Children.Add(l);
                s.Margin = new Thickness(5);
                s.Children.Add(TxResults[i]);
                TxResults[i].TextChanged += MainWindow_TextChanged;
                TxResults[i].PreviewTextInput += NumberValidationTextBox;
                if (i >= 0 && i <= 4)
                    EqColumn1.Children.Add(s);
                else if (i >= 5 && i <= 9)
                    EqColumn2.Children.Add(s);
                else
                    EqColumn3.Children.Add(s);
            }
            TxResults[0].Focus();
        }

        private void MainWindow_TextChanged(object sender, TextChangedEventArgs e)
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
                    EqCorrect[CurrentEq] = true;
                    t.Background = Brushes.MediumSeaGreen;
                    t.Foreground = Brushes.White;
                }
                else
                {
                    EqCorrect[CurrentEq] = false;
                    t.Background = Brushes.Firebrick;
                    t.Foreground = Brushes.White;
                }
            }
            catch { }
            int EqCorrectNum = EqCorrect.Where(c => c).Count();
            if (EqCorrectNum == EqNum)
            {
                LabelStatus.Background = Brushes.MediumSeaGreen;
                MessageBox.Show("Gartulálok " + Name + "!\nSzép eredmény!");
                string fileName = @"eredmeny.txt";
                try
                {
                    string content = File.ReadAllText(fileName);
                    int EqCorrectNumber = EqCorrect.Where(c => c).Count();
                    content = DateTime.Now.ToString("yyyy.MM.dd HH:mm ") + Name + " eredménye " + EqNum + "/" + EqCorrectNumber + "\n" + content;
                    File.WriteAllText(fileName, content);
                    ResultsFromFile.Text = content;
                }
                catch
                {
                    MessageBox.Show("Az eredményeket nem tudtuk menteni.");
                }


                NewGame();
            }
            LabelStatus.Content = "Aktuális eredményed: " + EqNum + "/" + EqCorrectNum;
        }

        /// <summary>
        /// Validates the textbox to contain only numbers 
        /// </summary>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[a-zA-Z]*$"); //Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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



        private void ButtonOperator_Click(object sender, RoutedEventArgs e)
        {
            if ((Button)sender == ButtonDivide)
            {
                ButtonDivide.Background = Brushes.MediumSeaGreen;
                ButtonPlus.Background = ButtonMultiply.Background = ButtonMinus.Background = Brushes.Gray;
                Operator = '%';
            }
            else if (sender == ButtonPlus)
            {
                ButtonPlus.Background = Brushes.MediumSeaGreen;
                ButtonDivide.Background = ButtonMultiply.Background = ButtonMinus.Background = Brushes.Gray;
                Operator = '+';
            }
            else if (sender == ButtonMinus)
            {
                ButtonMinus.Background = Brushes.MediumSeaGreen;
                ButtonDivide.Background = ButtonMultiply.Background = ButtonPlus.Background = Brushes.Gray;
                Operator = '-';
            }
            else
            {
                ButtonMultiply.Background = Brushes.MediumSeaGreen;
                ButtonDivide.Background = ButtonMinus.Background = ButtonPlus.Background = Brushes.Gray;
                Operator = '*';
            }
        }
        private void txtMaxResult_TextChanged(object sender, TextChangedEventArgs e)
        {
            try {
                MaxResult = Int32.Parse(txtMaxResult.Text);
            }
            catch { }
        }

        private void SliderParameter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try {
                ParameterNum = (int)SliderParameter.Value;
                LabelParameter.Content = SliderParameter.Value.ToString();
            }
            catch { }
        }
    }
}

