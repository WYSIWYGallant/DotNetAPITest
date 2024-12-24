using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TelerikWebApp1.Models;
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
                //pull personID from query string
                if (int.TryParse(Request.QueryString["personID"], out int personID))
                {
                    this.personID = personID;
                    await GetPerson(personID);
                }
                Highcharts chart = GenerateChart();
                string chartHtml = chart.ToHtmlString();

                if (ChartLiteral != null)
                {
                    ChartLiteral.Text = chartHtml;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ChartLiteral is null.");
                }
            }
        }

        private async Task<Person> GetPerson(int personID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Set a timeout for the HTTP client
                httpClient.Timeout = TimeSpan.FromSeconds(10);


                var response = await httpClient.GetAsync("https://localhost:7203/api/Person/" + personID);


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
        //Generate chart with random ints and dates
        private Highcharts GenerateChart()
        {
            List<int> randomInts = GenerateRandomInts(10, 0, 100000);
            List<string> randomDates = GenerateRandomDates(10, DateTime.Today, DateTime.Today.AddYears(50));
            System.Diagnostics.Debug.WriteLine("amount of ints: " + randomInts.Count());
            System.Diagnostics.Debug.WriteLine("amountofDates: " + randomDates.Count());

            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { Type = ChartTypes.Column })
                .SetTitle(new Title { Text = "Column Chart" })
                .SetXAxis(new XAxis { Categories = randomDates.ToArray() })
                .SetSeries(new Series[] {
                    new Series { Name = "Ints", Data = new Data(randomInts.Cast<object>().ToArray()) }
                });

            return chart;
        }
        private List<int> GenerateRandomInts(int count, int minValue, int maxValue)
        {
            Random random = new Random();
            List<int> randomInts = new List<int>();

            for (int i = 0; i < count; i++)
            {
                randomInts.Add(random.Next(minValue, maxValue));
            }

            return randomInts;
        }
        private List<string> GenerateRandomDates(int count, DateTime startDate, DateTime endDate)
        {
            Random random = new Random();
            List<string> randomDates = new List<string>();

            for (int i = 0; i < count; i++)
            {
                int range = (endDate - startDate).Days;
                randomDates.Add((startDate.AddDays(random.Next(range))).ToString());
            }

            randomDates.Sort();
            return randomDates;
        }
    }
}
    
