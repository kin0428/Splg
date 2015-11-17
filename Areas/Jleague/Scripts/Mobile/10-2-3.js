
$(document).ready(function () {

    seasonSelected();

    $("#cboTeamInfos").change(function (event) {
        cboSelected();
    });

    $("#Seasons").change(function (event) {
        seasonSelected();
    });
});

function seasonSelected() {
    var seasonID = document.getElementById("Seasons").value;
    var sessionIdStr = ".season-" + seasonID;

    $('.jrow').css('display', 'none');
    $(sessionIdStr).css('display', 'table');
    $('#resultDefault').css('display', '');

}

function cboSelected() {
    // 初期値（JリーグTOPの場合：0）
    var leagueType = 0;

    // URL取得
    var url = location.href;

    // leagueIdのセット
    // J1の場合：1
    if (url.indexOf('/j1') > 0) {
        leagueType = 2
        // J2の場合:2
    } else if (url.indexOf('/j2') > 0) {
        leagueType = 3
        // ナビスコカップの場合：3
    } else if (url.indexOf('/jleaguecup') > 0) {
        leagueType = 4
    }
    var teamID = document.getElementById("cboTeamInfos").value;
    GetResultGameInfos(teamID, leagueType);
}

function GetResultGameInfos(teamID, sJtype, seasonID) {
    $.ajax({
        url: '/Jleague/JlgResult/GetJleagueGameInfosResult/',
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ teamID: teamID, sJtype: sJtype, seasonID: seasonID }),
        success: function (result) {
            $('#resultDefault').html(result);
            seasonSelected();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed to retrieve jleague game infos result.');
        }
    });


}



