using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Inventory : Page
    {
        public Inventory()
        {
            this.InitializeComponent();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        public static string AWS_Name { get; set; }

        public void GetAWS()
        {

            var Var2 = new Amazon.Auth.AccessControlPolicy.Policy();
            var AWS_Name = Convert.ToString(Var2.Version);
            // Create the message dialog and set its content
            GetAPI.Text += "API: " + AWS_Name;
        }

        private void BtnGetAPI_Click(object sender, RoutedEventArgs e)
        {
            GetAWS();
        }

    }
}
