
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
        searchBooks();
    }
}

const menuBtn = document.getElementById('menu-btn');
const mobileMenu = document.getElementById('mobile-menu');
menuBtn.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
    mobileMenu.classList.toggle('scale-y-0');
});

function searchBooks(page = 1) {
    const input = $('#search-input').val().trim();
    const crit = $('#search-criteria').val();
    const $list = $('#bookList');

    // 1 . block short queries (<3 chars)
    if (input.length < 3) {
        $list.html(`
            <div class="text-center text-gray-400 mt-4">
                3 karakter veya daha fazla karakter girerek arama yapınız.
            </div>`);
        history.replaceState(null, '', `/Kullanici/AnaSayfa/${page}`);
        return;
    }

    // 2 . valid query → fetch results partial
    const params = new URLSearchParams({ searchInput: input, searchCriteria: crit });
    const url = `/Kullanici/AnaSayfa/${page}?${params}`;
    history.replaceState(null, '', url);

    fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
        .then(r => r.text())
        .then(html => { $list.html(html); });
}


$('#bookList').on('click', '.add-btn', function () {
    var $btn = $(this);
    if ($btn.data('state') === 'plus') {
        // Animate checkmark drawing
        $btn.removeClass('bg-gray-200 hover:bg-gray-300').addClass('bg-green-600 hover:bg-green-700');
        $btn.find('.plus-icon').addClass('hidden');
        var $checkIcon = $btn.find('.check-icon');
        $checkIcon.removeClass('hidden');
        var $svg = $checkIcon.find('svg');
        var $path = $svg.find('path');
        if ($path.length) {
            // Prepare for animation
            var length = $path[0].getTotalLength();
            $path.css({
                'stroke-dasharray': length,
                'stroke-dashoffset': -length, // for left-to-right
                'transition': 'none'
            });
            $path[0].getBoundingClientRect(); // force reflow
            $path.css({
                'transition': 'stroke-dashoffset 0.5s ease',
                'stroke-dashoffset': 0
            });
        }
        $btn.prop('disabled', true);
        $btn.data('state', 'check');

        // Send AJAX POST to update the database
        const bookId = $btn.closest('div[data-id]').data('id');
        if (bookId) {
            $.ajax({
                type: 'POST',
                url: '/Kullanici/Add',
                data: JSON.stringify({ Id: bookId, Status: 'Okunacak', CompletedPages: 0, TotalOfPages: 0 }),
                dataType: 'json',
                contentType: 'application/json'
            });
        }
    }
});

