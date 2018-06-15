$(function () {
    $('#doc-prompt-toggle').on('click', function () {
        $('#my-prompt').modal({
            relatedTarget: this,
            onConfirm: function () {
                var title_i = $("#bt").val();
                var xcontent = $("#doc-ta-1").val();
                if (xcontent == "") {
                    alert("请输入内容！");
                    return false;
                }

                $.ajax({
                    type: 'POST',
                    url: "/Admin/ContentsAdd",
                    dataType: "json",
                    data: { title: title_i, contents: xcontent },
                    success: function (result) {
                        if (result == "true") {
                            alert("添加成功！");
                            window.location.href = "/Admin/ContentsList"
                        }
                        else {
                            alert("添加失败！");
                        }
                    }
                });

            },
            onCancel: function () {

            }
        });
    });

    //修改
    $('.btnUpdate').on('click', function () {
        var id = $(this).attr("value");
        var oldTitle = $("#hdTitle_" + id).val();
        var oldCotent = $("#hdContent_" + id).val();

        $("#bt").val(oldTitle);
        $("#doc-ta-1").val(oldCotent);

        $('#my-prompt').modal({
            relatedTarget: this,
            onConfirm: function () {
                var title_i = $("#bt").val();
                var xcontent = $("#doc-ta-1").val();
                if (xcontent == "") {
                    alert("请输入内容！");
                    return false;
                }

                $.ajax({
                    type: 'POST',
                    url: "/Admin/ContentsEdit",
                    dataType: "json",
                    data: { Id: id, title: title_i, contents: xcontent },
                    success: function (result) {
                        if (result == "true") {
                            alert("修改成功！");
                            window.location.href = "/Admin/ContentsList"
                        }
                        else {
                            alert("修改失败！");
                        }
                    }
                });

            },
            onCancel: function () {

            }
        });
    });

    //删除
    $('.btnDelete').on('click', function () {
        if (confirm("确定删除吗？")) {
            window.location.href = "/Admin/ContentsDelete?Id=" + $(this).attr("value");
        }
    });
    //点击生成二维码
    $('.btnQRCode').on('click', function () {
        window.location.href = "/Contents/ReViewQRCode?Id=" + $(this).attr("value");
    });

    //退出
    $('.btnQ').on('click', function () {
        window.location.href = "/Login/SingOut";
    });


});


