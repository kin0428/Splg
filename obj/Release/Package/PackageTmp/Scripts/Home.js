$(document).ready(function () {
    $(document).on("click", "#show_more_game", function (event) {
        event.preventDefault();
        $('#more_game').toggle();
    });
});