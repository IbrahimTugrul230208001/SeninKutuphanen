const categories = document.querySelectorAll('.category');

function handleCategoryClick(event) {
    categories.forEach(category => category.classList.remove('selected-category'));
    
    event.target.classList.add('selected-category');
}

categories.forEach(category => category.addEventListener('click', handleCategoryClick));

document.addEventListener("DOMContentLoaded", function () {
    var readingCount = parseInt(document.getElementById("reading").innerText);
    var planningCount = parseInt(document.getElementById("planning").innerText);
    var completedCount = parseInt(document.getElementById("completed").innerText);
    var droppedCount = parseInt(document.getElementById("dropped").innerText);
    var onHoldCount = parseInt(document.getElementById("onHold").innerText);

    var totalCount = readingCount + planningCount + completedCount + droppedCount + onHoldCount;

    var readingPercent = (readingCount / totalCount) * 100;
    var planningPercent = (planningCount / totalCount) * 100;
    var completedPercent = (completedCount / totalCount) * 100;
    var droppedPercent = (droppedCount / totalCount) * 100;
    var onHoldPercent = (onHoldCount / totalCount) * 100;

    var libraryGenreData = document.querySelector(".LibraryGenreData");
    libraryGenreData.style.background = "conic-gradient(" +
        "blue 0, blue " + readingPercent + "%," +
        "blueviolet " + readingPercent + "%, blueviolet " + (readingPercent + planningPercent) + "%," +
        "green " + (readingPercent + planningPercent) + "%, green " + (readingPercent + planningPercent + completedPercent) + "%," +
        "deeppink " + (readingPercent + planningPercent + completedPercent) + "%, deeppink " + (readingPercent + planningPercent + completedPercent + droppedPercent) + "%," +
        "orange " + (readingPercent + planningPercent + completedPercent + droppedPercent) + "%, orange " + (readingPercent + planningPercent + completedPercent + droppedPercent + onHoldPercent) + "%)";
});


