// Grid.aspx.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using TelerikWebApp1.Models;

namespace TelerikWebApp1
{
    public partial class Grid : System.Web.UI.Page
    {
        public DataTable People
        {
            get
            {
                DataTable data = Session["Data"] as DataTable;

                if (data == null)
                {
                    data = GetDataFromApi();
                    Session["Data"] = data;
                }

                return data;
            }
            set
            {
                Session["Data"] = value;
            }
        }

        private DataTable ConvertToDataTable(List<Person> persons, List<PersonType> personTypes)
        {
            DataTable table = new DataTable();
            table.Columns.Add("PersonID", typeof(int));
            table.Columns.Add("PersonName", typeof(string));
            table.Columns.Add("PersonAge", typeof(int));
            table.Columns.Add("PersonType", typeof(string));

            foreach (var person in persons)
            {
                var personType = personTypes.Find(pt => pt.PersonTypeID == person.PersonType).PersonTypeDescription;
                table.Rows.Add(person.PersonID, person.PersonName, person.PersonAge, personType);
            }

            return table;
        }

        private DataTable GetDataFromApi()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Set a timeout for the HTTP client
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    // Log the start of the request

                    var response = httpClient.GetStringAsync("https://localhost:7203/api/Person").Result;


                    var persons = JsonConvert.DeserializeObject<List<Person>>(response);

                    response = httpClient.GetStringAsync("https://localhost:7203/api/PersonType").Result;
                    
                    var personTypes = JsonConvert.DeserializeObject<List<PersonType>>(response);
                    
                    return ConvertToDataTable(persons, personTypes);
                }
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                // Handle timeout
                System.Diagnostics.Debug.WriteLine("Request timed out.");
                throw new Exception("The request timed out.", ex.InnerException);
            }
            catch (Exception ex)
            {
                // Log and rethrow any other exceptions
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        private void RefreshData()
        {
            People = GetDataFromApi();
            RadGrid1.DataSource = People;
            RadGrid1.Rebind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = this.People;
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridHeaderItem headerItem = e.Item as GridHeaderItem;
            if (headerItem != null)
            {
                headerItem["EditColumn"].Text = string.Empty;
                headerItem["DeleteColumn"].Text = string.Empty;
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem dataItem)
            {
                // Get the data item and determine if the button should be added
                var data = (DataRowView)dataItem.DataItem;
                string personTypeValue = (string)data["PersonType"];

                // Create the ImageButton
                ImageButton imgButton = new ImageButton
                {
                    ImageUrl = "~/images/bar-chart.png", // Path to the image
                    OnClientClick = $"openNewWindow({data["PersonID"]}); return false;"
                };

                // Conditionally add the button
                if (personTypeValue.ToLower() == "student")
                {
                    dataItem["DeleteColumn"].Controls.Add(imgButton);
                }
            }
        }

        protected async void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editedItem = e.Item as GridEditableItem;
            var personID = (int)editedItem.GetDataKeyValue("PersonID");
            var person = new Person
            {
                PersonID = personID,
                PersonName = (editedItem["PersonName"].Controls[0] as TextBox).Text,
                PersonAge = int.Parse((editedItem["PersonAge"].Controls[0] as RadNumericTextBox).Text),
                PersonType = int.Parse((editedItem["PersonType"].Controls[0] as RadNumericTextBox).Text),
            };

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(person);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"https://localhost:7203/api/Person/{personID}", content);
                response.EnsureSuccessStatusCode();
            }

            RefreshData();
        }

        protected async void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            var editedItem = e.Item as GridEditableItem;
            var person = new Person
            {
                PersonName = (editedItem["PersonName"].Controls[0] as TextBox).Text,
                PersonAge = int.Parse((editedItem["PersonAge"].Controls[0] as RadNumericTextBox).Text),
                PersonType = int.Parse((editedItem["PersonType"].Controls[0] as RadNumericTextBox).Text),
            };
            int maxPersonID;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://localhost:7203/api/Person/maxPersonID");
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                maxPersonID = JsonConvert.DeserializeObject<int>(responseContent);
            }
            person.PersonID = maxPersonID;

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(person);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://localhost:7203/api/Person", content);
                response.EnsureSuccessStatusCode();
            }

            RefreshData();
        }

        protected async void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editedItem = e.Item as GridEditableItem;
            var personId = (int)editedItem.GetDataKeyValue("PersonID");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"https://localhost:7203/api/Person/{personId}");
                response.EnsureSuccessStatusCode();
            }

            RefreshData();
        }
    }

}