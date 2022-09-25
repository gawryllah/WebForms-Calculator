<%@ Page Title="Kalkulator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="WebApplication1.Calculator"  Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2><%: Title %></h2>
    <div class="body">
       <div class="calculator">
           <div class="Item">Liczba A:
               <asp:TextBox ID="numberA" runat="server" BorderStyle="Groove" Font-Bold="True"></asp:TextBox>
           </div>
           <div class="Item">Liczba B:
               <asp:TextBox ID="numberB" runat="server" BorderStyle="Groove" Font-Bold="True"></asp:TextBox>
           </div>
           <div class="Item">Operacja: <asp:DropDownList ID="operation" runat="server">
                   <asp:ListItem Selected="True">+</asp:ListItem>
                   <asp:ListItem>-</asp:ListItem>
                   <asp:ListItem>/</asp:ListItem>
                   <asp:ListItem>*</asp:ListItem>
                   <asp:ListItem>^</asp:ListItem>
               </asp:DropDownList>
           </div>
           <div class="Item">Wynik: <asp:Label ID="outputLbl" runat="server"></asp:Label>
           </div>
           </div>
        </div>
    <div class="center">

        <asp:Button ID="calcBtn" runat="server" Text="Wykonaj operacje" CssClass="calc-button" OnClick="calcBtn_Click" />

    </div>
     <div class="grid">
    
         <asp:GridView ID="grid" runat="server" CellPadding="5" ForeColor="#333333" GridLines="None" Width="80%"  HorizontalAlign="Center" style="margin-top:25px">
             <AlternatingRowStyle BackColor="White" />
             <EditRowStyle BackColor="#2461BF" />
             <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
             <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
             <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
             <RowStyle BackColor="#EFF3FB" />
             <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
             <SortedAscendingCellStyle BackColor="#F5F7FB" />
             <SortedAscendingHeaderStyle BackColor="#6D95E1" />
             <SortedDescendingCellStyle BackColor="#E9EBEF" />
             <SortedDescendingHeaderStyle BackColor="#4870BE" />
         </asp:GridView>
    
     </div>
</asp:Content>
