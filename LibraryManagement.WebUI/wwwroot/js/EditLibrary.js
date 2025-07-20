$('#bookList').on('click', '.delete-btn', function (e) {
    e.preventDefault();
    const $btn = $(this);
    const $shelf = $btn.closest('.shelf');
    console.log('this=', this, '.shelf count=', $shelf.length);
    const id = $shelf.data('id');
    console.log('Deleting shelf with ID:', id);
    $.ajax({
        type: 'DELETE',
        url: '/Kullanici/Delete',
        data: JSON.stringify(id),
        contentType: 'application/json',
        dataType: 'json'
    })
        .done(function (res) {
            if (res.success) {
                // animate, then remove
                $shelf.slideUp(200, () => $shelf.remove());
            } else {
                console.error('Delete failed');
            }
        })
        .fail(function (err) {
            console.error('Server error', err);
        });
});

