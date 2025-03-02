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
            Status: $("#UpdatetextBox6").val()
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
            CompletedPages: $("#AddtextBox4").val(),
            TotalOfPages: $("#AddtextBox5").val(),
            Status: $("#AddtextBox6").val()
        };

        $.ajax({
            type: "POST",
            url: "/EditLibrary/Add",
            data: JSON.stringify(formData),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Yeni kitap kütüphanenize eklendi!");
                    var tableBody = document.getElementById("tableBody");
                    var newRow = document.createElement("tr");

                    newRow.innerHTML = `
                        <td style="visibility:hidden;">${response.newBookId}</td>
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
    $("#DeleteBook").click(function (event) {
        event.preventDefault();
        var bookId = $("#IdTextBoxDel").val();
        

        $.ajax({
            type: "DELETE",
            url: "/EditLibrary/Delete",
            data: JSON.stringify(bookId),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Kitap kütüphaneden kaldırıldı.");
                    $("#tableBody tr").filter(function () {
                        return $(this).find("td:first").text() == bookId;
                    }).remove();
                }
                else {
                    alert("Kaldırma işlemi esnasında bir hata oluştu: " + response.error);
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
    $(".AddToFavoritesButton").click(function (event) {
        event.preventDefault();
        var bookID = $("#IdTextBoxFav").val();

        $.ajax({
            type: "POST",
            url: "/EditLibrary/AddToShowcase",
            data: JSON.stringify(bookID),
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    // Construct new book HTML
                    var emptyShowcase = $("#showcaseContainer .fav:has(label[name='booktitle']:contains('-'))").first();

                    if (emptyShowcase.length > 0) { // Check if an empty box was found
                        // Update the empty showcase with the new book details
                        emptyShowcase.find("label[name='booktitle']").text(response.bookName);
                        emptyShowcase.find("input[name='booktitle']").val(response.bookName);
                        // Remove any default placeholder elements
                        emptyShowcase.find(".upload-label").remove();
                        alert("Kitap kullanıcı vitrinine eklendi!");
                    }
                    else {
                        alert("İşlem esnasında bir hata oluştu.");
                    }
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
                url: "/EditLibrary/RemoveBookShowcase",
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

document.addEventListener("DOMContentLoaded", function () {
    // Get all cells in the table
    var cells = document.querySelectorAll(".Table1Rows td");

    // Add click event listener to each cell
    cells.forEach(function (cell) {
        cell.addEventListener("click", function (event) {
            // Deselect all other rows
            var rows = document.querySelectorAll(".Table1Rows tr");
            rows.forEach(function (row) {
                row.classList.remove("selected");
            });

            // Select the parent row of the clicked cell
            var row = cell.parentNode;
            row.classList.add("selected");
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    // Get all rows in the table body
    var rows = document.querySelectorAll(".Table1Rows tr");

    // Add click event listener to each row
    rows.forEach(function (row) {
        row.addEventListener("click", function (event) {
            // Deselect all other rows
            rows.forEach(function (otherRow) {
                otherRow.classList.remove("selected");
            });

            row.classList.add("selected");

            var cells = row.querySelectorAll("td");
            let ID = cells[0].textContent;
            let name = cells[1].textContent;
            let author = cells[2].textContent;
            let category = cells[3].textContent;
            let completedPages = cells[4].textContent;
            let totalPages = cells[5].textContent;
            let status = cells[6].textContent;

            document.getElementById("UpdatetextBox0").value = ID;
            document.getElementById("UpdatetextBox1").value = name;
            document.getElementById("UpdatetextBox2").value = author;
            document.getElementById("UpdatetextBox3").value = category;
            document.getElementById("UpdatetextBox4").value = completedPages;
            document.getElementById("UpdatetextBox5").value = totalPages;
            document.getElementById("UpdatetextBox6").value = status;
            document.getElementById("IdTextBoxDel").value = ID;
            document.getElementById("IdTextBoxFav").value = ID;
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