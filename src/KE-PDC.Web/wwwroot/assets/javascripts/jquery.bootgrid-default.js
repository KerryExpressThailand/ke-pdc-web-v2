/*! 
 * jQuery Bootgrid v1.3.1 - 09/11/2015
 * Copyright (c) 2014-2015 Rafael Staib (http://www.jquery-bootgrid.com)
 * Licensed under MIT http://www.opensource.org/licenses/MIT
 */
;(function ($, window, undefined)
{
    /*jshint validthis: true */
    "use strict";

    var i18n = window._i18n.bootgridLabels;

    $.extend($.fn.bootgrid.Constructor.defaults, {
        ajax: true,
        ajaxSettings: {
            method: "GET",
            cache: false,
        },
        requestHandler: function (request) {
            var request = {
                page: request.current,
                perPage: request.rowCount,
                order: Object.keys(request.sort)[0],
                direction: request.sort[Object.keys(request.sort)[0]],
                searchPhrase: request.searchPhrase,
            };

            return request;
        },
        responseHandler: function (response) {
            var response = {
                current: response.resultInfo.page,
                rowCount: response.resultInfo.perPage,
                rows: response.result,
                total: response.resultInfo.totalCount,
            };

            return response;
        },
        rowCount: [30, 50, 100, 200],
    });
    
    $.extend($.fn.bootgrid.Constructor.defaults.css, {
        icon: "icon",
        iconColumns: "ion-grid",
        iconDown: "ion-android-arrow-dropdown",
        iconRefresh: "ion-android-refresh",
        iconSearch: "ion-android-search",
        iconUp: "ion-android-arrow-dropup"
    });

    $.extend($.fn.bootgrid.Constructor.defaults.templates, {
        loading: "<tr><td colspan=\"{{ctx.columns}}\" class=\"loading\"><div class=\"loader loader-pulse loader-md\"></div></td></tr>",
    });

    $.extend($.fn.bootgrid.Constructor.defaults.labels, {
        all: i18n.all,
        infos: i18n.infos,
        loading: i18n.loading,
        noResults: i18n.noResults,
        refresh: i18n.refresh,
        search: i18n.search
    });

    $.extend($.fn.bootgrid.Constructor.defaults.converters, {
        datetime: {
            from: function (value) { return moment(value); },
            to: function (value) {
                if (value == null) return "-";
                var v = typeof value.format !== "function" ? moment(value) : value;
                return v.format("DD/MM/YYYY HH:mm");
            }
        },
        datetimeS: {
            from: function (value) { return moment(value); },
            to: function (value) {
                if (value == null) return "-";
                var v = typeof value.format !== "function" ? moment(value) : value;
                return v.format("DD/MM HH:mm");
            }
        },
        date: {
            from: function (value) { return moment(value); },
            to: function (value) {
                if (value == null) return "-";
                var v = typeof value.format !== "function" ? moment(value) : value;
                return v.format("DD/MM/YYYY");
            }
        },
        dateS: {
            from: function (value) { return moment(value); },
            to: function (value) {
                if (value == null) return "-";
                var v = typeof value.format !== "function" ? moment(value) : value;
                return v.format("DD/MM");
            }
        },
        time: {
            from: function (value) { return moment(value); },
            to: function (value) {
                if (value == null) return "-";
                var v = typeof value.format !== "function" ? moment(value) : value;
                return v.format("HH:mm");
            }
        },
        numeric: {
            from: function (value) { return value; },
            to: function (value) {
                return value == null ? 0 : parseInt(value).toLocaleString();
            }
        },
        decimal: {
            from: function (value) { return value; },
            to: function (value) {
                return value == null ? 0.00 : parseFloat(value).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }
    }
    });
})(jQuery, window);