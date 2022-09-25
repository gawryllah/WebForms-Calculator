using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                try
                {
                    Operation.Load(mappedPath);
                    LoadData(mappedPath);
                }
                catch
                {
                    throw;
                }
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

                SaveOperation(path, new Operation() { operationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), operationVale = $"{a} {operation.Text} {b} = {result.result}" });

                outputLbl.Text = result.result.ToString("F2");
            }
            else if (operation.SelectedIndex == 1)
            {
                var result = await client.subAsync(a, b);
                SaveOperation(path, new Operation() { operationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), operationVale = $"{a} {operation.Text} {b} = {result.result}" });
                outputLbl.Text = result.result.ToString("F2");
            }
            else if (operation.SelectedIndex == 2)
            {
                var result = await client.divAsync(a, b);
                SaveOperation(path, new Operation() { operationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), operationVale = $"{a} {operation.Text} {b} = {result.result}" });
                outputLbl.Text = result.result.ToString("F2");

            }
            else if (operation.SelectedIndex == 3)
            {
                var result = await client.mulAsync(a, b);
                SaveOperation(path, new Operation() { operationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), operationVale = $"{a} {operation.Text} {b} = {result.result}" });
                outputLbl.Text = result.result.ToString("F2");

            }
            else if (operation.SelectedIndex == 4)
            {
                var result = await client.powAsync(a, b);
                SaveOperation(path, new Operation() { operationDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), operationVale = $"{a} {operation.Text} {b} = {result.result}" });
                outputLbl.Text = result.result.ToString("F2");
            }


        }


        void SaveOperation(string path, Operation operation)
        {
            var mappedPath = Server.MapPath(path);

            Operation.Save(mappedPath);
            LoadData(mappedPath);
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

        /*
        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grid.PageIndex = e.NewPageIndex;
            this.DataBind();
        }
        */



    }

    public class Operation
    {
        public static List<Operation> operations = new List<Operation>();

        public Operation()
        {
            operations.Add(this);
        }

        [XmlAttribute("Operation Date")]
        public string operationDate { get; set; }
        [XmlAttribute("Operation Value")]
        public string operationVale { get; set; }

        public static void Save(string FileName)
        {
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(typeof(List<Operation>));

                serializer.Serialize(writer, operations);
                writer.Flush();
            }
        }

        public static void Load(string path)
        {
            if (!File.Exists(path))
                return;

            using (var stream = System.IO.File.OpenRead(path))
            {
                var serializer = new XmlSerializer(typeof(List<Operation>));
                operations = serializer.Deserialize(stream) as List<Operation>;
            }
        }
    }


}