$(document).ready(function () {
    // Change User Name

        $("#changeUserName").click(function () {
            console.log("Button clicked");  // Check if this line prints in the console
            var userName = $("#TbxNewUserName").val();
            console.log("New User Name: " + userName);  // Check if the value is being retrieved

            $.ajax({
                type: "PUT",
                url: "/Settings/SetNewUserName",
                data: JSON.stringify(userName),
                dataType: "json",
                contentType: "application/json",
                success: function (response) {
                    if (response.success) {
                        $("#userName").text(userName);
                        alert("Kullanıcı adı değiştirildi!");
                    } else {
                        alert("Kullanıcı adı değiştirilme esnasında bir hata oluştu.");
                    }
                },
                error: function (xhr, status, error) {
                    console.log("Error details:", xhr, status, error);
                    alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
                }
            });
        });


    // Change Location
    $("#changeLocation").click(function () {
        var user = {
            City: $("#TbxSetNewCity").val(),
            Country: $("#TbxSetNewCountry").val()
        };
        console.log("şehir: "+$("#TbxSetNewCity").val())
        console.log("ülke: "+$("#TbxSetNewCountry").val())
        $.ajax({
            type: "PUT",
            url: "/Settings/SetNewResidementPlaces",
            data: JSON.stringify(user),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("İkamet yerleri güncellendi.");
                } else {
                    alert("Güncelleme esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });

    // Remove Profile Picture
    $("#removeProfilePicButton").click(function () {
        var userName = $("#userName").text();
        $.ajax({
            type: "PUT",
            url: "/Settings/RemoveProfilePicture",
            data: JSON.stringify(userName),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Profil fotoğrafı kaldırıldı.");
                } else {
                    alert("Kaldırma esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });

    // Change Password
    $("#changePassword").click(function () {
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
                } else {
                    alert("Değiştirme esnasında bir hata oluştu.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile irtibat kurulurken bir hata oluştu.");
            }
        });
    });
});
const menuBtn = document.getElementById('menu-btn');
const mobileMenu = document.getElementById('mobile-menu');

menuBtn.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
});