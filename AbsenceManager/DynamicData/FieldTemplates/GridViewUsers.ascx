<%@ Control Language="C#" CodeBehind="GridViewUsers.ascx.cs" Inherits="AbsenceManager.GridViewUsersField" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>

<asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" EnablePersistedSelection="true"
    AllowPaging="True" AllowSorting="True" CssClass="DDGridView" RowStyle-CssClass="td"
    HeaderStyle-CssClass="th" CellPadding="6" OnRowEditing="GridView1_RowEditing"
    OnSelectedIndexChanging="GridView1_SelectedIndexChanging">
    <Columns>
    </Columns>
    <PagerStyle CssClass="DDFooter" />
    <PagerTemplate>
        <asp:GridViewPager ID="GridViewPager1" runat="server" />
    </PagerTemplate>
    <EmptyDataTemplate>
        There are currently no items in this table.
    </EmptyDataTemplate>
</asp:GridView>
<asp:EntityDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableFlattening="true" />
<div id="ScriptContainer" runat="server">
</div>
