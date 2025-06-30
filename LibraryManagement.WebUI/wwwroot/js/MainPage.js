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

// Add an event listener to capture the "data-id" attribute when a button is clicked
$(document).on('click', '.add-btn', function () {
    const bookId = $(this).closest('div').data('id'); // Get the "data-id" attribute
    const action = $(this).data('state'); // Get the current state (e.g., "plus")

    // Send the data to the backend using AJAX
    $.ajax({
        url: '/Kullanici/Add', // Backend endpoint
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ bookId: bookId, action: action }),
        success: function (response) {
            console.log('Success:', response);
            // Optionally update the UI based on the response
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });
});              