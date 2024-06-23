$(document).ready(function () {
    $("#changeUserName").click(function (event) {
        event.preventDefault();
        var UserName = $("#TbxNewUserName").val();
        $.ajax({
            type: "PUT",
            url: "/Settings/SetNewUserName",
            data: JSON.stringify(UserName),
            dataType: "json",
            contentType: "application/json",

            success: function(response) {
                if (response.success) {
                    $("#userName").text() = UserName;
                    alert("Kullanıcı adı değiştirildi!");
                }
                else {
                    alert("kullanıcı adı değiştirilme esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });
});

$(document).ready(function () {
    $("#changeLocation").click(function (event) {
        event.preventDefault();
        var user = {
            city: $("#TbxNewCity").val(),
            country: $("#TbxNewCountry").val()
        };

        $.ajax({
            type: "PUT",
            url: "/Settings/SetNewResidementPlaces",
            data: JSON.stringify(user),
            dataType: "json",
            contentType: "application/json",

            success: function (response) {
                if (response.success) {
                    alert("İkamet yerleri güncellendi.");
                }
                else {
                    alert("Güncelleme esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error Details: ", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });
});