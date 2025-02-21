$(document).ready(function () {
    $("#register").click(function (event) {
        event.preventDefault();
        var user = {
            Email: $("#email").val(),
            UserName: $("#username").val(),
            NewPassword: $("#password").val(),
            NewPasswordAgain: $("#password-again").val()
        };
        $.ajax({
            type: "POST",
            url: "/Register/RegisterUser",
            data: JSON.stringify(user),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    window.location.href = response.redirectUrl;
                } else {
                    alert("Kayıt esnasında bir hata oluştu: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            }
        });
    });
});
