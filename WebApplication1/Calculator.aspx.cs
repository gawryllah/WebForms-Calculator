using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using WebApplication1.CalcService;

namespace WebApplication1
{
    public partial class Calculator : System.Web.UI.Page
    {
        calcPortTypeClient client;
        string path = "~/Data/Operations.xml";

        

        protected void Page_Load(object sender, EventArgs e)
        {
            var mappedPath = Server.MapPath(path);

            client = new calcPortTypeClient("calc");
            if (!IsPostBack)
            {
                LoadData(path);
            }
        }

        protected async void calcBtn_Click(object sender, EventArgs e)
        {


            double a = 0;
            double b = 0;




            if (!(double.TryParse(numberA.Text, out a) && double.TryParse(numberB.Text, out b)))
            {
                if (outputLbl.Text.Length == 0)
                    return;

                outputLbl.Text = $"Niepoprawne dane! Nalezy wpisac tylko liczby.\nDane wejsciowe: {numberA.Text}, {numberB.Text}";
                return;
            }

            if (operation.SelectedIndex == 0)
            {
                var result = await client.addAsync(a, b);

                SaveOperation(path, new Operation() { operationDate = DateTime.Now, operationVale = $"{a} {operation.Text} {b} = {result.result}" });

                outputLbl.Text = result.result.ToString("F2");
            }
            else if (operation.SelectedIndex == 1)
            {
                var result = await client.subAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");
            }
            else if (operation.SelectedIndex == 2)
            {
                var result = await client.divAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");

            }
            else if (operation.SelectedIndex == 3)
            {
                var result = await client.mulAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");

            }
            else if (operation.SelectedIndex == 4)
            {
                var result = await client.powAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");
            }


        }

        void LoadData(string path)
        {

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);
            }
            catch
            {
                return;
            }

            using (DataSet ds = new DataSet())
            {
                ds.ReadXml(path);
                grid.DataSource = ds;
                grid.DataBind();
            }
        }

        void SaveOperation(string path, Operation operation)
        {
            var mappedPath = Server.MapPath(path);

            Operation.Save(mappedPath);
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid.PageIndex = e.NewPageIndex;
            this.DataBind();
        }



    }

    public class Operation
    {
        public  static List<Operation> operations = new List<Operation>();

        public Operation()
        {
            operations.Add(this);
        }

        public DateTime operationDate { get; set; }
        public string operationVale { get; set; }

        public static void Save(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(typeof(List<Operation>));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(writer, operations);
                writer.Flush();
            }
        }

        public static Operation Load(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(Operation));
                return serializer.Deserialize(stream) as Operation;
            }
        }

        /*
        public void Save(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(this.GetType());

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(writer, this, ns);
                writer.Flush();
            }
        }

        public static Operation Load(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(Operation));
                return serializer.Deserialize(stream) as Operation;
            }
        }
        */
    }

   
}