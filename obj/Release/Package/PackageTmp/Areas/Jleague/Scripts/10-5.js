$(document).ready(function () {
    $(".benchGroup_1").hide();
    $("#benchMembers_1").click(function (event)
    {
        $(".benchGroup_1").toggle();
    });

    $(".benchGroup_2").hide();
    $("#benchMembers_2").click(function (event) {
        $(".benchGroup_2").toggle();
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