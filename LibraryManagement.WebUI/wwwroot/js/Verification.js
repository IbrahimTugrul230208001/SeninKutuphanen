$document.ready(function () {
    $("#verify").click(function () {
        var code = $("#coode").val();
        $.ajax({
            type: "POST",
            url: "Verification/Verify",
            data: JSON.stringify(formData),
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