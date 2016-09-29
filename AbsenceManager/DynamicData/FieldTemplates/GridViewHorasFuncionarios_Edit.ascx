<%@ Control Language="C#" CodeBehind="GridViewHorasFuncionarios_Edit.ascx.cs" 
    Inherits="AbsenceManager.GridViewHorasFuncionarios_Edit" %>
<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:CustomValidator ID="customValida" ErrorMessage="Error." runat="server" Display="None"
    ForeColor="Red"></asp:CustomValidator>
<asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" EnablePersistedSelection="true"
    AllowPaging="True" AllowSorting="True" CssClass="DDGridView" RowStyle-CssClass="td"
    HeaderStyle-CssClass="th" CellPadding="6" OnRowEditing="GridView1_RowEditing"
    OnSelectedIndexChanging="GridView1_SelectedIndexChanging" OnRowDeleted="GridView1_RowDeleted">
    <Columns>
        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="EditColumn">
            <ItemTemplate>
                <asp:ImageButton ImageUrl="~/Images/inline_edit.png" ID="ImageButtonInlineEdit" runat="server"
                    CausesValidation="False" CommandName="Edit" Text="Inline Edit" ToolTip="Inline Edit"
                    CssClass="IconSweets" Visible='<%# HasInlinePermission %>'></asp:ImageButton>
                <asp:ImageButton ImageUrl="~/Images/delete.png" ID="ImageButtonDelete" runat="server"
                    CausesValidation="True" CommandName="Delete" Text="Delete" ToolTip="Delete" CssClass="IconSweets"
                    Visible='<%# !PageReadOnly && ShowDeleteBtn %>' OnClientClick='return confirm("Are you sure you want to delete this item?");'>
                </asp:ImageButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:ImageButton ImageUrl="~/Images/update.png" ID="ImageButtonUpdate" runat="server"
                    CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update" CssClass="IconSweets">
                </asp:ImageButton>
                <asp:ImageButton ImageUrl="~/Images/cancel.png" ID="ImageButtonCancel" runat="server"
                    CausesValidation="True" CommandName="Cancel" Text="Close" ToolTip="Close" CssClass="IconSweets">
                </asp:ImageButton>
            </EditItemTemplate>
        </asp:TemplateField>
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
<div class="DDBottomHyperLink">
    <asp:LinkButton ID="InsertHyperLink" runat="server" OnClick="lkbToggle_Click" CausesValidation="false">
        <img id="Img2" runat="server" src="~/Images/plus.gif" alt="Insert new item" />Insert
        new item
    </asp:LinkButton>
</div>
<div id="ScriptContainer" runat="server">
</div>
<asp:FormView runat="server" ID="FormView1" DataSourceID="FormViewDataSource" DefaultMode="ReadOnly"
    RenderOuterTable="false">
    <InsertItemTemplate>
        <table id="detailsTable" class="DDInnerInsert" cellpadding="6">
            <asp:DynamicEntity ID="DynamicEntity1" runat="server" Mode="Insert" />
            <tr class="td">
                <td colspan="2">
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Insert" Text="Insert" />
                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Cancel" Text="Close"
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
    </InsertItemTemplate>
</asp:FormView>
<asp:EntityDataSource ID="FormViewDataSource" runat="server" EnableDelete="true"
    EnableFlattening="true" OnInserted="FormViewDataSource_Inserted" OnInserting="FormViewDataSource_Inserting" />
