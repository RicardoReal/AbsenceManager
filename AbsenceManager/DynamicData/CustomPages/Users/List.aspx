<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeBehind="List.aspx.cs" Inherits="AbsenceManager.DynamicData.CustomPages.Users.List" %>

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
                <div id="toggleFilters" runat="server">
                    <img alt="" src="~/Images/RightArrow.gif" class="imgArrows" id="FilterArrow" runat="server" />
                </div>
                <asp:TextBox ID="TextBoxSearch" class="water" runat="server" AutoCompleteType="Search"></asp:TextBox>
                <asp:ImageButton ImageUrl="~/Images/search.png" ID="btnSearch" OnClick="btnSearch_Click"
                    runat="server" CausesValidation="True" Text="Search" ToolTip="Search" CssClass="IconSweets">
                </asp:ImageButton>
                <asp:ImageButton ImageUrl="~/Images/reset.png" ID="btnReset" runat="server" CausesValidation="True"
                    Text="Reset" ToolTip="Reset" OnClick="btnReset_Click" CssClass="IconSweets">
                </asp:ImageButton>
                &nbsp
                <asp:Label ID="LabelFilter" runat="server" Text="(Filtered)" CssClass="DDLabelFiltered"></asp:Label>
                <div class="DDInnerFilter" id="FilterValidationContainer" style="display: none;">
                    <div id="FilterRepeaterContainer" runat="server">
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
            <div id="ButtonContainer" runat="server">
                <asp:Label runat="server" Text="Mês:" style="float: left; padding-top: 1px; margin-right: 5px; margin-left: 2px;font-size:smaller;"/>
                <asp:DropDownList ID="MonthList" runat="server" style="float: left; padding-top: 1px; margin-right: 5px; margin-left: 2px; font-size:x-small;"/>
                <asp:Label runat="server" Text="Departamento:" style="float: left; padding-top: 1px; margin-right: 5px; margin-left: 2px;font-size:smaller;"/>
                <asp:DropDownList ID="DeptList" runat="server" style="float: left; padding-top: 1px; margin-right: 5px; margin-left: 2px; font-size:x-small;"/>
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click">
	                <div style="float: left; padding-top: 1px; margin-right: 5px; margin-left: 2px; cursor: pointer;" class="DDBottomHyperLink">
		                Extrair Listagem
	                </div>
                </asp:LinkButton>
            </div>
            <br>
            <div id="GridViewContainer">
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" CellPadding="6" CssClass="DDGridView" DataSourceID="GridDataSource" EnablePersistedSelection="True" HeaderStyle-CssClass="th" OnRowCommand="GridView1_ItemCommand" OnRowCreated="GridView1_RowCreated" RowStyle-CssClass="td">
                    <PagerTemplate>
                        <table id="detailsTable" cellpadding="6" class="DDDetailsTableAlt">
                            <asp:DynamicEntity ID="DynamicEntity1" runat="server" Mode="ReadOnly" />
                        </table>
                    </PagerTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="EditColumn" ShowHeader="False">
                            <ItemTemplate>
                                <asp:DynamicHyperLink ID="DynamicHyperLinkEdit" runat="server" Action="Edit" Visible="<%# HasOnScreenPermission %>">
                                    <img src="../Images/edit.png" alt="Edit" title="Edit" class="IconSweets" />
                                </asp:DynamicHyperLink>
                                <asp:ImageButton ID="ImageButtonDelete" runat="server" CausesValidation="True" CommandName="Delete" CssClass="IconSweets" ImageUrl="~/Images/delete.png" OnClientClick="return confirm(&quot;Are you sure you want to delete this item?&quot;);" Text="Delete" ToolTip="Delete" Visible="<%# HasOnScreenPermission %>" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ImageButtonUpdate" runat="server" CausesValidation="True" CommandName="Update" CssClass="IconSweets" ImageUrl="~/Images/update.png" Text="Update" ToolTip="Update" />
                                <asp:ImageButton ID="ImageButtonCancel" runat="server" CausesValidation="True" CommandName="Cancel" CssClass="IconSweets" ImageUrl="~/Images/cancel.png" Text="Close" ToolTip="Close" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mapa Presenças" ItemStyle-CssClass="EditColumn">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButtonMapaPresencas" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="ShowMapaPresencas" CssClass="IconSweets" ImageUrl="~/Images/page_find.png" Text="Mapa Presenças" ToolTip="Show Mapa Presenças" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ImageButtonMapaPresencas" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="ShowMapaPresencas" CssClass="IconSweets" ImageUrl="~/Images/page_find.png" Text="Mapa Presenças" ToolTip="Show Mapa Presenças" />
                            </EditItemTemplate>
                            <ItemStyle CssClass="EditColumn" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Track Changes" ItemStyle-CssClass="EditColumn">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButtonTrackChanges" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="ShowTrackChanges" CssClass="IconSweets" ImageUrl="~/Images/page_find.png" Text="Show Track Changes" ToolTip="Show Track Changes" Visible="<%# HasTrackChangesPermission %>" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton ID="ImageButtonTrackChanges" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="ShowTrackChanges" CssClass="IconSweets" ImageUrl="~/Images/page_find.png" Text="Track Changes" ToolTip="Show Track Changes" Visible="<%# HasTrackChangesPermission %>" />
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
                <asp:EntityDataSource ID="GridDataSource" runat="server" ConnectionString="name=AM_Entities" DefaultContainerName="AM_Entities" EnableDelete="True" EnableFlattening="False" EnableUpdate="True" EntitySetName="Users" EntityTypeFilter="User" OnSelecting="GridDataSource_Selecting" />
                <asp:QueryExtender ID="GridQueryExtender" runat="server" TargetControlID="GridDataSource">
                    <asp:DynamicFilterExpression ControlID="FilterRepeater" />
                    <asp:SearchExpression DataFields="default" SearchType="Contains">
                        <asp:ControlParameter ControlID="TextBoxSearch" />
                    </asp:SearchExpression>
                    <asp:CustomExpression OnQuerying="FilterByEmpresa"></asp:CustomExpression>
                </asp:QueryExtender>
                <br />
                <div class="DDBottomHyperLink">
                    <asp:DynamicHyperLink ID="InsertHyperLink" runat="server" Action="Insert" Visible="">
                        <img id="Img1" runat="server" src="~/DynamicData/Content/Images/plus.gif" alt="Insert new item" />
                        Insert new item
                    </asp:DynamicHyperLink>
                </div>
            </div>
            <br></br>
            </br>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
