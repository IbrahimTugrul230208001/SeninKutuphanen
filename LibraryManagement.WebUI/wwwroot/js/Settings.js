$(document).ready(function () {
    $("#changeUserName").click(function (event) {
        event.preventDefault();
        var userName = $("#TbxNewUserName").val();
        $.ajax({
            type: "PUT",
            url: "/Settings/SetNewUserName",
            data: JSON.stringify(userName),
            dataType: "json",
            contentType: "application/json",

            success: function(response) {
                if (response.success) {
                    $("#userName").text() = userName;
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

$(document).ready(function () {
    $("#removeProfilePicButton").click(function (event) {
        event.preventDefault();

        var userName = $("#userName").val();
        $.ajax({
            type: "PUT",
            url: "/Settings/RemoveProfilePicture",
            data: JSON.stringify(userName),
            dataType: "json",
            contentType: "application/json",

            success: function (response) {
                if (response.success) {
                    alert("Profil fotoğrafı kaldırıldı.");
                }
                else {
                    alert("Kaldırma esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error Details: ", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });
});

$(document).ready(function () {
    $("#changePassword").click(function (event) {
        event.preventDefault();
        var user = {
            Password: $("#TbxCurrentPassword").val(),
            NewPassword: $("#TbxNewPassword").val(),
            NewPasswordAgain: $("#TbxNewPasswordAgain").val()
        };
        $.ajax({
            type: "PUT",
            url: "/Settings/SetNewPassword",
            data: JSON.stringify(user),
            dataType: "json",
            contentType: "application/json",

            success: function (response) {
                if (response.success) {
                    alert("Şifre değiştirildi.");
                }
                else {
                    alert("Değiştirme esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error Details: ", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });
});