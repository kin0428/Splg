$(document).ready(function () {
    $(".benchGroup_1").hide();
    $("#benchMembers_1").click(function (event)
    {
        event.preventDefault();        
        $(".benchGroup_1").toggle();
        $("#benchMembers_1").hide();
    });

    $(".benchGroup_2").hide();
    $("#benchMembers_2").click(function (event) {
        event.preventDefault();        
        $(".benchGroup_2").toggle();
        $("#benchMembers_2").hide();
    });

    $("#closeBenchGroup_1").click(function (event) {
        debugger;
        event.preventDefault();
        $(".benchGroup_1").toggle();
        $("#benchMembers_1").show();
        var offposition = $(".evt_bl01_3_1").offset().top - 100;
        $('html, body').animate({
            scrollTop: offposition
        }, 100);
    });

    $("#closeBenchGroup_2").click(function (event) {
        debugger;
        event.preventDefault();
        $(".benchGroup_2").toggle();
        $("#benchMembers_2").show();
        var offposition = $(".evt_bl01_3_1").offset().top -100;
        $('html, body').animate({
            scrollTop: offposition
        }, 100);
    });

    //Show popup when game end in page 8-6
    $("#npbGameText").dialog({
        title : "テキスト速報",
        autoOpen: false,
        resizable: false,
        height: 600,
        width: 800,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true        
    });

    $("#npbPopup").click(function (event) {
        $("#npbGameText").dialog("open");        
    });
});