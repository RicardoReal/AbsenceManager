<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="MarcacaoFerias.aspx.cs"
    Inherits="AbsenceManager.MarcacaoFerias" EnableViewState="true" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />
    <h2 class="DDSubHeader">
        Mapa Presenças
    </h2>
    <asp:Label>Férias Marcadas: <%= FeriasMarcadas %> | </asp:Label>
    <asp:Label>Férias Por Marcar: <%= FeriasPorMarcar%> | </asp:Label>
    <asp:Label runat="server" ForeColor="ForestGreen">Férias Aprovadas: <%= FeriasAprovadas %> | </asp:Label>
    <asp:Label runat="server" ForeColor="LightCoral">Férias Não Aprovadas: <%= FeriasNaoAprovadas %> | </asp:Label>
    <asp:Label runat="server" ForeColor="MediumPurple">Férias Pendentes: <%= FeriasPendentes %> </asp:Label>
    </br>
    </br>
    <%--<asp:Label ID="Label1" runat="server">Compensações: <%= Compensacoes %></asp:Label> --%>
    </br>
    </br>
    <div>
        <asp:Label style="margin-left: 15%" >Tipo de Ausência: </asp:Label>
        <asp:DropDownList ID="TipoAusenciasDDL" runat="server" />
        </br>

        <asp:Button ID="SubmitButton" OnClick="SubmitButton_Click" runat="server" Text="Submeter" style="margin-left: 15%"/>
        <table style="margin-left: 15%; text-decoration: none">
            <tr>
                <td>
                    <asp:Calendar ID="JanCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray"></asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="FebCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="MarCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="AprCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Calendar ID="MayCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="JunCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="JulCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="AugCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Calendar ID="SepCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="OctCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="NovCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="DezCalendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Calendar ID="Jan_Calendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray"></asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="Feb_Calendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="Mar_Calendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
                <td>
                    <asp:Calendar ID="Apr_Calendar" runat="server" Height="173px" Width="205px" BorderColor="#714734"
                        DayHeaderStyle-BackColor="Gray" DayHeaderStyle-Font-Bold="false" TitleStyle-BackColor="#FF3300"
                        TitleStyle-ForeColor="White" Font-Names="Tahoma, sans-serif" Font-Size="Small"
                        BackColor="#ECECD8" ShowNextPrevMonth="false" OnSelectionChanged="Calendar_SelectionChanged"
                        OnPreRender="Calendar_PreRender" SelectedDayStyle-BackColor="Black" OtherMonthDayStyle-ForeColor="Gray"
                        Font-Overline="false" OnDayRender="DayRender" WeekendDayStyle-BackColor="DimGray">
                    </asp:Calendar>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
