/// Get first day of week


function getDateFormatCompare(d) {
    var _month = d.getMonth() < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1);
    var _date = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
    return d.getFullYear() + '' + _month + '' + _date;
}
$(document).ready(function () {
    var urlCommon = "/Jleague/JlgTop/ShowGameInfo";
    var dt = new Date();
    var year = dt.getFullYear(); // read the current year   
    var date = new Date();
    var page = $("#Page").val();
    var teamID = $("#TeamID").val();
    var leagueType = 1;

    // 節番号取得
    var occasionNo = $("#Page").attr('data-occasion');
    // seasonId取得
    var seasonid = $("#Page").attr('data-seasonid');
    debugger;
    tglClassBlue(seasonid, occasionNo);


    //@J1関連スクリプト

    //ファーストシーズン（J1）タブクリック
    $('#1st').live('click', function (event) {
        // タブのアクティブ切り替え
        if (!$('#1st').hasClass('active')) {
            $('#1st').addClass('active');
        }
        if ($('#2nd').hasClass('active')) {
            $('#2nd').removeClass('active');
        }
        if ($('#all').hasClass('active')){
            $('#all').removeClass('active');
        }
        // SeasonIdの値修正
        $("#Page").attr('data-seasonid', 2);
        // 節リストのディスプレイ切り替え
        $('#J1Seasons').css('display', 'block');
        $('#J1AllSeason').css('display', 'none');
        // 節番号取得
        var occasionNo = $("#Page").attr('data-occasion');
        // seasonId取得
        var seasonId = $("#Page").attr('data-seasonid');
        // 変更対象の名前を設定
        var cls = "#J1Seasons p.j_rank a#season_" + occasionNo;
        // 対象の節をアクティブに、その他は非アクティブに
        $("#J1Seasons p.j_rank a.active").removeClass('active');
        $(cls).addClass('active');


        //// 前節を見る、次節を見るのスタイル修正
        tglClassBlue(seasonId, occasionNo);

        // 試合結果リストの更新
        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);
        event.preventDefault();

    });

    //セカンドシーズン（J1）タブクリック
    $('#2nd').live('click', function (event) {

        // disableクラスがついてたら処理しない
        if (!$("#2nd").hasClass('disable')) {

            // タブのアクティブ切り替え
            if ($('#1st').hasClass('active')){
                $('#1st').removeClass('active');
            }
            if (!$('#2nd').hasClass('active')){
                $('#2nd').addClass('active');
            }
            if ($('#all').hasClass('active')){
                $('#all').removeClass('active');
            }
            // ASeasonIdの値修正
            $("#Page").attr('data-seasonid', 3);
            // 節リストのディスプレイ切り替え
            $('#J1Seasons').css('display', 'block');
            $('#J1AllSeason').css('display', 'none');
            // 節番号取得
            var occasionNo = $("#Page").attr('data-occasion');
            // seasonId取得
            var seasonId = $("#Page").attr('data-seasonid');
            // 変更対象の名前を設定
            var cls = "#J1Seasons p.j_rank a#season_" + occasionNo;
            // 対象の節をアクティブに、その他は非アクティブに
            $("#J1Seasons p.j_rank a.active").removeClass('active');
            $(cls).addClass('active');

            // 前節を見る、次節を見るのスタイル修正
            tglClassBlue(seasonId, occasionNo);

            // 試合結果リストの更新
            getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);
            event.preventDefault();
        }
    });

    //通年（J1）タブクリック
    $('#all').live('click', function (event) {
        // タブのアクティブ切り替え
        if ($('#1st').hasClass('active')) {
            $('#1st').removeClass('active');
            $("#Page").attr('data-seasonid', 2);
        }
        if ($('#2nd').hasClass('active')) {
            $('#2nd').removeClass('active');
            $("#Page").attr('data-seasonid', 3);
        }
        if (!$('#all').hasClass('active')) {
            $('#all').addClass('active');
        }
        // 節リストのディスプレイ切り替え
        $('#J1Seasons').css('display', 'none');
        $('#J1AllSeason').css('display', 'block');
        // 節番号取得
        var occasionNo = $("#Page").attr('data-occasion');
        // seasonId取得
        var seasonId = $("#Page").attr('data-seasonid');
        // 対象の節をアクティブに、その他は非アクティブに
        $('#1stseason p.j_rank a.active').removeClass('active');
        $('#2ndseason p.j_rank a.active').removeClass('active');
        if (seasonId == 2) {
            // 変更対象の名前を設定
            var cls = "#1stseason p.j_rank a#1st_" + occasionNo;
        } else {
            var cls = "#2ndseason p.j_rank a#2nd_" + occasionNo;
        }
        $(cls).addClass('active');

        // 前節を見る、次節を見るのスタイル修正
        tglClassBlue(seasonId, occasionNo);

        // 試合結果リストの更新
        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);
        event.preventDefault();
    });

    //節＠J1共有部分クリック
    $('#J1Seasons p.j_rank a:not(.active)').live('click', function (event) {
        if ($('#1st').hasClass('active'))
        {
            $("#Page").attr('data-seasonid', 2);
        }
        else if ($('#2nd').hasClass('active'))
        {
            $("#Page").attr('data-seasonid', 3);
        }

        $(this).toggleClass("active");

        $(this).siblings().removeClass("active");

        var occasionNo = $(this).text().replace("第", "").replace("節", "");

        occasionNo = Number(occasionNo);

        $("#Page").attr('occasionNo', occasionNo);

        var seasonId = $("#Page").attr('data-seasonid');

        //PrevNextリンク対応
        tglClassBlue(seasonId, occasionNo);

        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);

        event.preventDefault();
    });

    //節＠通年（J1＠シーズン１）クリック
    $('#1stseason p.j_rank a:not(.active)').live('click', function (event) {
        $("#Page").attr('data-seasonid', 2);
        $('#2ndseason p.j_rank a.active').removeClass('active');
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");
        var occasionNo = $(this).text().replace("第", "").replace("節", "");
        occasionNo = Number(occasionNo);
        $("#Page").attr('occasionNo', occasionNo);
        var seasonId = $("#Page").attr('data-seasonid');
        tglClassBlue(seasonId, occasionNo);
        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);
        event.preventDefault();
    });

    //節＠通年（J1＠シーズン２）クリック
    $('#2ndseason p.j_rank a:not(.active)').live('click', function (event) {
        $("#Page").attr('data-seasonid', 3);
        $('#1stseason p.j_rank a.active').removeClass('active');
        $(this).toggleClass("active");
        $(this).siblings().removeClass("active");
        var occasionNo = $(this).text().replace("第", "").replace("節", "");
        occasionNo = Number(occasionNo);
        $("#Page").attr('occasion', occasionNo);
        var seasonId = $("#Page").attr('data-seasonid');
        tglClassBlue(seasonId, occasionNo);
        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);
        event.preventDefault();
    });

    //次節リンククリック（J1 ※共用できるかはこれから検証）
    $(".board_next").click(function (event) {
        if ($(".board_next").hasClass('blue')) {
            var maxOccasionNo = $("#Page").attr('data-maxoccasionno');
            var _nextOccasion = document.getElementById("occasion").textContent;
            _nextOccasion = Number(_nextOccasion) + 1;
            var seasonId = $("#Page").attr('data-seasonid');
            if (_nextOccasion > maxOccasionNo) {
                _nextOccasion = 1;
                seasonId = Number(seasonId) + 1;
                $("#Page").attr('data-seasonid', seasonId);
            }
            debugger;
            var leagueType = getJType();

            switch (leagueType)
            {
                case 1:
                    changeJ1Season(seasonId, _nextOccasion);
                    break;
                case 2:
                    changeJ2Season(seasonId, _nextOccasion);
                    break;
                case 3:
                    changeNabiscoSeason(seasonId, _nextOccasion);
                    break;
            }

            tglClassBlue(seasonId, _nextOccasion);
            getGameInfoOccsion(urlCommon, page, _nextOccasion, 0, seasonId);
        }
    });

    //前節リンククリック（J1 ※共用できるかはこれから検証）
    $(".board_prev").click(function (event) {
        if ($(".board_prev").hasClass('blue')) {
            var maxOccasionNo = $("#Page").attr('data-maxoccasionno');
            var _prevOccasion = document.getElementById("occasion").textContent;
            _prevOccasion = Number(_prevOccasion) - 1;
            var seasonId = $("#Page").attr('data-seasonid');
            if (_prevOccasion < 1) {
                _prevOccasion = maxOccasionNo;
                seasonId = Number(seasonId) - 1;
                $("#Page").attr('data-seasonid', seasonId);
            }

            var leagueType = getJType();

            switch (leagueType) {
                case 1:
                    changeJ1Season(seasonId, _prevOccasion);
                    break;
                case 2:
                    changeJ2Season(seasonId, _prevOccasion);
                    break;
                case 3:
                    changeNabiscoSeason(seasonId, _prevOccasion);
                    break;
            }

            tglClassBlue(seasonId, _prevOccasion);
            getGameInfoOccsion(urlCommon, page, _prevOccasion, 0, seasonId);
        }
    });

    //@J2関連スクリプト

    //Todo:J2タブ（現在時点では実装不要）

    //節＠J2クリック
    $('#J2Season p.j_rank a:not(.active)').live('click', function (event)
    {
        $(this).toggleClass("active");

        $(this).siblings().removeClass("active");

        var occasionNo = $(this).text().replace("第", "").replace("節", "");

        occasionNo = Number(occasionNo);

        $("#Page").attr('occasionNo', occasionNo);

        var seasonId = $("#Page").attr('data-seasonid');

        //PrevNextリンク対応
        tglClassBlue(seasonId, occasionNo);

        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);

        event.preventDefault();
    });

    //@Nabisco関連スクリプト

    //Todo:Nabiscoタブ（決勝トーナメント実装時に対応する必要有）

    //節＠Nabiscoクリック
    $('#NabiscoSeasons p.j_rank a:not(.active)').live('click', function (event) {
        $(this).toggleClass("active");

        $(this).siblings().removeClass("active");

        var occasionNo = $(this).text().replace("第", "").replace("節", "");

        occasionNo = Number(occasionNo);

        $("#Page").attr('occasionNo', occasionNo);

        var seasonId = $("#Page").attr('data-seasonid');

        //PrevNextリンク対応
        tglClassBlue(seasonId, occasionNo);

        getGameInfoOccsion(urlCommon, page, occasionNo, 0, seasonId);

        event.preventDefault();
    });

    //@メソッド群

    // 節の情報取得
    function getGameInfoOccsion(urlCommon, type, occasion, round,seasonId) {
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
            // ナビスコカップの場合：3
        } else if (url.indexOf('/jleaguecup') > 0) {
            leagueType = 3
        }

        //Update for data-gamedate
        $("#Page").attr('data-occasion', occasion);
        var linkUrl;

        if (type == 1)
        {
            linkUrl = urlCommon + '?type=' + type + '&occasionNo=' + occasion + '&round=' + round + '&leagueType=' + leagueType + "&SeasonId=" + seasonId;
        }
        else if (type == 3)
        {
            linkUrl = urlCommon + '?type=' + type + '&occasionNo=' + occasion + '&round=' + round + '&teamID=' + teamID + "&SeasonId=" + seasonId;
        }

        $('#jlgGameInfo').load(linkUrl);

        $("#occasion").text(occasion);

    };

    //
    function tglClassBlue(seasonId, occasionNo) {
        var maxOccasionNo = $("#Page").attr('data-maxoccasionno');

        var leagueType = getJType();

        //J1
        if (leagueType == 1)
        {
            if (seasonId == 2 && occasionNo == 1)
            {
                //1stシーズン　1節
                $(".board_prev").removeClass('blue');
                if (!$(".board_next").hasClass('blue'))
                {
                    $(".board_next").addClass('blue');
                }
            }
            else if (seasonId == 3 && occasionNo == maxOccasionNo)
            {
                //2ndシーズン 最終節
                $(".board_next").removeClass('blue');
                if (!$(".board_prev").hasClass('blue'))
                {
                    $(".board_prev").addClass('blue');
                }
            }
            else if (!$(".board_next").hasClass('blue'))
            {
                //次へボタン活性時処理
                $(".board_next").addClass('blue');
                if (!$(".board_prev").hasClass('blue'))
                {
                    $(".board_prev").addClass('blue');
                }
            }
            else if (!$(".board_prev").hasClass('blue'))
            {
                //前へボタン活性時処理
                $(".board_prev").addClass('blue');
                if (!$(".board_next").hasClass('blue'))
                {
                    $(".board_next").addClass('blue');
                }
            }

        } else {
            if (occasionNo <= 1) {
                $(".board_prev").removeClass('blue');
                if (!$(".board_next").hasClass('blue')) {
                    $(".board_next").addClass('blue');
                }
            } else if (occasionNo >= maxOccasionNo) {
                $(".board_next").removeClass('blue');
                if (!$(".board_prev").hasClass('blue')) {
                    $(".board_prev").addClass('blue');
                }
            } else if (!$(".board_next").hasClass('blue')) {
                $(".board_next").addClass('blue');
                if (!$(".board_prev").hasClass('blue')) {
                    $(".board_prev").addClass('blue');
                }
            } else if (!$(".board_prev").hasClass('blue')) {
                $(".board_prev").addClass('blue');
                if (!$(".board_next").hasClass('blue')) {
                    $(".board_next").addClass('blue');
                }
            }
        }
    };

    //JType取得
    function getJType() {
        // URL取得
        var url = location.href;

        // J1：1
        if (url.indexOf('/j1') > 0) {
            return 1;
            // J2:2
        }
        else if (url.indexOf('/j2') > 0) {
            return 2;
            // Nabisco：3
        }
        else if (url.indexOf('/jleaguecup') > 0) {
            return 3;
        }
    };


    // 次節、前節移動時の表示
    function changeJ1Season(seasonId,occasion) {

        //Update for data-gamedate
        if ($("#all").hasClass('active')) {
            if (seasonId == 2)
            {
                // 変更対象の名前を設定
                var cls = "#1stseason p.j_rank a#1st_" + occasion;
            }
            else
            {
                var cls = "#2ndseason p.j_rank a#2nd_" + occasion;
            }

        }
        else
        {
            // 変更対象の名前を設定
            var cls = "#J1Seasons p.j_rank a#season_" + occasion;
            if (seasonId == 2) {
                // タブのアクティブ切り替え
                if (!$('#1st').hasClass('active'))
                {
                    $('#1st').addClass('active');
                }
                if ($('#2nd').hasClass('active'))
                {
                    $('#2nd').removeClass('active');
                }
                if (!$('#all').hasClass('active'))
                {
                    $('#all').removeClass('active');
                }

            }
            else
            {
                // タブのアクティブ切り替え
                if ($('#1st').hasClass('active'))
                {
                    $('#1st').removeClass('active');
                }
                if (!$('#2nd').hasClass('active'))
                {
                    $('#2nd').addClass('active');
                }
                if (!$('#all').hasClass('active'))
                {
                    $('#all').removeClass('active');
                }
            }
        }

        // アクティブ全削除
        $('#1stseason p.j_rank a.active').removeClass('active');
        $('#2ndseason p.j_rank a.active').removeClass('active');
        $('#J1Seasons p.j_rank a.active').removeClass('active');

        // 対象アクティブ設定
        $(cls).addClass('active');

    };

    // 次節、前節移動時の表示(J2)
    function changeJ2Season(seasonId, occasion)
    {    
        // 変更対象節設定
        var cls = "#J2Season p.j_rank a#season_" + occasion;

        // アクティブ全削除
        $('#J2Season p.j_rank a.active').removeClass('active');

        // 対象アクティブ設定
        $(cls).addClass('active');
    };

    // 次節、前節移動時の表示(Nabisco)
    function changeNabiscoSeason(seasonId, occasion) {

        // 変更対象節設定
        var cls = "#NabiscoSeasons p.j_rank a#season_" + occasion;

        // アクティブ全削除
        $('#NabiscoSeasons p.j_rank a.active').removeClass('active');

        // 対象アクティブ設定
        $(cls).addClass('active');

    };
});