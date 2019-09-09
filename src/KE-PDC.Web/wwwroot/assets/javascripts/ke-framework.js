// ESCAPE HTML
// ======================
function escapeHtml(string) {

    return String(string).replace(/[&<>"'\/]/g, function (s) {
        return entityMap[s];
    });
}


var entityMap = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': '&quot;',
    "'": '&#39;',
    "/": '&#x2F;'
};


// THAI ID CARD SPACE FORMAT
function thaiIdCardFormat(string, separator) {
    if (string == null) {
        return '-';
    }

    var str = [];

    separator = separator || ' ';

    str[0] = string.substr(0, 1);
    str[1] = string.substr(1, 4);
    str[2] = string.substr(5, 5);
    str[3] = string.substr(10, 2);
    str[4] = string.substr(12, 1);

    return str[0] + separator + str[1] + separator + str[2] + separator + str[3] + separator + str[4];
}


// BANK BBL SPACE FORMAT
function bankFormat(string, separator) {
    if (string == null) {
        return '-';
    }
    var str = [];

    separator = separator || ' ';

    str[0] = string.substr(0, 3);
    str[1] = string.substr(3, 1);
    str[2] = string.substr(4, 5);
    str[3] = string.substr(9, 1);

    return str[0] + separator + str[1] + separator + str[2] + separator + str[3];
}


// CHECK SUM BANK ACCOUNT NO
function checkBankAccountNo(value) {
    var sum = 0;

    value = value.toString();

    if (value.length != 10)
        return false;

    for (i = 0; i < 9; i++) {
        var v = value.charAt(i) * (i % 2 == 0 ? 2 : 1);

        v = v > 9 ? (v % 10) + 1 : v;

        sum += v
    }

    sum = sum % 10;
    sum = sum == 0 ? sum : 10 - sum;

    return value.charAt(9) == sum.toString();
}


+function ($) {
    'use strict';

    // PAGE LOADER
    // ======================
    $.pageLoader = (function (toggle) {
        if (typeof toggle === "undefined") {
            toggle = "hide";
        }

        if ($("body > .page-loader").hasClass("first-time")) {
            $("body > .page-loader").removeClass('bg-m-gray bg-m-lighten-2');
        }

        if (toggle == "show") {
            $("body > .page-loader").fadeIn(400);
        }
        else {
            $("#wrapper").addClass('in');
            $("body > .page-loader").fadeOut(400).removeClass('first-time');
        }
    });


    // GET CLASS TEXTBY SERVICE CODE
    // ======================
    $.serviceClassText = (function (service_code) {
        switch (service_code) {
            case "ND": return "text-m-light-green"; break;
            case "2D": return "text-m-lime bg-m-darken-3"; break;
            case "3D": return "text-m-brown"; break;
            default:
                return "text-m-gray";
        }
    });


    // GET CLASS BACKGROUND  BY SERVICE CODE
    // ======================
    $.serviceClassBg = (function (service_code) {
        switch (service_code) {
            case "ND": return "bg-m-light-green"; break;
            case "2D": return "bg-m-lime bg-m-darken-3"; break;
            case "3D": return "bg-m-brown"; break;
            default:
                return "bg-m-gray";
        }
    });
}(jQuery);

$(document).ready(function () {
    // Set default values for future Ajax requests
    // ======================
    $.ajaxSetup({
        statusCode: {
            401: function () {
                console.log("Unauthorized");
                location.reload();
            },
            500: function () {
                $("#error-status").find(".error-status-code").text("500")
                .end().find(".error-status-title").text(_i18n.ajaxStatus501.title)
                .end().find(".error-status-desc").text(_i18n.ajaxStatus501.desc)
                .end().show();
                $("body").addClass("error-page").css("overflow-y", "hidden");
            }
        }
    });
});