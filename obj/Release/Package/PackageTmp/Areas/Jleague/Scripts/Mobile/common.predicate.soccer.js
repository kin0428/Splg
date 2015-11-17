var urlPoint = "/Home/ShowPointAlert";

$(document).ready(function () {

    $(document).on('click', '.panel-btn1:not(.disable)', function (event) {
        event.preventDefault();
        var memberID;
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

        //User not login : show popup login.
        if (memberID == "") {
            $(".overlay").show(300);
            $('html, body').animate({
                scrollTop: $('.head-logo').offset().top
            }, 500);
            $('.head-link').css('display', 'none');
        }
            //User login : insert data to db.
        else {
            var odd = $(this).attr('data-odd');
            if (odd == "0") {
                alert("Can not bet");
            }
            else {

                //Get total point
                var possesionPoint;
                $.ajax({
                    type: "GET",
                    url: "/Npb/NpbTop/GetPossesionPoint",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    async: false,
                    success: function (point) {
                        if (point != null) {
                            possesionPoint = point;
                        }
                    }
                });

                if (possesionPoint < 100) {
                    alert("予想には100ポイント以上必要です。");
                    return;
                }

                var isBet = $(this).hasClass('change');
                if (!isBet) {
                    //Get attribute for item click.
                    var id = $(this).attr('data-id');
                    var type = $(this).attr('data-type');
                    var expectTarget = $(this).attr('data-expectTarget');
                    var team = $(this).attr('data-team');
                    var gameDate = $(this).attr('data-gameDate');
                    var isChange;
                    var sportID = $(this).attr('data-sport');
                    var reloadurl = encodeURIComponent($(this).attr('data-url'));
                    var reloadarea = $(this).attr('data-reloadarea');

                    //Start prediction.
                    $.ajax({
                        type: "POST",
                        url: '/Npb/NpbTop/StartPrediction',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ expectTargetID: expectTarget, gameID: id, betSelectID: type, memberID: memberID, teamID: team, gameDate: gameDate }),
                        async: false,
                        success: function (result) {
                            if (result == true) {
                                isChange = result;
                            }
                        },
                    });

                    if (isChange == true) {
                        //Change color for button and title.
                        $(this).addClass("change");
                        $(this).text("予想中");
                        $(this).parent().find('p').addClass("green");
                        $(this).parent().find('p').removeClass("orange");
                        $(this).parent().siblings('.panel-col5').addClass("gray1");
                        $(this).parent().siblings('.panel-col5').find('p').removeClass("orange");
                        $(this).parent().siblings('.panel-col5').find('p').addClass("gray1");
                        $(this).parent().siblings('.panel-col5').find('.panel-btn1').addClass("disable");

                        //Show popup to edit point.
                        var target = $(this).attr('data-target');
                        var linkUrl = '/Npb/NpbRightContent/EditPointInMobile?sportID=' + sportID + '&gameID=' + id + '&expectTargetID=' + target + '&betSelectID=' + type + '&memberID=' + memberID + '&teamID=' + team + '&reloadurl=' + reloadurl + '&reloadarea=' + reloadarea;
                        $('#PointEdit').load(linkUrl);
                        $(".overlay_point").show(300);
                        $('html, body').animate({
                            scrollTop: $('.head-logo').offset().top
                        }, 500);
                        $('#inputPoint').focus();
                    } else {
                        alert("処理に失敗しました。");
                    }
                }
            }

        }
    });

    $(document).on('keypress', '.pointBetMobile', function (event) {
        var code = event.keyCode || event.which;
        if (code == 13) { //Enter keycode  
            $(".pointBetMobile").blur();
        }
        if (this.value.length > 5) {
            //Don't input
        }
        //Number is valid.
        return !(code != 8 && code != 0 &&
                (code < 48 || code > 57) && code != 46);
    });

    $(document).on('blur', '.pointBetMobile', function () {
        var newPoint = parseInt($(this).val()) * 100;
        var oldPoint = $(this).attr('data-old');
        var totalPoint = $('#totalPoint').text();
        var actualTotalPoint = 0;

        if (newPoint == 0) {
            alert("予想には100ポイント以上必要です。");
            $(this).val(oldPoint / 100);
        }
        else if (newPoint > 1000000) {
            alert("ひとつの予想にかけられる上限は100万ポイントまでです。");
            $(this).val(oldPoint / 100);
        }
        else {
            actualTotalPoint = parseInt(totalPoint) + parseInt(oldPoint);
            if (newPoint > actualTotalPoint) {
                alert("所持ポイント以上は入力できません。");
                $(this).val(oldPoint / 100);
            }
            else if (newPoint == oldPoint) {
                //Not change.
            }
        }
    });

    //Update prediction and close popup Edit.
    $(document).on('click', '.my-btn02', function (event) {
        event.preventDefault();
        var newPoint = parseInt($('.pointBetMobile').val()) * 100;
        var oldPoint = $('.pointBetMobile').attr('data-old');
        var totalPoint = $('#totalPoint').text();
        var actualTotalPoint = 0;
        var newTotal;
        var reloadurl = $(this).attr('data-reloadurl');
        var reloadarea = '#' + $(this).attr('data-reloadarea');

        if (newPoint == 0) {
            alert("予想には100ポイント以上必要です。");
            $(this).val(oldPoint / 100);
        }
        else if (newPoint > 1000000) {
            alert("ひとつの予想にかけられる上限は100万ポイントまでです。");
            $(this).val(oldPoint / 100);
        }
        else {
            actualTotalPoint = parseInt(totalPoint) + parseInt(oldPoint);
            if (newPoint > actualTotalPoint) {
                alert("所持ポイント以上は入力できません。");
                $(this).val(oldPoint / 100);
            }
            else if (newPoint == oldPoint) {
                //Not change.
                $('#PosessionPoint').load(urlPoint);
                $(".overlay_point").hide(300);
                $(reloadarea).load(reloadurl);
            }
            else {
                var remainPoint = actualTotalPoint - newPoint;
                if (remainPoint >= 0) {
                    var point = $('.pointBetMobile').attr('data-point');
                    var expectPoint = $('.pointBetMobile').attr('data-expectpoint');
                    //Continue update to db
                    $.ajax({
                        type: "POST",
                        url: '/Npb/NpbRightContent/UpdatePrediction',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ expectPointID: expectPoint, pointID: point, newPoint: newPoint, oldPoint: oldPoint }),
                        async: false,
                        success: function (result) {
                            if (result != null) {
                                newTotal = result;
                                $('#PosessionPoint').load(urlPoint);
                                $(reloadarea).load(reloadurl);
                            }
                        },
                    });
                }
                else {
                    //Don't enough point, set value to old value.                
                    $('.pointBetMobile').val(oldPoint);
                }
                if (newTotal != null) {
                    //Update data-old
                    var tmp = parseInt($('.pointBetMobile').attr('value')) * 100;
                    $('.pointBetMobile').attr('data-old', tmp);
                    //Update totalpoint
                    $('#totalPoint').text(newTotal);
                    $(".overlay_point").hide(300);
                }
            }
            if ($('#popTitleSp').length != 0) {
                showPopup4();
            }
        }
    });

    function showPopup4() {
        var strKeyPopup4 = "splg_tutorial04";
        if (docCookies.getItem(strKeyPopup4) == null) {
            //Change content of popup3   
            $('#contentSp01').html('簡単でしたね！もちろん試合中も｢スポログ｣でリアルタイムに試合速報をお楽しみいただけます。<br>※一部競技に限ります。');
            $('#contentSp02').html('それでは試合結果を楽しみにお待ちください！');
            $('#imgTutorial01').hide();
            $('#popTitleSp').html('予想完了！');
            $('#nextPopupSp a').text('閉じる');

            //Show popup4
            $(".outside_overclay").show();
            $(".tutorial_popup").show();

            flgPopup4 = true;

            //Insert cookie popup1
            docCookies.setItem(strKeyPopup4, "true", Infinity);
        }
    }

    // Update Predication
    $(document).on('keypress', '.pointBetMobileUpdate', function (event) {
        var code = event.keyCode || event.which;
        if (code == 13) { //Enter keycode  
            $(this).blur();
        }
    });

    $(document).on('blur', '.pointBetMobileUpdate', function () {
        updatePoint(this);
    });

    function updatePoint(textbox) {
        var memberId = $("#txtMemberId").attr('value');

        var newStatus = 0;
        var sportId = $(textbox).attr('data-sport');
        var id = $(textbox).attr('data-gameid');
        var totalPoint = 0;
        var reloadurl = $(textbox).attr('data-url');
        var reloadarea = '#' + $(textbox).attr('data-reloadarea');

        $.ajax({
            type: "POST",
            url: '/Npb/NpbTop/GetStatusByGameID',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ sportID: sportId, gameID: id, memberID: memberId }),
            cache: false,
            async: false,
            success: function (status) {
                if (status != null) {
                    newStatus = status;
                }
            }
        });

        //Get total point
        $.ajax({
            type: "GET",
            url: '/Npb/NpbTop/GetPossesionPoint',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (point) {
                if (point != null) {
                    totalPoint = point;
                }
            }
        });

        if ((sportId == 1 && newStatus >= 1) || ((sportId == 2 || sportId == 3) && newStatus >= 4)) {
            alert("締切り時刻を過ぎたため変更できません。");
        }
        else {
            var newPoint = parseInt($(textbox).val().split(',').join('').trim()) * 100;
            var oldPoint = $(textbox).attr('data-old');
            var actualTotalPoint = 0;
            var newTotal;

            debugger;

            if (newPoint == 0) {
                alert("予想には100ポイント以上必要です。");
                $(textbox).val(oldPoint / 100);
            }
            else if (newPoint > 1000000) {
                alert("ひとつの予想にかけられる上限は100万ポイントまでです。");
                $(textbox).val(oldPoint / 100);
            }
            else {
                actualTotalPoint = totalPoint + parseInt(oldPoint);
                if (newPoint > actualTotalPoint) {
                    alert("所持ポイント以上は入力できません。");
                    $(textbox).val(oldPoint / 100);
                }
                else if (newPoint == oldPoint) {
                    //Not change.
                }
                else {
                    var remainPoint = actualTotalPoint - newPoint;
                    if (remainPoint >= 0) {
                        var pointId = $(textbox).attr('data-point');
                        var expectPoint = $(textbox).attr('data-expectpoint');
                        //Continue update to db
                        $.ajax({
                            type: "POST",
                            url: '/Npb/NpbRightContent/UpdatePrediction',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ expectPointID: expectPoint, pointID: pointId, newPoint: newPoint, oldPoint: oldPoint }),
                            async: false,
                            success: function (result) {
                                if (result != null) {
                                    newTotal = result;
                                }
                            }
                        });
                    }
                    else {
                        //Don't enough point, set value to old value.                
                        $(textbox).val(oldPoint / 100);
                    }
                    if (newTotal != null) {

                        //Update data-old
                        var tmp = parseInt($(textbox).attr('value')) * 100;
                        $(textbox).attr('data-old', tmp);
                        // reload
                        $(reloadarea).load(reloadurl);

                    }
                }
            }
        }
    }

});
