$document.ready(function () {
    $("#verify").click(function () {
        var code = $("#code").val();
        $.ajax({
            type: "POST",
            url: "Verification/VerifyEmail",
            data: JSON.stringify(code),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Doğrulama başarılı");
                }
                else {
                    alert("Doğrulama başarısız, kodu tekrar giriniz.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            }
        });
    });
});


$(document).ready(function () {
    $("#resend").click(function () {
        $.ajax({
            type: "POST",
            url: "Verification/ResendEmail",
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Doğrulama e-postası yeniden gönderildi.");
                }
                else {
                    alert("E-posta gönderimi başarısız, lütfen tekrar deneyiniz.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            }
        });
    });
});
