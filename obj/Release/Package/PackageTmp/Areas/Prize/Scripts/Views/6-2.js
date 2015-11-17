$(document).ready(function () {

    // メッセージ表示の設定
    //-------------------------
    //toastr.options = {
    //    "positionClass": "toast-top-center",
    //    "preventDuplicates": true
    //}

    // サービス呼び出しURL定義
    //------------------------

    //右カラムの現在の所持ポイントロードサービス
    var linkUrl = "/RightContent/ShowExpectedPoints?showCurrentPointInfoOnly=true";

    //「この商品と交換する」クリック　所持ポイントを使用して景品を申し込む
    $(".BuyButton").live('click', function (event) { 
        event.preventDefault();
        var memberID;
        winH = $("body").height();
        winW = $("body").width();
        $.ajax({
            type: "GET",
            url: '/Npb/NpbTop/DefineLoginOrNot',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache : false,
            async: false,
            success: function (result) {
                if (result != null) {
                    memberID = result;
                }
            },
        });

        //User not login : show popup login.
        if (memberID == "") {
            $('#overlay').css('height', winH);
            $('#overlay').css('width', winW);
            $("#overlay").show(300);
        }
            //User login : insert data to db.
        else {
            $(".adw_overlay").css('height', winpH);
            $(".adw_overlay").css('width', winpW);
            $(".adw_overlay").css("display", "block");
            $(".adw_popup2a").css("display", "block");
        }

    });

    //「この商品と交換する」クリック　所持ポイントを使用して抽選に応募する
    $(".DrawButton").live('click', function (event) {
        event.preventDefault();
        var memberID;
        winH = $("body").height();
        winW = $("body").width();
        $.ajax({
            type: "GET",
            url: '/Npb/NpbTop/DefineLoginOrNot',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache : false,
            async: false,
            success: function (result) {
                if (result != null) {
                    memberID = result;
                }
            },
        });

        //User not login : show popup login.
        if (memberID == "") {
            $('#overlay').css('height', winH);
            $('#overlay').css('width', winW);
            $("#overlay").show(300);
        }
            //User login : insert data to db.
        else {
            $(".adw_overlay").css('height', winpH);
            $(".adw_overlay").css('width', winpW);
            $(".adw_overlay").css("display", "block");
            $(".adw_popup2a").css("display", "block");
        }

    });

    // 応募関連
    $(document).ready(function () {
        winpH = $("body").height();
        winpW = $("body").width();
        $(".adw_overlay").css('height', winpH);
        $(".adw_overlay").css('width', winpW);

        // 閉じるボタン
        $(".btn_close").click(function () {
            $(".adw_overlay").css("display", "none");
            $(".adw_popcontent").css("display", "none");
            // 読み込みなおし
            location.reload();
        });

        // キャンセルボタン
        $(".adw_cancel").click(function () {
            $(".adw_popcontent").css("display", "none");
            $(".adw_overlay").css("display", "block");
            $(".adw_popup_cancel").css("display", "block");
        });

        // 確認ボタン
        $(".adw_step1").click(function () {
            // エラーチェック
            // 入力した数値を取得(口数)
            var entryCount = $("#inputEntryCount").val();

            // URL取得
            var url = location.href;
            var arrUrl = url.split("/");
            var rallyGoodId = arrUrl[5];

            var errMsg = "";
            $.ajax({
                type: "POST",
                url: '/prize/IsEntry/' + rallyGoodId + '/' + entryCount + '/',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: false,
                success: function (result) {
                    if (result != null) {
                        errMsg = result;
                    }
                },
            });

            if (errMsg == "")
            {
                // 使用可能ポイント取得
                var availablePoints = $("#inputEntryCount").attr("data-availablepoint");
                // 1口の必要ポイント取得
                var entryCost = $("#inputEntryCount").attr("data-entrycost");

                // 使用ポイントの算出
                var cost = Number(entryCost) * Number(entryCount)

                // 口数の数値変更
                $("#entryCount").text(entryCount);

                // 使用ポイントの数値変更
                $("#entryPoint").text(cost);

                // 使用可能ポイント変更
                $("#afterAvailablePoint").text(Number(availablePoints) - Number(cost));

                $(".adw_popup2a").css("display", "none");
                $(".adw_overlay").css("display", "block");
                $(".adw_popup2b").css("display", "block");
            }
            else
            {
                // 取得したエラーメッセージ表示
                $("#errMsg").text(errMsg);

                $(".adw_popup2a").css("display", "none");
                $(".adw_overlay").css("display", "block");
                $(".adw_popup1").css("display", "block");
            }

        });


        // 応募するボタン
        $(".adw_step2").click(function () {
            entryPrize();
        });
    });
});


    //現在の所持ポイントの取得処理
    //-------------------------------
    function getPossessionPoints() {

        var points = 0;

        "prize/GetPossessionPoints/";
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                if (result == null) {
                    //toastr.error("所持ポイントをできませんでした。" + result.Message);
                } else {
                    var points = result.PosessionPoints;
                }
            }
        });

        return 0;
    }

    // 要件としてないのでコメントアウト
    //// 「あとXXポイント」の更新処理
    ////-----------------------------------------------
    //function setAvailablePoints() {

    //    //UI
    //    var morePointsForBuy = $("#morePointsForBuy");
    //    var morePointsForDraw = $("#morePointsForDraw");

    //    var points = getPossessionPoints();
    //    var availablePoints = points - 100000;
    //    if (availablePoints < 0) availablePoints = 0;

    //    //TODO
    //    // availablePoints >= PointsForXXX
    //    // 文言「申し込めます！」など？
    //    //CanBuyButtonをshow(); 
    //    //CannotBuyButtonをhide(); 

    //    // TODO
    //    //availablePoints < PointsForXXX
    //    // 文言「あとXXポイント」
    //    // PointsForBuy - (availablePoints)
    //    // PointsForDraw - (availablePoints)
    //    //CanBuyButtonをhide(); 
    //    //CannotBuyButtonをshow(); 
    //}

    // Todo:今回未実装
    //右カラム予想による所持ポイント変動時に呼ばれるよう登録する。
    //（~/Scripts/common.jsで所持ポイントに変動があったとき、登録したハンドラーが実行される）
    //
    // ★そのため、common.jsを６－２の前にロードすること！！
    //
    //　common.jsもデバッグお願いします！！
    //------------------------------------------------------------------------------
    //var handlers = SpologApp.possessionPointChangeHandlers;
    //handlers[handlers.length - 1] = setAvailablePoints;


    // 所持ポイントを使用して景品を申し込む
    //-----------------------------------------------------
    function entryPrize(event) {

        // ラリーグッズIDを取得
        var url = location.href;
        var arrUrl = url.split("/");
        var rallyGoodId = arrUrl[5];

        // 入力した数値を取得(口数)
        var entryCount = $("#inputEntryCount").val();

        // 応募種別を取得
        var entryMethod = $("#inputEntryCount").attr("data-entrymethod");

        $.ajax({
            type: "POST",
            url: '/prize/EntryPrize/' + rallyGoodId + '/' + entryCount + '/' + entryMethod + '/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (result) {
                if (result == true) {
                    $(".adw_popup2b").css("display", "none");
                    $(".adw_overlay").css("display", "block");
                    $(".adw_popup2c").css("display", "block");
                }
                else {
                    var errMsg = "応募処理中にエラーが発生しました。";
                    $("#errMsg").text(errMsg);

                    $(".adw_popup2b").css("display", "none");
                    $(".adw_overlay").css("display", "block");
                    $(".adw_popup1").css("display", "block");
                }
            },
        });
        
    }




