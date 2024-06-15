$(document).ready(function () {
    $("#loginButton").click(function (event) {
        event.PreventDefault();
        var user = {
            UserName: $("#tbxUserName").val(),
            Password: $("#tbxPassword").val(),
        };
        $.ajax({
            type: "POST",
            url: "/LogIn/UserLogIn",
            data: JSON.stringify(user),
            dataType: "json",
            contentType: "application/json",

            success: function (response) {
                if (response.success) {
                    alert("Giriş yapılıyor...");
                }
                else {
                    alert("Şifre veya kullanıcı adı yanlış.");
                }
            },
            error: function(xhr, status, error) {
                    console.log("Error details:", xhr, status, error);
                    alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }          
        });
    });
});

$(document).ready(function () {
    $(".Register-button").click(function (event) {
        event.PreventDefault();

        var newUser = {
            UserName: $("#registerUserName"),
            Password: $("#registerPassword"),
        };
        $.ajax({
            type: "POST",
            url: "/Register/RegisterUser",
            data: JSON.stringify(newUser),
            contentType: "json",
            dataType: "application/json",

            success: function (response) {
                if (response.success) {
                    alert("Kullanıcı kaydı başarılı.");
                }
                else
                {
                    alert("Kullanıcı kaydı başarısız.");
                }
            },
            error: function (xhr,status,error) {
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
                console.log("Error Details: ",xhr, status, error);
            }
        });
    });
});