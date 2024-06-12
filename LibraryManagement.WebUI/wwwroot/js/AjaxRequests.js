$(document).ready(function () {
    $("#UpdateButton").click(function (event) {
        event.preventDefault(); // Prevent default form submission

        // Gather form data
        var formData = {
            Id: $("#UpdatetextBox0").val(),
            Name: $("#UpdatetextBox1").val(),
            Author: $("#UpdatetextBox2").val(),
            Category: $("#UpdatetextBox3").val(),
            CompletedPages: $("#UpdatetextBox4").val(),
            TotalOfPages: $("#UpdatetextBox5").val(),
            Status: $("#UpdatetextBox6").val(),
        };

        $.ajax({
            type: "PUT",
            url: "/EditLibrary/Update",
            data: JSON.stringify(formData),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Kitap bilgileri başarıyla güncellendi!");

                    // Find the selected row
                    var selectedRow = document.querySelector(".Table1Rows tr.selected");

                    if (selectedRow) {
                        // Update the cells of the selected row
                        var cells = selectedRow.querySelectorAll("td");
                        cells[0].textContent = formData.Id;
                        cells[1].textContent = formData.Name;
                        cells[2].textContent = formData.Author;
                        cells[3].textContent = formData.Category;
                        cells[4].textContent = formData.CompletedPages;
                        cells[5].textContent = formData.TotalOfPages;
                        cells[6].textContent = formData.Status;

                        // Optionally, you can also update the circular progress bar
                        const completionRate = (formData.CompletedPages / formData.TotalOfPages) * 100;
                        const parsedPercentage = parseFloat(completionRate);
                        const degreeValue = (parsedPercentage / 100) * 360;
                        const formattedDegreeValue = parsedPercentage.toFixed(2); // Convert to a string with two decimal places

                        if (!isNaN(parsedPercentage) && parsedPercentage >= 0 && parsedPercentage <= 100) {
                            circularProgressBar.style.background = `conic-gradient(#8b4513 ${degreeValue}deg, #ccc 0deg)`;
                            document.getElementById("progressValue").textContent = `${formattedDegreeValue}%`;
                        } else {
                            console.error('Invalid input or prompt canceled.');
                        }
                    }
                } else {
                    alert("Güncelleme sırasında bir hata oluştu: " + response.error);
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
    $("#AddButton").click(function (event) {
        event.preventDefault();
        var formData = {
            Name: $("#AddtextBox1").val(),
            Author: $("#AddtextBox2").val(),
            Category: $("#AddtextBox3").val(),
            TotalOfPages: $("#AddtextBox4").val(),
            CompletedPages: $("#AddtextBox5").val(),
            Status: $("#AddtextBox6").val(),
        };

        $.ajax({
            type: "POST",
            url: "/EditLibrary/Add",
            data: JSON.stringify(formData),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    var tableBody = document.getElementById("tableBody");
                    var newRow = document.createElement("tr");

                    newRow.innerHTML = `
                        <td>${response.newBookId}</td>
                        <td>${formData.Name}</td>
                        <td>${formData.Author}</td>
                        <td>${formData.Category}</td>
                        <td>${formData.CompletedPages}</td>
                        <td>${formData.TotalOfPages}</td>
                        <td>${formData.Status}</td>
                    `;
                    tableBody.appendChild(newRow);
                    $("#AddtextBox1").val('');
                    $("#AddtextBox2").val('');
                    $("#AddtextBox3").val('');
                    $("#AddtextBox4").val('');
                    $("#AddtextBox5").val('');
                    $("#AddtextBox6").val('');
                } else {
                    alert("Ekleme sırasında bir hata oluştu: " + response.error);
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
    $(".deletebutton").click(function (event) {
        event.preventDefault();
        var bookId = {
            Id: $("#IdTextBoxDel").val(),
        };

        $.ajax({
            type: "DELETE",
            url: "/EditLibrary/Delete",
            data: JSON.stringify(bookId),
            dataType: "json",
            contentType: "application/json",
            successs: function (response) {
                if (response.success) {
                    alert("Kitap kütüphaneden kaldırıldı.");
                    $("#tableBody tr").filter(function () {
                        return $(this).find("td:first").text() == bookId;
                    }).remove();
                }
                else {
                    alert("Kaldırma işlemi esnasında bir hata oluştu: " + response.error);
                }
            }
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            });
    });
});