<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="MapaPresencas.aspx.cs"
    Inherits="AbsenceManager.MapaPresencas" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
    <h2 class="DDSubHeader">
        Mapa de Presenças do Funcionário
        <%= NomeFuncionario %>
    </h2>
    <asp:Label runat="server">Compensações: <%= Compensacoes %></asp:Label>
    </br>
    <div>
        <table style="margin-left: 30%; text-decoration: none">
            <tr>
                <td>
                    <asp:TextBox ID="TextBoxDate" runat="server" CssClass="JQDTextBox" Text="" Columns="20"></asp:TextBox>
                    <asp:DropDownList ID="TipoMarcacaoList" AutoPostBack="true" runat="server" />
                    <asp:Button ID="OK_Button" runat="server" OnClick="Button_OnClick" Text="Marcar" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Calendar ID="MonthCalendar" runat="server" Height="400px" Width="400px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black"
                        OtherMonthDayStyle-ForeColor="Gray" Font-Overline="false" OnDayRender="DayRender"
                        WeekendDayStyle-BackColor="DimGray" OnVisibleMonthChanged="MonthCalendar_MonthChanged">
                    </asp:Calendar>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
