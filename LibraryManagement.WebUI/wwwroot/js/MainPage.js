$(document).ready(function () {
    $('.add-btn').on('click', function () {
        var $btn = $(this);
        if ($btn.data('state') === 'plus') {
            // Animate checkmark drawing
            $btn.removeClass('bg-blue-600 hover:bg-blue-700').addClass('bg-green-600 hover:bg-green-700');
            $btn.find('.plus-icon').addClass('hidden');
            var $checkIcon = $btn.find('.check-icon');
            $checkIcon.removeClass('hidden');
            var $svg = $checkIcon.find('svg');
            var $path = $svg.find('path');
            // Prepare for animation
            var length = $path[0].getTotalLength();
            $path.css({
                'stroke-dasharray': length,
                'stroke-dashoffset': -length, // Change offset to negative for left-to-right animation
                'transition': 'none'
            });
            // Force reflow for transition to take effect
            $path[0].getBoundingClientRect();
            // Animate
            $path.css({
                'transition': 'stroke-dashoffset 0.5s ease',
                'stroke-dashoffset': 0
            });
            $btn.data('state', 'check');
        }
    });
});

$('.add-btn').on('click', function () {
    const bookId = $(this).closest('div').data('id');
    if (!bookId) return;
    $.ajax({
        type: 'POST',
        url: '/Kullanici/Add',
        data: JSON.stringify({ Id: bookId, Status: 'Okunacak', CompletedPages: 0, TotalOfPages: 0 }),
        dataType: 'json',
        contentType: 'application/json'
    });
});


$('.page').on('click', function () {
    const pageId = $(this).val();
    if (!pageId) return;

    $('.page').on('click', function () {
        const pageId = $(this).val();
        if (!pageId) return;
        $.ajax({
            type: 'GET',
            url: `/Kullanici/Anasayfa/${pageId}`,
            dataType: 'json',
            contentType: 'application/json'
        });
    });
});

function checkEnter(event) {
    if (event.key === 'Enter') {
        sendPrompt();
    }
}

const menuBtn = document.getElementById('menu-btn');
const mobileMenu = document.getElementById('mobile-menu');
menuBtn.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
    mobileMenu.classList.toggle('scale-y-0');
});

function searchBooks() {
    var searchData = {
        SearchInput: document.getElementById('search-input').value,
        SearchCriteria: document.getElementById('search-criteria').value
    };

    fetch("/Kullanici/SearchBooks", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(searchData)
    })
        .then(response => response.json())
        .then(data => {
            if (!data.response) {
                console.error("Invalid API response:", data);
                return;
            }
            // Handle data.response here
        })
        .catch(error => console.error("Error:", error));
}
