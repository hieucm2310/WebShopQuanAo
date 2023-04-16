$(document).ready(function () {
    $.validator.setDefaults({ ignore: null });
})

$('input[name="payMethod"]').on('change', function (e) {
    $("#paymentMethod").val(e.target.value);
});
