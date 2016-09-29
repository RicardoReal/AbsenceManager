function pageLoad() {
    SetupFilterContainer();
}

// Toogle no campo de Search
function SetupFilterContainer() {

    $('.JQDTTextBox').datetimepicker({
        //dateFormat: 'dd-mm-yy',
        dateFormat: 'yy-mm-dd',
        hourFormat: 'hh:mm',
        ampm: false,
        buttonImage: '/AbsenceManager/Images/calendar.png',
        buttonImageOnly: true,
        buttonText: 'Open',
        showOn: 'button'
    }).inputmask("y-m-d h:s", { clearIncomplete: true });

    $('.JQDTextBox').datepicker({
        //dateFormat: 'dd-mm-yy',
        dateFormat: 'yy-mm-dd',
        ampm: false,
        buttonImage: '/AbsenceManager/Images/calendar.png',
        buttonImageOnly: true,
        buttonText: 'Open',
        showOn: 'button'
    }).inputmask("y-m-d", { clearIncomplete: true });

    $('.JQDTextBoxMapaPresenças').datepicker({
        //dateFormat: 'dd-mm-yy',
        dateFormat: 'yy-mm-dd',
        ampm: false,
        buttonImage: '/AbsenceManager/Images/calendar.png',
        buttonImageOnly: true,
        buttonText: 'Open',
        showOn: 'button'
    }).inputmask("y-m-d", { clearIncomplete: true });

    $('.JQTTextBox').timepicker({
        hourFormat: 'hh:mm',
        ampm: false,
        buttonImage: '../Images/calendar.png',
        buttonImageOnly: true,
        buttonText: 'Open',
        showOn: 'button'
    }).inputmask("h:s", { clearIncomplete: true });

    $('.DDValidator').click(function () {
        $(this).slideUp();
    });

    var d = this.status.toString();

    var selects = $('SELECT.DDFilter');

    var showFilteredLabel = false;

    for (var i = 0; i < selects.length; i++) {
        var sel = selects[i];

        if (sel.options[sel.selectedIndex].value != "")
            showFilteredLabel = true;
    }

    var radiobuttons = $('[type="radio"]');

    for (var h = 0; h < radiobuttons.length; h++) {
        var rb = radiobuttons[h];

        if (rb.value != "" && rb.checked) {
            showFilteredLabel = true;
        }
    }

    var checkboxs = $('SPAN.DDFilter INPUT');

    for (var j = 0; j < checkboxs.length; j++) {
        var cbx = checkboxs[j];

        if (cbx.checked == true)
            showFilteredLabel = true;
    }

    var textboxs = $('INPUT[type=text].DDFilter');

    for (var k = 0; k < textboxs.length; k++) {
        var tbx = textboxs[k];

        if (tbx.value != '')
            showFilteredLabel = true;
    }

    var textboxsDT = $('.JQDTTextBox');

    for (var l = 0; l < textboxsDT.length; l++) {
        var tbxDT = textboxsDT[l];

        if (tbxDT.value != '')
            showFilteredLabel = true;
    }

    //If there are filters, LabelFilter will be showed
    if (showFilteredLabel) {
        $('.DDLabelFiltered').css('display', '')
    }
    else {
        $('.DDLabelFiltered').css('display', 'none')
    }


    //    if (showFilters) {
    var showFilters = getCookie("showFilter");

    if (showFilters != "true") {
        $('.imgArrows').attr('src', '../Images/RightArrow.gif');
        $('#toggleFilters').toggle(
                            function () {
                                setCookie("showFilter", "true", 1);
                                $('#FilterValidationContainer').css('display', '');
                                $('.imgArrows').attr('src', '../Images/DownArrow.gif');
                            }, function () {
                                setCookie("showFilter", "false", 1);
                                $('#FilterValidationContainer').css('display', 'none');
                                $('.imgArrows').attr('src', '../Images/RightArrow.gif');
                            });
    }
    else {
        $('#FilterValidationContainer').show();
        $('.imgArrows').attr('src', '../Images/DownArrow.gif');
        $('#toggleFilters').toggle(
                            function () {
                                setCookie("showFilter", "false", 1);
                                $('#FilterValidationContainer').css('display', 'none');
                                $('.imgArrows').attr('src', '../Images/RightArrow.gif');
                            }, function () {
                                setCookie("showFilter", "true", 1);
                                $('#FilterValidationContainer').css('display', '');
                                $('.imgArrows').attr('src', '../Images/DownArrow.gif');
                            });
    }





    $(".water").each(function () {
        $tb = $(this);
        if ($tb.val() != this.title) {
            $tb.removeClass("water");
        }
    });

    $(".water").focus(function () {
        $tb = $(this);
        if ($tb.val() == this.title) {
            $tb.val("");
            $tb.removeClass("water");
        }
    });

    $(".water").blur(function () {
        $tb = $(this);
        if ($.trim($tb.val()) == "") {
            $tb.val(this.title);
            $tb.addClass("water");
        }
    });
}

