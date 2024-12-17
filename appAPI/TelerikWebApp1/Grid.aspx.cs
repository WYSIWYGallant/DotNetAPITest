// Grid.aspx.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

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

        private DataTable ConvertToDataTable(List<Person> persons)
        {
            DataTable table = new DataTable();
            table.Columns.Add("PersonID", typeof(int));
            table.Columns.Add("PersonName", typeof(string));
            table.Columns.Add("PersonAge", typeof(int));
            table.Columns.Add("PersonType", typeof(int));

            foreach (var person in persons)
            {
                table.Rows.Add(person.PersonID, person.PersonName, person.PersonAge, person.PersonType);
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
                    System.Diagnostics.Debug.WriteLine("Starting API request...");

                    var response = httpClient.GetStringAsync("https://localhost:7203/api/Person").Result;

                    // Log the response
                    System.Diagnostics.Debug.WriteLine("API response received.");

                    var persons = JsonConvert.DeserializeObject<List<Person>>(response);
                    return ConvertToDataTable(persons);
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
            RadComboBox comboBox = e.Item.FindControl("RCB_City") as RadComboBox;
            if (comboBox != null)
            {
                if (!(e.Item.DataItem is GridInsertionObject))
                {
                    comboBox.SelectedValue = (e.Item.DataItem as DataRowView)["City"].ToString();
                }
                comboBox.DataTextField = string.Empty;
                //comboBox.DataSource = this.GetCities();
                comboBox.DataBind();
                if (this.RadGrid1.ResolvedRenderMode == RenderMode.Mobile)
                {
                    (e.Item.FindControl("TB_Age") as WebControl).Enabled = false;
                }
                else
                {
                    ((e.Item as GridEditableItem)["Age"].Controls[0] as WebControl).Enabled = false;
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
                var response = await httpClient.PutAsync($"https://localhost:7203/api/Person/{personID},{person.PersonName},{person.PersonAge},{person.PersonType}", content);
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

    public class Person
    {
        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public int PersonAge { get; set; }
        public int PersonType { get; set; }
    }
}
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Web.UI.WebControls;
//using Telerik.Web.UI;

//namespace TelerikWebApp1
//{
//    public partial class Grid : System.Web.UI.Page
//    {
//        public DataTable Sellers
//        {
//            get
//            {
//                DataTable data = Session["Data"] as DataTable;

//                if (data == null)
//                {
//                    data = GetSellers();
//                    Session["Data"] = data;
//                }

//                return data;
//            }
//        }

//        public DataTable GetSellers()
//        {
//            DataTable data = new DataTable();
//            data.Columns.Add("ID", typeof(int));
//            data.Columns.Add("Name");
//            data.Columns.Add("Age", typeof(int)).DefaultValue = 0;
//            data.Columns.Add("BirthDate", typeof(DateTime));
//            data.Columns.Add("Rating", typeof(int)).DefaultValue = 0;
//            data.Columns.Add("City");
//            data.PrimaryKey = new DataColumn[] { data.Columns["ID"] };

//            List<string> firstNames = new List<string>() { "Nancy", "Andrew", "Janet", "Margaret", "Steven", "Michael", "Robert", "Laura", "Anne", "Nige" };
//            List<string> cities = this.GetCities();
//            List<DateTime> birthDates = new List<DateTime>() { DateTime.Parse("1948/12/08"), DateTime.Parse("1952/02/19"), DateTime.Parse("1963/08/30"), DateTime.Parse("1937/09/19"), DateTime.Parse("1955/03/04"), DateTime.Parse("1963/07/02"), DateTime.Parse("1960/05/29"), DateTime.Parse("1958/01/09"), DateTime.Parse("1966/01/27"), DateTime.Parse("1966/03/27") };
//            Random random = new Random();

//            for (int i = 0; i < 84; i++)
//            {
//                DateTime birthDate = birthDates[random.Next(birthDates.Count - 1)];
//                data.Rows.Add(
//                    random.Next(10000, int.MaxValue),
//                    firstNames[random.Next(firstNames.Count - 1)],
//                    DateTime.Now.Year - birthDate.Year, birthDate, random.Next(1, 5),
//                    cities[random.Next(cities.Count - 1)]);
//            }

//            return data;
//        }

//        public List<string> GetCities()
//        {
//            return new List<string>()
//        {
//            "Seattle",
//            "Tacoma",
//            "Kirkland",
//            "Redmond",
//            "London",
//            "Philadelphia",
//            "New York",
//            "Seattle",
//            "London",
//            "Boston"
//        };
//        }

//        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
//        {
//            RadGrid1.DataSource = this.Sellers;
//        }

//        protected void RadGrid1_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
//        {
//            Hashtable table = new Hashtable();
//            (e.Item as GridEditableItem).ExtractValues(table);

//            DataRow row = this.Sellers.Rows.Find((e.Item as GridEditableItem).GetDataKeyValue("ID"));

//            foreach (string key in table.Keys)
//            {
//                row[key] = table[key] ?? DBNull.Value;
//            }
//            DateTime date;
//            if (DateTime.TryParse((row["BirthDate"].ToString()), out date))
//            {
//                row["Age"] = DateTime.Now.Year - date.Year;
//            }
//            else
//            {
//                row["Age"] = 0;
//            }
//        }

//        protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
//        {
//            Hashtable table = new Hashtable();
//            (e.Item as GridEditableItem).ExtractValues(table);
//            DataRow row = this.Sellers.NewRow();

//            foreach (string key in table.Keys)
//            {
//                if (table[key] != null)
//                {
//                    row[key] = table[key];
//                }
//            }
//            row["ID"] = new Random().Next(int.MaxValue);
//            DateTime date;
//            if (DateTime.TryParse((row["BirthDate"].ToString()), out date))
//            {
//                row["Age"] = DateTime.Now.Date.Year - date.Year;
//            }
//            this.Sellers.Rows.InsertAt(row, 0);
//        }

//        protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
//        {
//            this.Sellers.Rows.Remove(this.Sellers.Rows.Find((e.Item as GridEditableItem).GetDataKeyValue("ID")));
//        }

//        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
//        {
//            RadComboBox comboBox = e.Item.FindControl("RCB_City") as RadComboBox;
//            if (comboBox != null)
//            {
//                if (!(e.Item.DataItem is GridInsertionObject))
//                {
//                    comboBox.SelectedValue = (e.Item.DataItem as DataRowView)["City"].ToString();
//                }
//                comboBox.DataTextField = string.Empty;
//                comboBox.DataSource = this.GetCities();
//                comboBox.DataBind();
//                if (this.RadGrid1.ResolvedRenderMode == RenderMode.Mobile)
//                {
//                    (e.Item.FindControl("TB_Age") as WebControl).Enabled = false;
//                }
//                else
//                {
//                    ((e.Item as GridEditableItem)["Age"].Controls[0] as WebControl).Enabled = false;
//                }
//            }
//        }

//        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
//        {
//            GridHeaderItem headerItem = e.Item as GridHeaderItem;
//            if (headerItem != null)
//            {
//                headerItem["EditColumn"].Text = string.Empty;
//                headerItem["DeleteColumn"].Text = string.Empty;
//            }
//        }
//    }
//}
