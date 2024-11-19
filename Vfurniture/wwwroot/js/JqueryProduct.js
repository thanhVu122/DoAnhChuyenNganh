$(document).ready(function () {
    var mainImageSrc = $('#product-image-single img').attr('src');
    var timeout;

    $('.small-image').on('click', function () {
        clearTimeout(timeout);

        var selectedImageSrc = $(this).attr('src');

        $('#product-image-single img').fadeOut(200, function () {
            $(this).attr('src', selectedImageSrc).fadeIn(200);
        });

        timeout = setTimeout(function () {
            $('#product-image-single img').fadeOut(200, function () {
                $(this).attr('src', mainImageSrc).fadeIn(200);
            });
        }, 5000);
    });
});
