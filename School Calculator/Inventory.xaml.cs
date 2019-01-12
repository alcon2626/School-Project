using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Popups;
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
        //AZURE
        public class Inventory_EasyTable
        {
            public string id { get; set; }
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public bool Complete { get; set; }
        }

        private IMobileServiceSyncTable<Inventory_EasyTable> todoGetTable = App.MobileService.GetSyncTable<Inventory_EasyTable>();


        private async Task SyncAsync()
        {
            await App.MobileService.SyncContext.PushAsync();
            await todoGetTable.PullAsync("Inventory_EasyTable", todoGetTable.CreateQuery());
        }

        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("Inventory_EasyTable");
                store.DefineTable<Inventory_EasyTable>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }
            await SyncAsync();
        }


        async private void Btn_AddItem_Click(object sender, RoutedEventArgs e)
        {
            await InitLocalStoreAsync();
            MessageDialog dialog;
            try
            {
                string itemname = await InputTextDialogAsync("Please enter item name.");
                string itemquatity = await InputTextDialogAsync("Please enter item quatity.");
                if (itemname == "" || itemquatity == ""){return;}
                if (!IsNumeric(itemquatity)) {
                    dialog = new MessageDialog("Please enter a numeric value and try again.");
                    await dialog.ShowAsync();
                    return;
                }
                Inventory_EasyTable itemReg = new Inventory_EasyTable
                {
                    ItemName = itemname,
                    Quantity = Int32.Parse(itemquatity),
                    Complete = false
                };
                await todoGetTable.InsertAsync(itemReg);
                dialog = new MessageDialog("Successful!");
                await dialog.ShowAsync();
            }
            catch (Exception em)
            {
                dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();
            }
        }

        private async void Btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Refresh called");
            await InitLocalStoreAsync();
            await RefreshTodoItems();
        }

        List<string> Names;
        List<string> Quantity;
        private async Task RefreshTodoItems()
        {

            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                // Select one field -- just the Text
                Names = await todoGetTable //Inventory_EasyTable => Inventory_EasyTable.ItemName
                    .Select(Inventory_EasyTable => Inventory_EasyTable.ItemName)
                    .ToListAsync();
                Quantity = await todoGetTable //Inventory_EasyTable => Inventory_EasyTable.ItemName
                    .Select(Inventory_EasyTable => Inventory_EasyTable.Quantity.ToString())
                    .ToListAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                foreach (var value in Names)
                {
                    listNames.Items.Add(value);
                }
                foreach (var value in Quantity)
                {
                    listQuantity.Items.Add(value);
                }
                this.btn_Refresh.IsEnabled = true;
            }
        }

        //helper functions
        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }

        public static bool IsNumeric(string str)
        {
            try
            {
                str = str.Trim();
                int foo = int.Parse(str);
                return (true);
            }
            catch (FormatException)
            {
                // Not a numeric value
                return (false);
            }
        }
    }
}
