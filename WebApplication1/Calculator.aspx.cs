﻿using System;
using WebApplication1.CalcService;

namespace WebApplication1
{
    public partial class Calculator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void calcBtn_Click(object sender, EventArgs e)
        {

            calcPortTypeClient client = new calcPortTypeClient("calc");
            double a = 0;
            double b = 0;




            if (!(double.TryParse(numberA.Text, out a) && double.TryParse(numberB.Text, out b)))
            {

                outputLbl.Text = $"Nie poprawne dane! Nalezy wpisac tylko liczby.\nDane wejsciowe: {numberA.Text}, {numberB.Text}";
                return;
            }


            if (operation.Text == "+")
            {
                var result = await client.addAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");
            }
            else if (operation.Text == "-")
            {

                var result = await client.subAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");
            }
            else if (operation.Text == "/")
            {
                var result = await client.divAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");

            }
            else if (operation.Text == "*")
            {
                var result = await client.mulAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");

            }
            else if (operation.Text == "^")
            {
                var result = await client.powAsync(a, b);

                outputLbl.Text = result.result.ToString("F2");
            }

        }

    }
}