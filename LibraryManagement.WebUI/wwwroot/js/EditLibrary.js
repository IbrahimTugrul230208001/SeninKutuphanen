document.addEventListener("DOMContentLoaded", function() {
  // Get all cells in the table
  var cells = document.querySelectorAll(".Table1Rows td");

  // Add click event listener to each cell
  cells.forEach(function(cell) {
      cell.addEventListener("click", function(event) {
          // Deselect all other rows
          var rows = document.querySelectorAll(".Table1Rows tr");
          rows.forEach(function(row) {
              row.classList.remove("selected");
          });

          // Select the parent row of the clicked cell
          var row = cell.parentNode;
          row.classList.add("selected");
      });
  });
});

document.addEventListener("DOMContentLoaded", function() {
  // Get all rows in the table body
  var rows = document.querySelectorAll(".Table1Rows tr");

  // Add click event listener to each row
  rows.forEach(function(row) {
      row.addEventListener("click", function(event) {
          // Deselect all other rows
          rows.forEach(function(otherRow) {
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
    row.addEventListener('click', function() {
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


$(document).ready(function () {
    $("#UpdateButton").click(function (event) {
        event.preventDefault(); // Prevent default form submission

        // Gather form data
        var formData = {
            UserName: $("#userName").val(),
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
            url: "/Update", 
            data: JSON.stringify(formData),   
            dataType: "json", 
            contentType: "application/json",
            success: function (response) {
                if (response.success) {
                    alert("Kitap bilgileri başarıyla güncellendi!");
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

