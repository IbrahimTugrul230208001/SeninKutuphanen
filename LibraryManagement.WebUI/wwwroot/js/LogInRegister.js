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