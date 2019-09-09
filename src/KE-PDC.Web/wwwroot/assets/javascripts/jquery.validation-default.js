$(document).ready(function () {
    'use strict';

    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            console.log(element);
            if (element.parent('.input-group, .btn-group').length) {
                error.insertAfter(element.parent());
            } else if (element.next('label, .bootstrap-select').length) {
                error.insertAfter(element.next());
            } else if (element.parents('.fileinput').length) {
                error.insertAfter(element.parents('.fileinput'));
            } else if (element.parents('.radio-ma').length) {
                element.parents('.form-group').append(error);
            } else {
                error.insertAfter(element);
            }
        }
    });
});