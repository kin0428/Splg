
$(document).ready(function () {

    seasonSelected();

    $('#Seasons').change(function (event) {
        seasonSelected();
    });

    function seasonSelected() {
        var seasonID = document.getElementById("Seasons").value;
        var sessionIdStr = ".season-" + seasonID;

        $('.team-list').css('display', 'none');
        $(sessionIdStr).css('display', 'table');

    }


});


