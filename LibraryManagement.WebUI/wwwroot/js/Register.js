$(document).ready(function () {
    $("#registerButton").click(function (event) {
        event.preventDefault();
        var user = {
            UserName: $("#email").val(),
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
                    alert("Kullanıcı kayıt edildi!");
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
