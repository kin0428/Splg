$(document).ready(function () {

    $(".leagueId8").fadeIn();
    $(".leagueId9").fadeOut();

    //アメリカン・リーグ
    $("#ALeagueButton").click(function (event) {

        $(".leagueId8").fadeIn(0);
        $(".leagueId9").fadeOut(0);

    });

    //ナショナル・リーグ
    $("#NaLeagueButton").click(function (event) {

        $(".leagueId9").fadeIn(0);
        $(".leagueId8").fadeOut(0);

    });

});