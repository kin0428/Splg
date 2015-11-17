var articlecount;

$(document).ready(function () {

    getMoreArticles(0)


    //投稿のもっとみる検索
    $("#GetMoreArticlesButton").click(function (event) {

        articlecount = $(this).data("articlecount");

        getMoreArticles(articlecount);

    });



});

function getMoreArticles(articlecount) {


    var url = '/mypage/article/' + articlecount + '/';

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ articlecount: articlecount }),
        async: false,
        success: function (result) {

            var contributions = result.Contributions;
            var topics = result.TopicTitles;
            var totalCount = result.TotalCount;

            var lines = $("#lines");

            var oldDate = $("#GetMoreArticlesButton").data("lastheaderdate");

            for (var i = 0; i < contributions.length; i++) {

                var c = contributions[i];

                //日付タイトル
                var newDate = null;
                newDate = c.FormattedContributeDate;

                if (newDate != oldDate) {
                    lines.append('<h4>' + newDate + '</h4>');
                    $('#GetMoreArticlesButton').data("lastheaderdate", newDate);
                }

                var formattedModifiedDateTime = "";
                if (c.ModifiedDate != null)
                    formattedModifiedDateTime = c.FormattedModifiedDateTime + " 更新";


                //記事行
                var line = '<li>' +
                            '   <h4><a href="/user_article/0/' + c.ContributeId + '">' + c.Title + '</a></h4>' +
                            '   <p>';
                if (c.TopicTitle != "" && c.TopicTitle != undefined)
                    line += c.TopicTitle + '<br />';
                line += '<span>' + c.FormattedContributeDate + ' 投稿</span></p>' +
                            '</li>';

                lines.append(line);

                oldDate = newDate;

            }

            var newarticlecount = contributions.length * 1 + articlecount;
            $('#GetMoreArticlesButton').data("articlecount", newarticlecount);

            if (totalCount <= newarticlecount)
                $('#GetMoreArticlesButton').hide();


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("エラーが起こりました。" + errorThrown);
        }
    });



}