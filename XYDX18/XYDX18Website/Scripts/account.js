$(function () {
    $('#doc-prompt-toggle').on('click', function () {
        $('#my-prompt').modal({
            relatedTarget: this,
            onConfirm: function () {
                var title_i = $("#bt").val();
                var xcontent = $("#doc-ta-1").val();
                if (title_i == "") {
                    alert("请输入账号！");
                    return false;
                }
                if (xcontent == "") {
                    alert("请输入密码！");
                    return false;
                }

                $.ajax({
                    type: 'POST',
                    url: "AccountAdd",
                    dataType: "json",
                    data: { title: title_i, contents: xcontent },
                    success: function (result) {
                        if (result == "true") {
                            alert("添加成功！");
                            window.location.href = "/SuperAdmin/AccountList"
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
                if (title_i == "") {
                    alert("请输入账号！");
                    return false;
                }
                if (xcontent == "") {
                    alert("请输入密码！");
                    return false;
                }

                $.ajax({
                    type: 'POST',
                    url: "AccountEdit",
                    dataType: "json",
                    data: { Id: id, title: title_i, contents: xcontent },
                    success: function (result) {
                        if (result == "true") {
                            alert("修改成功！");
                            window.location.href = "/SuperAdmin/AccountList"
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
            window.location.href = "/SuperAdmin/AccountDelete?Id=" + $(this).attr("value");
        }
    });

    //退出
    $('.btnQ').on('click', function () {
        window.location.href = "/Login/SingOut";
    });

});


