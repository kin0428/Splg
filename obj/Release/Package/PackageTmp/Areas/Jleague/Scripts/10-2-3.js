
$(document).ready(function () {

    var theLastSeasonId;

    if ($('#tab-2').hasClass('active')){
        theLastSeasonId = 2;
    }else if($('#tab-3').hasClass('active')){
        theLastSeasonId = 3;
    }else{
        theLastSeasonId = 1;
    }

    setTabs(theLastSeasonId);

    $('#tab-1').live('click', function (event) {
        setTabs(1);
    });
    $('#tab-2').live('click', function (event) {
        setTabs(2);
    });
    $('#tab-3').live('click', function (event) {
        setTabs(3);
    });

});

function setTabs(seasonId) {
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
    
    if (leagueType == 1){
        $( ".block_04_3_l3" ).tabs();

        $(".block_04_3_l3").removeClass('ui-tabs');
        $(".block_04_3_l3").removeClass('ui-widget');
        $(".block_04_3_l3").removeClass('ui-content');
        $(".block_04_3_l3").removeClass('ui-corner-a');

    }

    var sessionIdStr = ".season-" + seasonId;

    $('.jrow').hide();
    $(sessionIdStr).show();

    if ($(sessionIdStr).length > 0) {
        $(".ready_box").hide();
    }
    else {
        $(".ready_box").show();
    }

    for (var i = 2; i <= 3; i++) {
        var tabIdStr = "#tab-" + i;
        if (i == seasonId) {
            if (!$(tabIdStr).hasClass('active')) {
                $(tabIdStr).addClass('active');
            }
        } else {
            $(tabIdStr).removeClass('active');
        }
    }
}

function GetResultGameInfos(teamID, sJtype, seasonID) {
    $.ajax({
        url: '/Jleague/JlgResult/GetJleagueGameInfosResult/',
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ teamID: teamID, sJtype: sJtype, seasonID: seasonID }),
        success: function (result) {
            $('#resultDefault').html(result);
            var theLastSeasonId;

            if ($('#tab-2').hasClass('active')) {
                theLastSeasonId = 2;
            } else if ($('#tab-3').hasClass('active')) {
                theLastSeasonId = 3;
            } else {
                theLastSeasonId = 1;
            }

            setTabs(theLastSeasonId);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed to retrieve jleague game infos result.');
        }
    });


}



