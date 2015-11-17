
$(document).ready(function () {
    var firstDayOfMonth = "";
    // var lastDayOfMonth = "";
    var urlCommon = "/Jleague/JlgTop/ShowGameInfo";
    var strYearMonth = "";
    var year = "";
    var month = "";
    var page = $("#Page").val();

    //節ドロップダウン生成
    fillOccasiontoDropDown();

    //dropdownlist date change
    $('#ddl_occasion').change(function (event) {
        debugger;
        // 節取得
        var occasionNo = $("#ddl_occasion option:selected").text().replace("第", "").replace("節", "").replace("1stステージ　", "").replace("2ndステージ　", "").replace("予選リーグ　", "");

        //シーズン取得
        var seasonId = $("#ddl_occasion option:selected").val().substr(0, 1);

        // URL取得
        var url = location.href;

        var seasonTxt = "";
        // J1の場合：1
        if (url.indexOf('/j1') > 0)
        {
            if (seasonId == 2)
            {
                seasonTxt = "1stステージ　";
            }
            else
            {
                seasonTxt = "2ndステージ　";
            }
        }
        else if (url.indexOf('/jleaguecup') > 0)
        {
                seasonTxt = "予選リーグ　";
        }

        // hiddenの節を修正
        $("#Page").attr('data-occasion', occasionNo);
        $("#Page").attr('data-seasonid', seasonId);
        var season = document.getElementById("season");
        var octxt = document.getElementById("occasionTitle");
        season.innerHTML = seasonTxt;
        octxt.innerHTML = occasionNo;
        // 対象の節の日程・結果を表示
        getGameInfoOccsion(urlCommon, page, occasionNo, seasonId);
        event.preventDefault();
    });


    //Load div JlgGameInfoWeek after select year, month, week.
    function getGameByWeek(type, startDate, endDate) {
        var linkUrl = urlCommon + '?type=' + type + '&startDate=' + firstDayOfMonth + '&endDate=' + firstDayOfMonth;
        $('#jlgGameInfoWeek').load(linkUrl);
    }
});

// fill data to dropdownlist day
function fillOccasiontoDropDown() {
    //get id dropdownlist
    var sel = $("#ddl_occasion");
    var maxOccasionNo = $("#Page").attr('data-maxoccasionno');
    var occasionNo =  $("#Page").attr('data-occasion');
    var seasonId = $("#Page").attr('data-seasonid');
    $('#ddl_occasion option[value!="0"]').remove();
    // URL取得
    var url = location.href;

    // leagueIdのセット
    // J1の場合：1
    if (url.indexOf('/j1') > 0)
    {
        for (var r = 2; r <= 3; r++) {
            if (r == 2)
            {
                var season = "1stステージ　";
            }
            else
            {
                var season = "2ndステージ　";
            }

            for (var i = 1; i <= maxOccasionNo; i++) {
                var opt = document.createElement('option');
                opt.innerHTML =season + '第' + i + '節';
                opt.value = r + '_' + i;
                sel.append(opt);
                //check current OccasionNo
                if (i == occasionNo && r == seasonId) {
                    sel.val(r + '_' +i);
                }
            }
        }
        // J2の場合:2
    }
    else if (url.indexOf('/j2') > 0)
    {
        for (var i = 1; i <= maxOccasionNo; i++) {
            var opt = document.createElement('option');
            opt.innerHTML ='第' + i + '節';
            opt.value = '1_' + i;
            sel.append(opt);
            //check current OccasionNo
            if (i == occasionNo) {
                sel.val(i);
            }
        }
        // ナビスコカップの場合：3
    } else if (url.indexOf('/jleaguecup') > 0) {
        for (var i = 1; i <= maxOccasionNo; i++) {
            var opt = document.createElement('option');
            opt.innerHTML ='予選リーグ　第' + i + '節';
            opt.value = '4_' + i;
            sel.append(opt);
            //check current OccasionNo
            if (i == occasionNo) {
                sel.val(i);
            }
        }
    }
};

// 節の情報取得
function getGameInfoOccsion(urlCommon, type, occasion, seasonId) {
    // 初期値（JリーグTOPの場合：0）
    var leagueType = 0;

    // URL取得
    var url = location.href;

    // leagueIdのセット
    // J1の場合：1
    if (url.indexOf('/j1') > 0) {
        leagueType = 1
        // J2の場合:2
    } else if (url.indexOf('/j2') > 0) {
        leagueType = 2
        round = 0
        // ナビスコカップの場合：3
    } else if (url.indexOf('/jleaguecup') > 0) {
        leagueType = 3
        seasonId = 0
    }

    //Update for data-gamedate
    $("#Page").attr('data-occasion', occasion);
    var linkUrl;
    if (type == 1) {

        linkUrl = urlCommon + '?type=' + type + '&occasionNo=' + occasion + '&round=' + 0 + '&leagueType=' + leagueType + '&SeasonId=' + seasonId;
    }
    else if (type == 3) {
        linkUrl = urlCommon + '?type=' + type + '&occasionNo=' + occasion + '&round=' + 0 + '&teamID=' + teamID + '&SeasonId=' + seasonId;
    }
    $('#jlgGameInfo').load(linkUrl);

    $("#occasion").text(occasion);

};


