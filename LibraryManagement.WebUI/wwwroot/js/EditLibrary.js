﻿$(document).ready(function () {
    $("#UpdateButton").click(function (event) {
        event.preventDefault();

        var formData = {
            Id: $("#UpdateId").val(),
            Name: $("#UpdateName").val(),
            Author: $("#UpdateAuthor").val(),
            Category: $("#UpdateCategory").val(),
            CompletedPages: $("#UpdateCompletedPages").val(),
            TotalOfPages: $("#UpdateTotalPages").val(),
            Status: $("#UpdateStatus").val()
        };

        $.ajax({
            type: "PUT",
            url: "/Kullanici/Update",
            data: JSON.stringify(formData),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Kitap bilgileri başarıyla güncellendi!");
                    var row = document.querySelector(`#tableBody tr[data-id="${formData.Id}"]`);
                    if (row) {
                        var cells = row.querySelectorAll("td");
                        cells[1].textContent = formData.Name;
                        cells[2].textContent = formData.Author;
                        cells[3].textContent = formData.Category;
                        cells[4].textContent = formData.CompletedPages;
                        cells[5].textContent = formData.TotalOfPages;
                        cells[6].textContent = formData.Status;
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
            Name: $("#AddName").val(),
            Author: $("#AddAuthor").val(),
            Category: $("#AddCategory").val(),
            CompletedPages: $("#AddCompletedPages").val(),
            TotalOfPages: $("#AddTotalPages").val(),
            Status: $("#AddStatus").val()
        };

        if (!formData.Name || !formData.Author || !formData.Category || !formData.CompletedPages || !formData.TotalOfPages || !formData.Status) {
            alert("Lütfen tüm alanları doldurun!");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/Kullanici/Add",
            data: JSON.stringify(formData),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Yeni kitap kütüphanenize eklendi!");

                    var newRow = `<tr data-id="${response.id}" class="table-row cursor-pointer">
                        <td class="px-4 py-2 border border-gray-300">${response.id}</td>
                        <td class="px-4 py-2 border border-gray-300">${response.Name}</td>
                        <td class="px-4 py-2 border border-gray-300">${response.Author}</td>
                        <td class="px-4 py-2 border border-gray-300">${response.Category}</td>
                        <td class="px-4 py-2 border border-gray-300">${response.CompletedPages}</td>
                        <td class="px-4 py-2 border border-gray-300">${response.TotalOfPages}</td>
                        <td class="px-4 py-2 border border-gray-300">${response.Status}</td>
                    </tr>`;

                    $("#tableBody").append(newRow); // Add row without refresh

                    // Clear input fields
                    $("#AddName, #AddAuthor, #AddCategory, #AddCompletedPages, #AddTotalPages, #AddStatus").val('');
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
    $("#DeleteBook").click(function () {
        var bookId = $("#UpdateId").val(); // Ensure input exists for book ID

        $.ajax({
            type: "DELETE",
            url: "/Kullanici/Delete",
            data: JSON.stringify(bookId),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Kitap kütüphaneden kaldırıldı.");
                    $("#tableBody tr[data-id='" + bookId + "']").remove();
                } else {
                    alert("Kaldırma işlemi esnasında bir hata oluştu: " + response.error);
                }
            },
            error: function (xhr, status, error) {
                console.log("Error details:", xhr, status, error);
                alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
            }
        });
    });

    $("#AddToFavoritesButton").click(function () {
        var bookID = $("#UpdateId").val(); // Ensure input exists for book ID

        $.ajax({
            type: "POST",
            url: "/Kullanici/AddToShowcase",
            data: JSON.stringify(bookID),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    var showcaseContainer = $("#showcaseContainer");
                    showcaseContainer.append(`
                        <div class="bg-gray-200 p-4 rounded-lg text-center fav">
                            <span class="block text-sm font-semibold">${response.bookName}</span>
                            <div class="w-full h-32 bg-gray-300 rounded mt-2"></div>
                            <form action="RemoveBookShowcase" method="post">
                                <input type="hidden" name="booktitle" value="${response.bookName}" />
                                <button type="submit" class="w-full bg-red-500 text-white p-2 rounded mt-2">Kaldır</button>
                            </form>
                        </div>
                    `);
                    alert("Kitap kullanıcı vitrinine eklendi!");
                } else {
                    alert("İşlem esnasında bir hata oluştu.");
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
    $(".favbutton").click(function (event) {
        event.preventDefault();
        var titleId = $(this).closest('form').find("input[id^='bookTitleId']").val();
        var bookName = $("#bookTitle"+titleId).text();
            $.ajax({
                type: "DELETE",
                url: "/Kullanici/RemoveBookShowcase",
                data: JSON.stringify(bookName),
                dataType: "json",
                contentType: "application/json",
                success: function (response) {
                    if (response.success) {

                        $(event.target).closest('.fav').remove(); 

                        var emptyPlaceholder = `
                        <div class="fav">
                            <div class="booktitle-container"><label class="book-title" name="booktitle">-</label></div>
                            <form class="AddToFavorites" action="AddToShowcase" method="post">
                                <input type="hidden" name="booktitle" />
                                <div class="imgBox">
                                    <input type="file" id="imageInput" name="imageFile" accept="image/*">
                                    <label class="upload-label" for="imageInput">Resim Yüklemek İçin Tıkla</label>
                                </div>
                                <button type="submit" class="upload-button">Yükle</button>
                            </form>
                        </div>`;
                        $("#showcaseContainer").append(emptyPlaceholder);
                        alert("Kitap kullanıcı vitrininden kaldırıldı!");

                    } else {
                        alert("İşlem esnasında bir hata oluştu.");
                    }
                },
                error: function (xhr, status, error) {
                    console.log("Error details:", xhr, status, error);
                    alert("Sunucu ile iletişim kurulurken bir hata oluştu.");
                }
            });
    });
});




const circularProgressBar = document.querySelector('.circular-progress');
const rows = document.querySelectorAll(".Table1Rows tr");

rows.forEach(row => {
    row.addEventListener('click', function () {
        // Remove the "selected" class from all rows
        rows.forEach(r => r.classList.remove("selected"));
        // Add the "selected" class to the clicked row
        this.classList.add("selected");

        // Get data from the selected row
        const cells = this.querySelectorAll("td");
        const selectedCompletedPages = parseInt(cells[4].textContent);
        const selectedTotalOfPages = parseInt(cells[5].textContent);
        const completionRate = (selectedCompletedPages / selectedTotalOfPages) * 100;
        const parsedPercentage = parseFloat(completionRate);


        if (!isNaN(parsedPercentage) && parsedPercentage >= 0 && parsedPercentage <= 100) {
            const degreeValue = (parsedPercentage / 100) * 360;
            const formattedDegreeValue = parsedPercentage.toFixed(2); // Convert to a string with two decimal places
            circularProgressBar.style.background = `conic-gradient(#8b4513 ${degreeValue}deg, #ccc 0deg)`;
            document.getElementById("progressValue").textContent = `${formattedDegreeValue}%`;
        } else {
            console.error('Invalid input or prompt canceled.');
        }

    });
});

// Get reference to the table body
var tbody = document.getElementById('tableBody');

// Function to filter table rows based on user input
function filterTable() {
    var inputName = document.getElementById('txtSearchByName').value.toLowerCase();
    var inputAuthor = document.getElementById('txtSearchByAuthor').value.toLowerCase();
    var inputCategory = document.getElementById('txtSearchByCategory').value.toLowerCase();

    var rows = tbody.getElementsByTagName('tr');

    for (var i = 0; i < rows.length; i++) {
        var row = rows[i];
        var name = row.cells[1].textContent.toLowerCase();
        var author = row.cells[2].textContent.toLowerCase();
        var category = row.cells[3].textContent.toLowerCase();

        if (name.indexOf(inputName) > -1 && author.indexOf(inputAuthor) > -1 && category.indexOf(inputCategory) > -1) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    }
}

document.getElementById('txtSearchByName').addEventListener('input', filterTable);
document.getElementById('txtSearchByAuthor').addEventListener('input', filterTable);
document.getElementById('txtSearchByCategory').addEventListener('input', filterTable);

const menuBtn = document.getElementById('menu-btn');
const mobileMenu = document.getElementById('mobile-menu');

menuBtn.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
});

document.addEventListener("DOMContentLoaded", function () {
    var tableBody = document.getElementById("tableBody");

    tableBody.addEventListener("click", function (event) {
        var row = event.target.closest("tr");
        if (!row) return; // Ensure a row was clicked

        // Deselect all rows
        document.querySelectorAll("#tableBody tr").forEach(r => r.classList.remove("bg-gray-400"));

        // Select the clicked row
        row.classList.add("bg-gray-400"); 

        // Get data from clicked row
        var cells = row.getElementsByTagName("td");
        if (cells.length < 7) return; 

        let ID = cells[0].textContent.trim();
        let name = cells[1].textContent.trim();
        let author = cells[2].textContent.trim();
        let category = cells[3].textContent.trim();
        let completedPages = cells[4].textContent.trim();
        let totalPages = cells[5].textContent.trim();
        let status = cells[6].textContent.trim();

        // Populate input fields
        document.getElementById("UpdateId").value = ID;
        document.getElementById("UpdateName").value = name;
        document.getElementById("UpdateAuthor").value = author;
        document.getElementById("UpdateCategory").value = category;
        document.getElementById("UpdateCompletedPages").value = completedPages;
        document.getElementById("UpdateTotalPages").value = totalPages;
        document.getElementById("UpdateStatus").value = status;
        document.getElementById("IdTextBoxDel").value = ID;
        document.getElementById("IdTextBoxFav").value = ID;
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var tableBody = document.getElementById("tableBody");
    var circle = document.getElementById("progress-circle");
    var progressValue = document.getElementById("progressValue");
    var circumference = 251.2; // Circle circumference (2 * π * r)

    tableBody.addEventListener("click", function (event) {
        var row = event.target.closest("tr");
        if (!row) return;

        document.querySelectorAll("#tableBody tr").forEach(r => r.classList.remove("bg-gray-400"));
        row.classList.add("bg-gray-400");

        var cells = row.getElementsByTagName("td");
        if (cells.length < 7) return;

        let completedPages = parseInt(cells[4].textContent.trim()) || 0;
        let totalPages = parseInt(cells[5].textContent.trim()) || 1;
        let percentage = Math.round((completedPages / totalPages) * 100);
        percentage = Math.max(0, Math.min(100, percentage));

        let targetOffset = circumference - (percentage / 100) * circumference;

        // Animate progress bar
        let currentOffset = parseFloat(circle.style.strokeDashoffset) || circumference;
        let step = (currentOffset - targetOffset) / 30; // Adjust for smoothness
        let frame = 0;

        function animateProgress() {
            if (frame < 30) {
                circle.style.strokeDashoffset = currentOffset - step * frame;
                frame++;
                requestAnimationFrame(animateProgress);
            } else {
                circle.style.strokeDashoffset = targetOffset;
            }
        }

        animateProgress();
        progressValue.textContent = percentage + "%";
    });
});