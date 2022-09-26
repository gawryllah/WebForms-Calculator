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

        protected void calcBtn_Click(object sender, EventArgs e)
        {


            double a = 0;
            double b = 0;




            if (!(double.TryParse(numberA.Text, out a) && double.TryParse(numberB.Text, out b)))
            {
                if (numberA.Text.Length == 0 || numberB.Text.Length == 0)
                    return;

                outputLbl.Text = $"Niepoprawne dane! Nalezy wpisac tylko liczby.\nDane wejsciowe: {numberA.Text}, {numberB.Text}";
                return;
            }


            CreateOperation(client, a, b, operation);
            SaveOperation(path);


        }


        void SaveOperation(string path)
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

        void CreateOperation(calcPortTypeClient ptc, double a, double b, DropDownList operation)
        {

            switch (operation.Text)
            {
                case "+":
                    new Operation() { operationDate = DateTime.Now, operationValue = $"{a} {operation.Text} {b} = {ptc.addAsync(a, b).Result.result}" };
                    break;
                case "-":
                    new Operation() { operationDate = DateTime.Now, operationValue = $"{a} {operation.Text} {b} = {ptc.subAsync(a, b).Result.result}" };
                    break;
                case "/":
                    new Operation() { operationDate = DateTime.Now, operationValue = $"{a} {operation.Text} {b} = {ptc.divAsync(a, b).Result.result}" };
                    break;
                case "*":
                    new Operation() { operationDate = DateTime.Now, operationValue = $"{a} {operation.Text} {b} = {ptc.mulAsync(a, b).Result.result}" };
                    break;
                case "^":
                    new Operation() { operationDate = DateTime.Now, operationValue = $"{a} {operation.Text} {b} = {ptc.powAsync(a, b).Result.result}" };
                    break;
            }

        }
    }

    public class Operation
    {
        public static List<Operation> operations = new List<Operation>();

        public Operation()
        {
            operations.Add(this);
        }

        [XmlAttribute("Operation Date")]
        public DateTime operationDate { get; set; }
        [XmlAttribute("Operation Value")]
        public string operationValue { get; set; }

        public static void Save(string path)
        {
            using (var writer = new System.IO.StreamWriter(path))
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