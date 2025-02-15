$(document).ready(function () {
    $("#login").click(function (event) {
        event.preventDefault();

        var user = {
            Email: $("#email").val(),
            Password: $("#password").val()
        };

        $.ajax({
            type: "POST",
            url: "/LogIn/UserLogIn",
            data: JSON.stringify(user),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Giriş Yapılıyor.");
                    window.location.href = response.redirectUrl;
                } else {
                    alert("Giriş yapılamadı.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            }
        });
    });
});