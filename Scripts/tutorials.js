var strKeyPopup1 = "splg_tutorial01";
var strKeyPopup3 = "splg_tutorial03";
var strKeyPopup5 = "splg_tutorial05";
var flgPopup1 = false;
var flgPopup3 = false;
var flgPopup5 = false;

$(document).ready(function () {

    //Close all tutorials.
    closePopupTutorial();

    $("#nextEvent").click(function () {
        if (flgPopup1 == true && flgPopup3 == false) {
            closePopupTutorial();
            var newURL = window.location.protocol + "//" + window.location.host;
            window.location.href = newURL + "/signup/";
        }
        if (flgPopup3 == true && flgPopup5 == false) {
            closePopupTutorial();
        }
        if (flgPopup5 == true) {
            closePopupTutorial();
            //Redirect to page 試合結果ページ.
            var newURL = window.location.protocol + "//" + window.location.host;
            window.location.href = newURL + "/mypage/today/";
        }
    });

    var memberID;
    //1 : Check login or not.
    $.ajax({
        type: "GET",
        url: '/Npb/NpbTop/DefineLoginOrNot',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        async: false,
        success: function (result) {
            if (result != null) {
                memberID = result;
            }
        },
    });

    var strValue;
    //1 : Non member or not login
    if (memberID == "") {     
        //1.1 : Check Cookie Popup 1 Showed or not
        if (docCookies.getItem(strKeyPopup1) == null) {
            showPopup1();
        }
    }
    //2. Member
    else {
        var expectPoint;
        $.ajax({
            type: "GET",
            url: '/Home/DefineBetFirstTimeOrNot',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (result) {
                if (result != null) {
                    expectPoint = result;
                }
            },
        });

        //2.2 Not bet anytime.
        if (expectPoint == "") {
            if (docCookies.getItem(strKeyPopup3) == null) {
                showPopup3();
            }
        }
        //2.3 When game close, show popup the first time.
        else {
            if (docCookies.getItem(strKeyPopup5) == null) {
                showPopup5();
            }
        }
    }
});

function showPopup1() {
    //Change content of popup1   
    $('#content01').html('<span class="bold">無償 </span>で付与されるポイントを使って、実際に行われるプロ野球、J リーグ、MLBなどの試合に対し勝敗予想を楽しめる、完全無料のサービスです。<br> 今までに無い、スポーツの新しい楽しみ方をご提供します！');
    $('#content02').html('<span class="bold">ポイントを貯めると？</span><br>試合予想を見事的中させ、貯めたポイントを使って次の試合に予想することができる他、「スポログ」で開催される懸賞に応募することもできます！');
    $('#content03').html('ぜひ「スポログ」にご登録（無料）いただき、スポーツ観戦を「より熱く！楽しく！」お楽しみください！');
    $('.add_image').show();

    //Show popup1
    $(".outside_overclay").show();
    $(".add_popup").show();

    flgPopup1 = true;

    //Insert cookie popup1
    docCookies.setItem(strKeyPopup1, "true", Infinity);
}

function showPopup3() {

    //Change content of popup1 -> popup2
    $('#content01').html('まずは好きな試合の勝敗を予想してみましょう！やり方は簡単、ページをスクロールして好きな試合を選び、｢予想する｣ボタンを押してみてください。');
    $('#content02').html('右の枠に予想した試合のカードが表示され、ここで使いたいポイントを変更することもできます。<br>もちろん試合中も｢スポログ｣でリアルタイムに試合速報をお楽しみいただけます。<br/>※一部競技に限ります。');
    $('#content03').html('それでは試合結果を楽しみにお待ちください！');
    $('.add_image').show();
    $('#imgPop1').attr('src', '/Content/img/tmp/tutorial_03_01_pc.gif');
    //$('#imgPop2').attr('src', '/Content/img/tmp/tutorial_03_02_pc.gif');
    $('#popTitle').html('試合予想してみましょう！');
    $('#add_image02').hide();
    $('#nextEvent a').text('閉じる');

    //Show popup1
    $(".outside_overclay").show();
    $(".add_popup").show();

    flgPopup3 = true;

    //Insert cookie popup3
    docCookies.setItem(strKeyPopup3, "true", Infinity);
}

function showPopup5() {
    //Change content of popup1 -> popup5
    $('#popTitle').html('試合終了！');
    $('#content01').html('予想した試合が終了したようです。<br>早速予想結果ページを確認してみましょう！');
    $('#content02').html('');
    $('#content03').html('');
    $('.add_image').hide();    
    $('#nextEvent a').text('予想結果ページへ');

    //Show popup5
    $(".outside_overclay").show();
    $(".add_popup").show();

    flgPopup5 = true;

    //Insert cookie popup5
    docCookies.setItem(strKeyPopup5, "true", Infinity);
}
