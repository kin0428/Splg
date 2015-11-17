function GetResultGameInfos(teamID, teamsOpponentCD) {
    $('#resultDefault').css('display','none');
    $.ajax({
        url: '/Npb/NpbTeamInfoConfrontationResult/GetTeamStatsCardDifferenceInfos/',
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ teamID: teamID, teamsOpponentCD: teamsOpponentCD }),
        success: function (result) {
            $('#resultAjax').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed to retrieve TeamStatsCardDifferenceInfos.');
        }
    });
}
function GetResultGameInfosByMonth(teamID, teamsOpponentCD, month) {   
    $('#resultDefaultGameInfos').css('display', 'none');    
    $.ajax({
        url: '/Npb/NpbTeamInfoConfrontationResult/GetResultGameInfosByMonth/',
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ teamID: teamID, teamsOpponentCD: teamsOpponentCD , month : month}),
        success: function (result) {
            $('#resultAjaxGameInfos').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed to retrieve GetResultGameInfosByMonth.');
        }
    });
}
