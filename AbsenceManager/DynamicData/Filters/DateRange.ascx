<%@ Control Language="C#" CodeBehind="DateRange.ascx.cs" Inherits="AbsenceManager.DateRangeFilter" %>
<asp:TextBox ID="txbDateFrom" runat="server" CssClass="JQDTextBox">
</asp:TextBox>
<%--<ajaxtoolkit:calendarextender id="txbDateFrom_CalendarExtender" runat="server" enabled="True"
    targetcontrolid="txbDateFrom">
</ajaxtoolkit:calendarextender>--%>
 a
<asp:TextBox ID="txbDateTo" runat="server" CssClass="JQDTextBox">
</asp:TextBox>
<%--<ajaxtoolkit:calendarextender id="txbDateTo_CalendarExtender" runat="server" enabled="True"
    targetcontrolid="txbDateTo">
</ajaxtoolkit:calendarextender>--%>
<asp:Button ID="btnClear" CssClass="DDFilter" runat="server" Text="Limpar" OnClick="btnRangeButton_Click" />