// Tooltip da AirlineLogoes
$(document).ready(function () {

    // Get all the thumbnail
    $('div.thumbnail-item').mouseenter(function (e) {

        // Calculate the position of the image tooltip
        x = e.pageX - $(this).offset().left;
        y = e.pageY - $(this).offset().top;

        // Set the z-index of the current item, 
        // make sure it's greater than the rest of thumbnail items
        // Set the position and display the image tooltip
        $(this).css('z-index', '1500')
		.children("div.tooltip")
		.css({ 'bottom': y, 'left': x, 'display': 'block' });

    }).mouseleave(function () {

        // Reset the z-index and hide the image tooltip 
        $(this).css('z-index', '1')
		.children("div.tooltip")
		.animate({ "opacity": "hide" }, "fast");
    });

});

// Checkboxs da AirlineLogoes
function uncheckOthers(id, cbName) {
    var elm = document.getElementsByTagName('input');

    for (var i = 0; i < elm.length; i++) {
        if (id.id.lastIndexOf(cbName) >= 0 && elm.item(i).id.lastIndexOf(cbName) >= 0) {
            if (elm.item(i).id.substring(id.id.lastIndexOf('_')) != id.id.substring(id.id.lastIndexOf('_'))) {
                if (elm.item(i).type == "checkbox" && elm.item(i) != id)
                    elm.item(i).checked = false;
            }
        }
    }
}


// Menu - testes
$('.fg-button').hover(
    		function () { $(this).removeClass('ui-state-default').addClass('ui-state-focus'); },
    		function () { $(this).removeClass('ui-state-focus').addClass('ui-state-default'); }
    	);

$(document).ready(function () {
    try {
        $('#hierarchy').menu({
            content: $('#hierarchy').next().html(),
            crumbDefaultText: ' ',
            flyOut: true
        });
    }
    catch (ex) {
    }
});


$(document).ready(function () {
    try {
        $.fn.qtip.styles.themeroller = {
            background: null,
            color: null,
            border: {
                width: 4,
                radius: 2
            },
            classes: {
                tooltip: 'ui-widget',
                tip: 'ui-widget',
                title: 'ui-widget-header',
                content: 'ui-widget-content'
            }
        };

        // // Tooltip para a imagem dos filters
        $('.imgArrows').qtip({ style: { name: 'themeroller' }, position: { corner: { target: 'topRight', tooltip: 'bottomLeft'}} })
        $('.IconSweets').qtip({ style: { name: 'themeroller' }, position: { corner: { target: 'topRight', tooltip: 'bottomLeft'}} })
        $('.IconSweets2').qtip({ style: { name: 'themeroller' }, position: { corner: { target: 'topRight', tooltip: 'bottomLeft'}} })
        $('.IconSweets2left').qtip({ style: { name: 'themeroller' }, position: { corner: { target: 'topLeft', tooltip: 'bottomRight'}} })
        $('.PriorityButton').qtip({ style: { name: 'themeroller' }, position: { corner: { target: 'topRight', tooltip: 'bottomLeft'}} })
    }
    catch (ex) {
    }
});

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
};