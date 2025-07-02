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