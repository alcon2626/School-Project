using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Storage;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Collections.ObjectModel;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace School_Calculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserAuth : Page
    {
       
        public UserAuth()
        {
            this.InitializeComponent();
        }

        public int IsAuth { get; set; }

        //[DataTable("Auth_Cred")]
        public class User_Cred
        {
            public string id { get; set; }
            public string FullName { get; set; }
            public string UserID { get; set; }
            public string Password { get; set; }
        }

        private IMobileServiceSyncTable<User_Cred> todoGetTable = App.MobileService.GetSyncTable<User_Cred>();
        private IMobileServiceSyncTable<TodoItem> todoTable = App.MobileService.GetSyncTable<TodoItem>();

        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("Rasmussen.db");
                store.DefineTable<User_Cred>();
                store.DefineTable<TodoItem>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }
            await SyncAsync();
        }

        private async Task SyncAsync()
        {
            await App.MobileService.SyncContext.PushAsync();
            await todoGetTable.PullAsync("User_Cred", todoGetTable.CreateQuery());
            await todoTable.PullAsync("TodoItem", todoTable.CreateQuery());
        }



        async private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            await InitLocalStoreAsync();

            GetAuthentication();

        }

        async public void GetAuthentication()
        {
            try
            {

                //IMobileServiceTable<User_Cred> todoTable = App.MobileService.GetTable<User_Cred>();

                List<User_Cred> items = await todoGetTable
                    .Where(User_Cred => User_Cred.UserID == txtBoxUserId.Text)
                    .ToListAsync();

                IsAuth = items.Count();

                foreach (var value in items)
                {
                    var dialog = new MessageDialog("UserID: " + value.UserID);
                    await dialog.ShowAsync();
                }


                if (IsAuth > 0)
                {
                    var dialog = new MessageDialog("You are Authenticated");
                    await dialog.ShowAsync();

                }
                else
                {
                    var dialog = new MessageDialog("User Must Register - Account Does Not Exist");
                    await dialog.ShowAsync();
                }
            }
            catch (Exception em)
            {
                var dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();
            }
        }

        async private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User_Cred itemReg = new User_Cred
                {
                    FullName = "System Test",
                    UserID = txtBoxUserId.Text,
                    Password = txtBoxPasswd.Text
                };
                await App.MobileService.GetTable<User_Cred>().InsertAsync(itemReg);
                var dialog = new MessageDialog("Successful!");
                await dialog.ShowAsync();
            }
            catch (Exception em)
            {
                var dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();
            }

        }

        private async void btnSync_Click(object sender, RoutedEventArgs e)
        {
            await InitLocalStoreAsync();
        }

        //C# to call MainPage from HyperLink button
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        public class TodoItem
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public bool Complete { get; set; }
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            await InitLocalStoreAsync();
            try
            {
                var mytext = textBox.Text;
                if(mytext == "" || mytext == null)
                {
                    mytext = "My first to do item";
                }
                TodoItem todoItem = new TodoItem() { Text = mytext, Complete = false };
                await todoTable.InsertAsync(todoItem);

            }
            catch (Exception em)
            {
                var dialog = new MessageDialog("An Error Occured: " + em.Message);
                await dialog.ShowAsync();
            }
            
        }

        private async void btnRefresh_Click_1(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Refresh called");
            await InitLocalStoreAsync();
            await RefreshTodoItems();
        }

        Collection<TodoItem> items = null;
        private async Task RefreshTodoItems()
        {
            
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                items = await todoTable
                    .Where(todoItem => todoItem.Complete == false)
                    .ToCollectionAsync();
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
                Debug.WriteLine(items);
                ListItems.ItemsSource = items;
                this.btnRefresh.IsEnabled = true;
            }
        }

        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            TodoItem item = cb.DataContext as TodoItem;
            await UpdateCheckedTodoItem(item);
        }

        private async Task UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the MobileService 
            // responds, the item is removed from the list 
            await todoTable.UpdateAsync(item);
            items.Remove(item);
            ListItems.Focus(Windows.UI.Xaml.FocusState.Unfocused);

            //await SyncAsync(); // offline sync
        }
    }
}
