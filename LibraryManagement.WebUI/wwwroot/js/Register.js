$(document).ready(function () {
    $("#registerButton").click(function (event) {
        event.preventDefault();
        var user = {
            UserName: $("#registerUserName").val(),
            NewPassword: $("#registerPassword").val(),
            NewPasswordAgain: $("#registerPasswordAgain").val()
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
                }
                else {
                    alert("Kayıt esnasında bir hata oluştu.");
                }
            },
             error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            }
        });
    });
});
   