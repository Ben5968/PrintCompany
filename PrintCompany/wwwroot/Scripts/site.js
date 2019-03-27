$('form').validate(
    {
        errorPlacement: function (error, element) { },
        highlight: function (element) {
            $(element)
                .closest('.form-group')
                .addClass('has-error');
        },
        unhighlight: function (element) {
            $(element)
                .closest('.form-group')
                .removeClass('has-error')
        }
    }
);

$('select').on('change', function () {
    $(this).valid();
});