using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Popups;
using System.Linq;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace School_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //store values
        List<string> ValuesArray = new List<string>();
        List<string> Term = new List<string>();
        List<double> SavedTerm = new List<double>();
        List<char> Operands = new List<char>();
        //numbers
        double Result;
        double Number_1 = 10, Number_2 = 10, Number_3 = 10, Number_4 = 10, Number_5 = 10, Number_6 = 10, Number_7 = 10, Number_8 = 10, Number_9 = 10, Number_0 = 10;
        //strings
        string Number;
        //operators
        string Plus, Minus, Times, Divition, Reminder;
        //main initialization
        public MainPage()
        {
            this.InitializeComponent();
            BL_PageContent.CreatedBy = "Created By: Leonel Gonzalez";
            txtBoxFooter.Text = BL_PageContent.CreatedBy;

        }
        //number click set the number to value and display
        private void button_NumberOne_Click(object sender, RoutedEventArgs e)
        {
            Number_1 = 1;
            Number = Number_1.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberTwo_Click(object sender, RoutedEventArgs e)
        {
            Number_2 = 2;
            Number = Number_2.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberThree_Click(object sender, RoutedEventArgs e)
        {
            Number_3 = 3;
            Number = Number_3.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberFour_Click(object sender, RoutedEventArgs e)
        {
            Number_4 = 4;
            Number = Number_4.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }

        private void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            txtBoxDisplay.Text = "";
            listView.Items.Clear();
            ValuesArray.Clear();
            SavedTerm.Clear();
            Operands.Clear();
            Result = 0;
        }

        
        async private void BtnCourse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog(""); //init dialog object
            var limit = listViewCourses.Items.Count >= 5 ? true : false; //max of 5 items in list
            if (limit) //maximium reached?
            {
                listViewCourses.Items.Clear(); //clear the list
            }
            String name = null;
            if (sender is Button) //get the sender's name
            {
                name = (sender as Button).Name;
                BL_PageContent.VarOutput = "";
                switch (name) // case statement for the name, do things depending on the button clicked
                {
                    case "btnCourse1":
                        BL_PageContent.CourseCredits(1);
                        listViewCourses.Items.Add(BL_PageContent.VarOutput);
                        dialog = new MessageDialog(BL_PageContent.VarOutput);
                        await dialog.ShowAsync();
                        break;
                    case "btnCourse2":
                        BL_PageContent.CourseCredits(2);
                        listViewCourses.Items.Add(BL_PageContent.VarOutput);
                        dialog = new MessageDialog(BL_PageContent.VarOutput);
                        await dialog.ShowAsync();
                        break;
                    case "btnCourse3":
                        BL_PageContent.CourseCredits(3);
                        listViewCourses.Items.Add(BL_PageContent.VarOutput);
                        dialog = new MessageDialog(BL_PageContent.VarOutput);
                        await dialog.ShowAsync();
                        break;
                    default:
                        listViewCourses.Items.Clear();
                        break;
                }
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Faculty));
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserAuth));
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Inventory));
        }

        //number click set the number to value and display
        private void button_NumberFive_Click(object sender, RoutedEventArgs e)
        {
            Number_5 = 5;
            Number = Number_5.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberSix_Click(object sender, RoutedEventArgs e)
        {
            Number_6 = 6;
            Number = Number_6.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberSeven_Click(object sender, RoutedEventArgs e)
        {
            Number_7 = 7;
            Number = Number_7.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberEight_Click(object sender, RoutedEventArgs e)
        {
            Number_8 = 8;
            Number = Number_8.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button__NumberNine_Click(object sender, RoutedEventArgs e)
        {
            Number_9 = 9;
            Number = Number_9.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //number click set the number to value and display
        private void button_NumberZero_Click(object sender, RoutedEventArgs e)
        {
            Number_0 = 0;
            Number = Number_0.ToString();
            listView.Items.Add(Number);
            ValuesArray.Add(Number);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //Plus click add and display
        private void button_Plus_Click(object sender, RoutedEventArgs e)
        {
            Plus = "+";
            listView.Items.Add(Plus);
            ValuesArray.Add(Plus);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //Minus click Substract and display
        private void button_Minus_Click(object sender, RoutedEventArgs e)
        {
            Minus = "-";
            listView.Items.Add(Minus);
            ValuesArray.Add(Minus);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //Times click multiply and display
        private void button_Times_Click(object sender, RoutedEventArgs e)
        {
            Times = "*";
            listView.Items.Add(Times);
            ValuesArray.Add(Times);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //Divition click divide and display
        private void button_Divition_Click(object sender, RoutedEventArgs e)
        {
            Divition = "/";
            listView.Items.Add(Divition);
            ValuesArray.Add(Divition);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //Reminder click get reminder and display
        private void button_Reminder_Click(object sender, RoutedEventArgs e)
        {
            Reminder = "%";
            listView.Items.Add(Reminder);
            ValuesArray.Add(Reminder);
            txtBoxDisplay.Text = string.Join("", ValuesArray);
        }
        //enter and display to main
        private void ProcessCalc(object sender, RoutedEventArgs e)
        {
            txtBoxDisplay.Text = ""; //clear screen
            string New_term;
            //First_Term Double
            //Term String
            // Loop over strings.
            int Size = ValuesArray.Count();
            int count = 0;
            System.Diagnostics.Debug.WriteLine("Size: " + Size); //debug
            foreach (string s in ValuesArray)
            {
                count++;
                if (s == "+" || s == "-" || s == "*" || s == "/" || s == "%")
                {
                    Operands.Add(Convert.ToChar(s)); //if found break
                    New_term = string.Join("", Term); //join all s found into one line
                    New_term.Trim();
                    double term_completed = Convert.ToDouble(New_term); //Convert to double
                    SavedTerm.Add(term_completed); // save double
                    Term.Clear(); //clear last term
                    New_term = "";
                }
                else
                {
                    Term.Add(s);//we are going to append as long as not arithmetic is found
                }

                System.Diagnostics.Debug.WriteLine("Count: " + count); //debug
                if (Size == count)
                {
                    System.Diagnostics.Debug.WriteLine("End of ValuesArray"); //debug
                    New_term = string.Join("", Term);
                    System.Diagnostics.Debug.WriteLine("Last Term: " + New_term); //debug
                    double term_completed = Convert.ToDouble(New_term); //Convert to double
                    SavedTerm.Add(term_completed); // save double
                    Term.Clear(); //clear last term
                    New_term = "";
                }
            }
            ResultCalc();
            System.Diagnostics.Debug.WriteLine(SavedTerm[0]); //debug
            System.Diagnostics.Debug.WriteLine(Operands[0]); //debug
            System.Diagnostics.Debug.WriteLine(SavedTerm[1]); //debug
            System.Diagnostics.Debug.WriteLine("Result: " + Result); //debug
            string final = string.Join("", ValuesArray);
            txtBoxDisplay.Text = final + "= " + Result;
        }
        private void ResultCalc()
        {
            System.Diagnostics.Debug.WriteLine("Result_CAll: True"); //debug

            for (int i = 0; i < SavedTerm.Count - 1; i++)
            {
                System.Diagnostics.Debug.WriteLine("LOOP: True"); //debug
                if (Operands[i] == '+')
                    Result += SavedTerm[i] + SavedTerm[i + 1];
                else if (Operands[i] == '-')
                    Result += SavedTerm[i] - SavedTerm[i + 1];
                else if (Operands[i] == '*')
                    Result += SavedTerm[i] * SavedTerm[i + 1];
                else if (Operands[i] == '/')
                    Result += SavedTerm[i] / SavedTerm[i + 1];
                else if (Operands[i] == '%')
                    Result += SavedTerm[i] % SavedTerm[i + 1];
            }
        }
    }
}
