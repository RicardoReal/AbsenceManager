﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="AbsenceManager.AutocompleteField_Edit"
    CodeBehind="Autocomplete_Edit.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:TextBox runat="server" ID="AutocompleteTextBox" Width="300px" 
    autocomplete="off" CssClass="DDFilter Autocomplete"
    ontextchanged="AutocompleteTextBox_TextChanged" />
<asp:HiddenField runat="server" ID="AutocompleteValue" />
<ajaxToolkit:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteBehaviorIDPlaceholder"
    ID="autoComplete1" TargetControlID="AutocompleteTextBox" ServicePath="~\AutocompleteField.asmx"
    ServiceMethod="GetCompletionList" MinimumPrefixLength="0" CompletionInterval="750"
    EnableCaching="true" CompletionSetCount="10" CompletionListCssClass="Autocomplete_CompletionList"
    CompletionListItemCssClass="Autocomplete_CompletionListItem" FirstRowSelected="true"
    CompletionListHighlightedItemCssClass="Autocomplete_CompletionListHighlightedItem"
    DelimiterCharacters=";, :">
    <Animations>
        <OnShow>
            <Sequence>
                <%--Make the completion list transparent and then show it --%>
                <OpacityAction Opacity="0" />
                <HideAction Visible="true" />
                
                <%--Cache the original size of the completion list the first time
                    the animation is played and then set it to zero --%>
                <ScriptAction Script="
                    // Cache the size and setup the initial size
                    var behavior = $find('AutoCompleteBehaviorIDPlaceholder');
                    if (!behavior._height) {
                        var target = behavior.get_completionList();
                        behavior._height = target.offsetHeight - 2;
                        target.style.height = '0px';
                    }" />
                
                <%--Expand from 0px to the appropriate size while fading in--%>
                <Parallel Duration=".4">
                    <FadeIn />
                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteBehaviorIDPlaceholder')._height" />
                </Parallel>
            </Sequence>
        </OnShow>
        <OnHide>
            <%--Collapse down to 0px and fade out--%>
            <Parallel Duration=".4">
                <FadeOut />
                <Length PropertyKey="height" StartValueScript="$find('AutoCompleteBehaviorIDPlaceholder')._height" EndValue="0" />
            </Parallel>
        </OnHide>
    </Animations>
</ajaxToolkit:AutoCompleteExtender>
<script type="text/javascript">
    // Work around browser behavior of "auto-submitting" simple forms
    var frm = document.getElementById("aspnetForm");
    if (frm) {
        frm.onsubmit = function () { return false; };
    }
</script>
<%-- Prevent enter in textbox from causing the collapsible panel from operating --%>
<input type="submit" style="display: none;" />

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="DDControl DDValidator" ControlToValidate="AutocompleteTextBox" Display="Static" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="DDControl DDValidator" ControlToValidate="AutocompleteTextBox" Display="Static" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="DDControl DDValidator" ControlToValidate="AutocompleteTextBox" Display="Static" />