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
using System.Threading.Tasks;
using Windows.Devices.Geolocation;


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
        //GEOLOCATION Stuff
        private async void OneShotLocation_Click(object sender, RoutedEventArgs e)
        {

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                LatitudeTextBlock.Text = geoposition.Coordinate.Latitude.ToString("0.00");
                LongitudeTextBlock.Text = geoposition.Coordinate.Longitude.ToString("0.00");
                StatusTextBlock.Text = "Found GPS Location.";
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    StatusTextBlock.Text = "GPS location is disabled in phone settings.";
                }
                //else
                {
                    // something else happened acquring the location
                    StatusTextBlock.Text = "Error: " + ex.Message;
                }
            }
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
            listNames.Items.Clear();
            listQuantity.Items.Clear();
            Debug.WriteLine("Refresh called");
            await InitLocalStoreAsync();
            await RefreshTodoItems();
        }

        async private void Btn_UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            await InitLocalStoreAsync();
            List<Inventory_EasyTable> ITEMIDs;
            MessageDialog dialog;
            try
            {
                string itemname = await InputTextDialogAsync("Please enter new item name.");
                string itemquatity = await InputTextDialogAsync("Please enter new item quatity.");
                if (itemname == "" || itemquatity == "") { return; }
                if (!IsNumeric(itemquatity))
                {
                    dialog = new MessageDialog("Please enter a numeric value and try again.");
                    await dialog.ShowAsync();
                    return;
                }
                ITEMIDs = await todoGetTable
                    .Where(Inventory_EasyTable => Inventory_EasyTable.ItemName == itemname)
                    .ToListAsync();
                Debug.WriteLine(ITEMIDs[0].ToString());
                Inventory_EasyTable itemReg = new Inventory_EasyTable
                {
                    id = ITEMIDs[0].id.ToString(),//"e9b35795-1e61-4c71-aba7-61391da13338",
                    ItemName = itemname,
                    Quantity = Int32.Parse(itemquatity),
                    Complete = false
                };
                await todoGetTable.UpdateAsync(itemReg);
                dialog = new MessageDialog("Successful!");
                await dialog.ShowAsync();
            }
            catch (Exception em)
            {
                dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();
            }
            string text = listNames.SelectedItems[0].ToString();
            Debug.WriteLine(text);
        }

        async private void Btn_DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            await InitLocalStoreAsync();
            List<Inventory_EasyTable> ITEMIDs;
            MessageDialog dialog;
            bool result = await WarningDialogAsync("The selected item will be deleted");
            if (!result)
            {
                return;
            }
            else
            {
                try
                {
                    string itemname = listNames.SelectedItems[0].ToString();
                    ITEMIDs = await todoGetTable
                        .Where(Inventory_EasyTable => Inventory_EasyTable.ItemName == itemname)
                        .ToListAsync();
                    Debug.WriteLine(ITEMIDs[0].ToString());
                    Inventory_EasyTable itemReg = new Inventory_EasyTable
                    {
                        id = ITEMIDs[0].id.ToString(),//"e9b35795-1e61-4c71-aba7-61391da13338",
                        ItemName = itemname,
                        Quantity = 0,
                        Complete = false
                    };
                    await todoGetTable.DeleteAsync(itemReg);
                    dialog = new MessageDialog("Successful!");
                    await dialog.ShowAsync();
                }
                catch (Exception em)
                {
                    dialog = new MessageDialog("An Error Occured: " + em.Message);
                    await dialog.ShowAsync();
                }
            }                        
        }

        TextBox inputTextBox;
        /*******************************************************************************helper functions***************************************************************************/
        private async Task<string> InputTextDialogAsync(string title)//creates a diologbox
        {
            inputTextBox = new TextBox();
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

        private async Task<bool> WarningDialogAsync(string title)//creates a warning diologbox
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return true;
            else
                return false;
        }

        public static bool IsNumeric(string str) //is numeric
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
        //get item id
        private async Task<List<Inventory_EasyTable>> GetItem(string name)
        {
            List<Inventory_EasyTable> items = null;

            MobileServiceInvalidOperationException exception = null;
            try
            {
                items = await todoGetTable
                     .Where(Inventory_EasyTable => Inventory_EasyTable.ItemName == name)
                     .ToListAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
                return null;
            }
            else
            {
                return items;
            }
        }
        //refresh grid
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
                Itemcount.Text = "Total number of items: " + listNames.Items.Count.ToString();
                Int32 Total = 0;
                foreach (var value in Quantity)
                {
                    listQuantity.Items.Add(value);
                    Total += Int32.Parse(value);
                }
                Itemtotal.Text = Total.ToString();
                this.btn_Refresh.IsEnabled = true;
            }
        }
    }
}
