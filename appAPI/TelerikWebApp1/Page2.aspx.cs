using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TelerikWebApp1
{
    public partial class Page2 : Page
    {
        public int personID { get; set; }
        public Person Person { get; set; }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["personID"], out int personID))
                {
                    this.personID = personID;
                    await GetPerson(personID);
                }
            }
        }

        private async Task<Person> GetPerson(int personID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Set a timeout for the HTTP client
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                // Log the start of the request
                System.Diagnostics.Debug.WriteLine("Starting API request...");

                var response = await httpClient.GetAsync("https://localhost:7203/api/Person/" + personID);

                // Log the response
                System.Diagnostics.Debug.WriteLine("API response received.");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var person = JsonConvert.DeserializeObject<Person>(responseData);
                    this.Person = person;
                    return person;
                }
                else
                {
                    // Handle the error response
                    System.Diagnostics.Debug.WriteLine("API request failed with status code: " + response.StatusCode);
                    return null;
                }
            }
        }
        public DataTable People
        {
            get
            {
                DataTable data = Session["SinglePerson"] as DataTable;
                return data;
            }
            set
            {
                Session["SinglePerson"] = value;
            }
        }

        private DataTable ConvertToDataTable(Person person)
        {
            DataTable table = new DataTable();
            table.Columns.Add("PersonName", typeof(string));
            table.Columns.Add("PersonAge", typeof(int));
            table.Columns.Add("PersonType", typeof(int));
            table.Rows.Add(person.PersonName, person.PersonAge, person.PersonType);

            return table;
        }
        protected void RadGrid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            RadGrid1.DataSource = ConvertToDataTable(this.Person);
        }

    }
}
    
