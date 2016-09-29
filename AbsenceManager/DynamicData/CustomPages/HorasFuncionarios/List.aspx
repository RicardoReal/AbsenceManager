<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="AbsenceManager.DynamicData.CustomPages.HorasFuncionarios.List" %>

<%@ Register Src="~/DynamicData/Content/GridViewPager.ascx" TagName="GridViewPager"
    TagPrefix="asp" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" AutoLoadForeignKeys="true">
        <DataControls>
            <asp:DataControlReference ControlID="GridView1" />
        </DataControls>
    </asp:DynamicDataManager>
    <h2 class="DDSubHeader">
        <%= table.DisplayName%>
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:CustomValidator ID="customValida" ErrorMessage="Error." runat="server" Display="None"
                ForeColor="Red"></asp:CustomValidator>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                HeaderText="List of validation errors" CssClass="DDValidator" />
            <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="GridView1"
                Display="None" CssClass="DDValidator" />
            <div class="DD" id="SearchContainer" runat="server">
                <div id="toggleFilters">
                    <img alt="" src="~/Images/RightArrow.gif" class="imgArrows" id="FilterArrow" runat="server" />
                </div>
                <asp:TextBox ID="TextBoxSearch" class="water" runat="server" AutoCompleteType="Search"></asp:TextBox>
                <asp:ImageButton ImageUrl="~/Images/search.png" ID="btnSearch" OnClick="btnSearch_Click"
                    runat="server" CausesValidation="True" Text="Search" ToolTip="Search" CssClass="IconSweets"></asp:ImageButton>
                <asp:ImageButton ImageUrl="~/Images/reset.png" ID="btnReset" runat="server" CausesValidation="True"
                    Text="Reset" ToolTip="Reset" OnClick="btnReset_Click" CssClass="IconSweets"></asp:ImageButton>
                &nbsp
                <asp:Label ID="LabelFilter" runat="server" Text="(Filtered)" CssClass="DDLabelFiltered"></asp:Label>
                <div class="DDInnerFilter" id="FilterValidationContainer" style="display: none;">
                    <div id="FilterRepeaterContainer">
                        <ul id="FilterContainer">
                            <asp:QueryableFilterRepeater runat="server" ID="FilterRepeater">
                                <ItemTemplate>
                                    <li>
                                        <div class="FilterRepeaterItemContainer">
                                            <div class="FilterRepeaterItemLabelContainer">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("DisplayName") %>' OnPreRender="Label_PreRender" />
                                            </div>
                                            <asp:DynamicFilter runat="server" ID="DynamicFilter" OnFilterChanged="DynamicFilter_FilterChanged" />
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:QueryableFilterRepeater>
                        </ul>
                    </div>
                    <br />
                </div>
            </div>
            <div id="GridViewContainer">
                <asp:GridView ID="GridView1" runat="server" DataSourceID="GridDataSource" EnablePersistedSelection="true"
                    AllowPaging="True" AllowSorting="True" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" CellPadding="6" OnRowUpdated="GridView1_OnRowUpdated" OnRowDeleted="GridView1_OnRowDeleted"
                    OnRowCreated="GridView1_RowCreated" OnRowCommand="GridView1_ItemCommand">
                    <PagerTemplate>
                        <table id="detailsTable" class="DDDetailsTableAlt" cellpadding="6">
                            <asp:DynamicEntity ID="DynamicEntity1" runat="server" Mode="ReadOnly" />
                        </table>
                    </PagerTemplate>
                    <Columns>
                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="EditColumn">
                            <ItemTemplate>
                                <asp:DynamicHyperLink ID="DynamicHyperLinkEdit" runat="server" Action="Edit" Visible='<%# HasOnScreenPermission %>'>
                                    <img src="../Images/edit.png" alt="Edit" title="Edit" class="IconSweets" />
                                </asp:DynamicHyperLink>
                                <asp:ImageButton ImageUrl="~/Images/delete.png" ID="ImageButtonDelete" runat="server" Visible='<%# HasOnScreenPermission %>'
                                    CausesValidation="True" CommandName="Delete" Text="Delete" ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this item?");'
                                    CssClass="IconSweets"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ImageUrl="~/Images/update.png" ID="ImageButtonUpdate" runat="server"
                                    CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update" CssClass="IconSweets"></asp:ImageButton>
                                <asp:ImageButton ImageUrl="~/Images/cancel.png" ID="ImageButtonCancel" runat="server"
                                    CausesValidation="True" CommandName="Cancel" Text="Close" ToolTip="Close" CssClass="IconSweets"></asp:ImageButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Track Changes" ItemStyle-CssClass="EditColumn">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="~/Images/page_find.png" ID="ImageButtonTrackChanges" runat="server"
                                    CausesValidation="True" CommandName="ShowTrackChanges" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    Text="Show Track Changes" ToolTip="Show Track Changes" CssClass="IconSweets"
                                    Visible='<%# HasTrackChangesPermission %>'></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ImageUrl="~/Images/page_find.png" ID="ImageButton1" runat="server"
                                    CausesValidation="True" CommandName="ShowTrackChanges" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    Text="Track Changes" ToolTip="Show Track Changes" CssClass="IconSweets" Visible='<%# HasTrackChangesPermission %>'></asp:ImageButton>
                            </EditItemTemplate>
                            <ItemStyle CssClass="EditColumn" />
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
                <asp:EntityDataSource ID="GridDataSource" runat="server" EnableDelete="true" EnableUpdate="true"
                    OnSelecting="GridDataSource_Selecting" />
                <asp:QueryExtender TargetControlID="GridDataSource" ID="GridQueryExtender" runat="server">
                    <asp:DynamicFilterExpression ControlID="FilterRepeater" />
                    <asp:SearchExpression DataFields="default" SearchType="Contains">
                        <asp:ControlParameter ControlID="TextBoxSearch" />
                    </asp:SearchExpression>
                </asp:QueryExtender>
                <br />
                <div class="DDBottomHyperLink">
                    <asp:DynamicHyperLink ID="InsertHyperLink" runat="server" Action="Insert" Visible='<%# HasOnScreenPermission %>'>
                        <img id="Img1" runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Insert new item" />
                        Insert new item
                    </asp:DynamicHyperLink>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
