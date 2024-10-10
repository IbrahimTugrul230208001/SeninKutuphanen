document.addEventListener("DOMContentLoaded", function () {
    var historyCount = parseInt(document.getElementById("historyBookCount").innerText);
    var literatureCount = parseInt(document.getElementById("literatureBookCount").innerText);
    var scienceCount = parseInt(document.getElementById("scienceBookCount").innerText);
    var religionCount = parseInt(document.getElementById("philosophyReligionBookCount").innerText);
    var psychologyCount = parseInt(document.getElementById("psychologySelfImprovementBookCount").innerText);

    var totalCount = historyCount + literatureCount + scienceCount + religionCount + psychologyCount;

    var historyPercent = (historyCount / totalCount) * 100;
    var literaturePercent = (literatureCount / totalCount) * 100;
    var sciencePercent = (scienceCount / totalCount) * 100;
    var religionPercent = (religionCount / totalCount) * 100;
    var psychologyPercent = (psychologyCount / totalCount) * 100;

    var libraryGenreData = document.querySelector(".LibraryGenreData");
    libraryGenreData.style.background = "conic-gradient(" +
        "darkred 0, darkred " + historyPercent + "%," +
        "purple " + historyPercent + "%, purple " + (historyPercent + literaturePercent) + "%," +
        "blue " + (historyPercent + literaturePercent) + "%, blue " + (historyPercent + literaturePercent + sciencePercent) + "%," +
        "green " + (historyPercent + literaturePercent + sciencePercent) + "%, green " + (historyPercent + literaturePercent + sciencePercent + religionPercent) + "%," +
        "orange " + (historyPercent + literaturePercent + sciencePercent + religionPercent) + "%, orange " + (historyPercent + literaturePercent + sciencePercent + religionPercent + psychologyPercent) + "%)";
});

document.addEventListener("DOMContentLoaded", function () {
    // Get the default color from the combobox
    var defaultColor = document.getElementById("colorSelect").value;

    // Set the color of the circular progress bar
    var progressBar = document.getElementById("progressBar");
    progressBar.style.borderColor = defaultColor;


    // Update the progress bar color when the combobox value changes
    document.getElementById("colorSelect").addEventListener("change", function () {
        var selectedColor = this.value;
        progressBar.style.borderColor = selectedColor;
        // You can also submit the form here if needed
        // document.getElementById("colorForm").submit();
    });
});