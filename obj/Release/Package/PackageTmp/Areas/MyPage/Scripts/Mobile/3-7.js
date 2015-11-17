$(document).ready(function () {

    toastr.options = {
        "positionClass": "toast-top-center",
        "preventDuplicates": true
    }
	
	// team list show
	$('.frm_list02 > a').click(function(event) {
		event.preventDefault();
		$(this).siblings('ul').toggle();
	});	
	
    $('#fileupload').focus().trigger('click');

    //プロファイル画像をプレビューする
    $(function () {
        $('#fileupload').fileupload({
            dataType: 'json',
            url: '/mypage/setting/uploadprofileimg/',
            autoUpload: true,
            singleFileUploads: true,
            limitMultiFileUploadSize: 5120000000,
            done: function (e, data) {
                var FilePath = data.result.FilePath;
                $("#ProfileImg").attr("src", FilePath);
            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                //$('#progress .bar').css(
                //    'width',
                //    progress + '%'
                //);
            },
            fail: function (e, data) {
                alert(data.errorThrown + ' ' + data.textStatus);
            }
        });
    });

    //Click 設定を保存する
    $("#save_settings").click(function (event) {
        save_setting();
    });

    //Click 設定を保存する
    $("#save_mailsetting").click(function (event) {
        save_mailsetting();
    });

    var f_like_team = function (SportsID, TeamID, TeamName) {
        this.SportsID = SportsID;
        this.TeamID = TeamID;
        this.TeamName = TeamName;
    }

    function save_setting() {

        var nickname = document.getElementById("nickname").value;
        var profileImg = $("#ProfileImg").attr("src");

        var gender = document.getElementsByName("gender")[0].value;

        var birthday_year = document.getElementsByName("year")[0].value;
        var birthday_month = document.getElementsByName("month")[0].value;
        var birthday_day = document.getElementsByName("day")[0].value;

        var prefecture = document.getElementsByName("出身地")[0].value;

        var like_sports = [];
        var sports_id;
        var checkboxt_likesports = document.getElementsByName("好きなスポーツ");
        for (var i = 0; i < checkboxt_likesports.length; i++) {
            if (checkboxt_likesports[i].checked) {
                sports_id = checkboxt_likesports[i].id;
                like_sports.push(sports_id);
            }
        }

        var like_teams = [];
        var team_id;
        var team_name;
        var sports_team;
        var checkboxt_liketeams = document.getElementsByName("好きなチーム");
        for (var j = 0; j < checkboxt_liketeams.length; j++) {
            if (checkboxt_liketeams[j].checked) {
                sport_team = checkboxt_liketeams[j].id.split("-")
                sports_id = sport_team[0]
                team_id = sport_team[1];
                team_name = checkboxt_liketeams[j].value;
                var like_team = new f_like_team(sports_id, team_id, team_name);
                like_teams.push(like_team);
            }
        }
        var post_data = {
            MemberID:0,
            Nickname: nickname,
            ProfileImg: profileImg,
            Image:"",
            ExpectNumber: 0,
            FollowingNumber: 0,
            FollowerNumber:0,
            Gender: gender,
            GenderCheckedMale: "",
            GenderCheckedFeMale:"",
            BirthdayYear: birthday_year,
            BirthdayMonth: birthday_month,
            BirthdayDay: birthday_day,
            PrefecturesID: prefecture,
            LikeSportsID: like_sports,
            LikeTeam: like_teams
        };

        var url = '/mypage/Setting/SaveSetting/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                if (result.HasError) {
                    toastr.error("プロフィール設定を保存できませんでした。" + result.Message);
                }
                else {
                    toastr.success('プロフィール設定を保存しました。');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                toastr.error("プロフィール設定を保存できませんでした。");
            }

        });
    }

    function save_mailsetting() {

        var mail_game_start = 0;
        var mail_game_end = 0;
        var checkboxt_mail = document.getElementsByName("予想した試合");
        for (var i = 0; i < checkboxt_mail.length; i++) {
            if (checkboxt_mail[i].checked) {
                if (checkboxt_mail[i].id == "name01_14") {
                    mail_game_start = 1;
                }
                if (checkboxt_mail[i].id == "name01_15") {
                    mail_game_end = 2;
                }
            }
        }
        var post_data = {
            MailSendAtStart: mail_game_start,
            MailSendAtEnd: mail_game_end
        };

        var url = '/mypage/Setting/SaveMailSetting/';

        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(post_data),
            async: false,
            success: function (result) {

                if (result.HasError) {
                    toastr.error("メール設定を保存できませんでした。" + result.Message);
                }
                else {
                    toastr.success('メール設定を保存しました。');
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                toastr.error("メール設定を保存できませんでした。");
            }
        });
    }
});
