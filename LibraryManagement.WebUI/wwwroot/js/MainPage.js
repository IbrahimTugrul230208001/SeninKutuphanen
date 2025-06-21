$(document).ready(function () {
    const searchInput = $('#searchInput');
    const searchFilter = $('#searchFilter');
    const items = $('#bookList').children('div');

    function filter() {
        const query = searchInput.val().toLowerCase();
        const filter = searchFilter.val();
        items.each(function () {
            const title = $(this).data('title').toLowerCase();
            const author = ($(this).data('author') || '').toLowerCase();
            let match = false;
            if (filter === 'title') {
                match = title.includes(query);
            } else if (filter === 'author') {
                match = author.includes(query);
            }
            $(this).toggle(match || query === '');
        });
    }

    searchInput.on('input', filter);
    searchFilter.on('change', filter);

    // sample add to library button
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
    const menuBtn = document.getElementById('menu-btn');
    const mobileMenu = document.getElementById('mobile-menu');
    if (menuBtn && mobileMenu) {
        menuBtn.addEventListener('click', () => {
            mobileMenu.classList.toggle('hidden');
        });
    }
});
