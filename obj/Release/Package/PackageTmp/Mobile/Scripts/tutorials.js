var strKeyPopup1 = "splg_tutorial01";
var strKeyPopup2 = "splg_tutorial02";
var strKeyPopup3 = "splg_tutorial03";
var strKeyPopup5 = "splg_tutorial05";
var flgPopup1 = false;
var flgPopup2 = false;
var flgPopup3 = false;
var flgPopup4 = false;
var flgPopup5 = false;
var numClick = 0;

$(document).ready(function () {
    closePopupTutorial();

    $("#nextPopupSp").click(function () {
        numClick++;       
        if (flgPopup1 == true && flgPopup2 == false) {
            showPopup2();
        }
        if (flgPopup1 == true && flgPopup2 == true) {
            if (numClick == 2)
            {
                closePopupTutorial();             
                var newURL = window.location.protocol + "//" + window.location.host;
                window.location.href = newURL + "/signup/";
            }                
        }
        if((flgPopup3 == true && flgPopup5 == false) || flgPopup4 == true )
        {
            closePopupTutorial();
        }
        if(flgPopup5 == true)
        {
            if ($('#nodisplay').prop('checked')) {
                //Insert cookie popup1
                docCookies.setItem(strKeyPopup5, "true", Infinity);
            }

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
        //1.2 : Check Cookie Popup 1 Showed or not
        if (docCookies.getItem(strKeyPopup1) == null) {           
            showPopup1();
        }
    }
    //2 : Member
    else {
        //Reset numclick
        numClick = 0;

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
            var isExistsFinishedGame = false;
            $.ajax({
                type: "GET",
                url: '/Home/IsExistsFinishedGame',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                async: false,
                success: function (result) {
                    if (result != null) {
                        isExistsFinishedGame = result;
                    }
                },
            });

            if (isExistsFinishedGame == true && docCookies.getItem(strKeyPopup5) == null) {
            //if (docCookies.getItem(strKeyPopup5) == null) {
                showPopup5();
            }
        }
    }
});

function showPopup1() {

    $('#nodisplay').hide();

    //Change content of popup1   
    $('#contentSp01').html('無償で付与されるポイントを使って、実際に行われるプロ野球、J リーグ、MLBなどの試合に対し勝敗予想を楽しめる、完全無料のサービスです。<br>今までに無い、新しいスポーツの楽しみ方をご提供します！');
    $('#contentSp02').html('');
    $('#imgTutorial01').show();
    
    //Show popup1
    $(".outside_overclay").show();
    $(".tutorial_popup").show();

    flgPopup1 = true;

    //Insert cookie popup1
    docCookies.setItem(strKeyPopup1, "true", Infinity);
}

function showPopup2() {   
    if (docCookies.getItem(strKeyPopup2) == null) {

        $('#nodisplay').hide();

        //Change content of popup1   
        $('#contentSp01').html('見事試合予想を的中させポイントが増えると、次の試合予想にも使える他、「スポログ」で開催される懸賞に応募することもできます！');
        $('#contentSp02').html('ぜひ「スポログ」にご登録（無料）いただき、スポーツ観戦を「より熱く！楽しく！」お楽しみください！');
        $('#imgTutorial01').show();
        $('#imgTutorial01 img').attr('src', '/Content/img/tmp/tutorial_02_sp.gif');
        $('#popTitleSp').html('ポイントを貯めると？');
        $('#nextPopupSp a').text('ユーザー登録（無料）');

        //Show popup2
        $(".outside_overclay").show();
        $(".tutorial_popup").show();

        flgPopup2 = true;

        //Insert cookie popup2
        docCookies.setItem(strKeyPopup2, "true", Infinity);
    }
}

function showPopup3()
{  
    $('#nodisplay').hide();

    //Change content of popup3   
    $('#contentSp01').html('まずは好きな試合の勝敗を予想してみましょう！<br>やり方は簡単、ページをスクロールして好きな試合を選び、｢予想する｣ボタンを押してみてください。');
    $('#contentSp02').html('');
    $('#imgTutorial01').show();
    $('#imgTutorial01 img').attr('src', '/Content/img/tmp/tutorial_03_sp.gif');
    $('#popTitleSp').html('試合予想してみましょう！');
    $('#nextPopupSp a').text('閉じる');   

    //Show popup3
    $(".outside_overclay").show();
    $(".tutorial_popup").show();

    flgPopup3 = true;

    //Insert cookie popup1
    docCookies.setItem(strKeyPopup3, "true", Infinity);
}

function showPopup5()
{
    //Change content of popup5   
    $('#contentSp01').html('予想した試合が終了したようです。<br>早速予想結果ページを確認してみましょう！');
    $('#contentSp02').html('');
    $('#imgTutorial01').hide();   
    $('#popTitleSp').html('試合終了！');
    $('#nextPopupSp a').text('予想結果ページへ');

    //Show popup5
    $(".outside_overclay").show();
    $(".tutorial_popup").show();

    flgPopup5 = true;
}