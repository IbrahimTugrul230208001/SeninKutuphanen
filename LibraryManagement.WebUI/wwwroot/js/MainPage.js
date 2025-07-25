
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



const menuBtn = document.getElementById('menu-btn');
const mobileMenu = document.getElementById('mobile-menu');
menuBtn.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
    mobileMenu.classList.toggle('scale-y-0');
});
function checkEnter(event) {
    if (event.key === 'Enter') {
        searchBooks();
    }
}

function searchBooks(page = 1) {
    console.log("searching books...");
    const input = $('#search-input').val().trim();
    const crit = $('#search-criteria').val();
    const $list = $('#bookList');
    $('#search-predictions').hide();
    $('#overlay').addClass('opacity-0 pointer-events-none').removeClass('opacity-100');

    // 1 . block short queries (<3 chars)
    if (input.length < 3 && input.length > 0) {
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
        $btn.removeClass('bg-gray-200 hover:bg-blue-600 cursor-pointer').addClass('bg-green-600');
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


document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById('search-input');
    const overlay = document.getElementById('overlay');
    const container = document.querySelector('.search-container');
    const preds = document.getElementById('search-predictions');
    const searchContainer = document.querySelector('.search-container');
    // Show overlay and predictions on input focus (if there are predictions)
    input.addEventListener('focus', () => {
        overlay.classList.remove('opacity-0', 'pointer-events-none');
        overlay.classList.add('opacity-100');
        if (preds.children.length > 0) preds.style.display = '';
    });

    // Hide overlay and predictions on input blur (unless moving to prediction)
    input.addEventListener('blur', () => {
        setTimeout(() => {
            if (
                !container.contains(document.activeElement) ||
                (preds.style.display === 'none')
            ) {
                searchContainer.classList.add("rounded-b-3xl");
                overlay.classList.add('opacity-0', 'pointer-events-none');
                overlay.classList.remove('opacity-100');
                searchContainer.classList.add("rounded-b-3xl");
                preds.style.display = 'none';
            }
        }, 50);
    });

    // Hide overlay/predictions if overlay is clicked
    overlay.addEventListener('click', () => {
        input.blur();
        preds.style.display = 'none';
    });

    // Hide predictions when a prediction is clicked (optional: also run a select action)
    preds.addEventListener('mousedown', e => {
        // Use mousedown to prevent blur from hiding too early
        preds.style.display = 'none';
        overlay.classList.add('opacity-0', 'pointer-events-none');
        overlay.classList.remove('opacity-100');
    });

    // When showing predictions in your AJAX:
    // $('#search-predictions').show()   <-- For vanilla JS: preds.style.display = ''
    // $('#search-predictions').hide()   <-- preds.style.display = 'none'
    // (already handled above, but make sure to only show if there are results)

    window.checkEnter = e => e.key === 'Enter' && searchBooks();
});


let autocompleteTimeout;

$('#search-input').on('input', function () {
    clearTimeout(autocompleteTimeout);
    const query = this.value.trim();
    const crit = $('#search-criteria').val();

    if (query.length < 2) {
        $('#search-predictions').hide();
        return;
    }

    // Debounce: only request after 200ms pause
    autocompleteTimeout = setTimeout(() => {
        $.get(`/api/books/predict`, { query, crit }, function (predictions) {
            const searchContainer = document.querySelector('.search-container');
            searchContainer.classList.remove("rounded-b-3xl");


            // predictions = array of strings or objects from server
            console.log(predictions); // <--- SEE WHAT'S RETURNED
            if (!predictions.length) {
                $('#search-predictions').hide();
                return;
            }
            // Render dropdown
            $('#search-predictions').html(
                predictions.slice(0, 8).map(p =>
                    `
                      <div class="flex items-center group prediction-item" data-value="${p}">
    <div class="flex items-center justify-center w-10 h-10 bg-gray-600 group-hover:bg-gray-500">
      <svg xmlns="http://www.w3.org/2000/svg"
           viewBox="0 0 24 24"
           class="h-6 w-6 text-white"
           fill="none"
           stroke="currentColor"
           stroke-width="2">
        <circle cx="11" cy="11" r="7" />
        <line x1="16.5" y1="16.5" x2="21" y2="21" stroke-linecap="round" />
      </svg>
    </div>
    <div class="px-4 py-2 w-full group-hover:bg-gray-500">
      ${p}
    </div>
  </div>
                    `
                ).join('')
            ).show();
        }, 'json');
    }, 200);
});


// Optional: click prediction to fill input
$('#search-predictions').on('mousedown', '.prediction-item', function () {
    const val = $(this).data('value');
    $('#search-input').val(val);
    console.log(`Selected prediction: ${val}`);
    $('#search-predictions').hide();
    $('#overlay').addClass('opacity-0 pointer-events-none').removeClass('opacity-100');
    searchBooks();
});

