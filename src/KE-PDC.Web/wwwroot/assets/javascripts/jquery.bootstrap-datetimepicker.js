/*! version : 4.14.30
 =========================================================
 bootstrap-datetimejs
 https://github.com/Eonasdan/bootstrap-datetimepicker
 Copyright (c) 2015 Jonathan Peterson
 =========================================================
 */
/*
 The MIT License (MIT)

 Copyright (c) 2015 Jonathan Peterson

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 */
/*global define:false */
/*global exports:false */
/*global require:false */
/*global jQuery:false */
/*global moment:false */
; (function ($, window, undefined)
{
    /*jshint validthis: true */
    "use strict";

    var i18n = window._i18n.datetimepickerTooltips;

    $.extend($.fn.datetimepicker.defaults, {
        //locale: 'th',
        icons: {
            time: 'ion-ios-clock-outline',
            date: 'ion-ios-calendar-outline',
            up: 'ion-ios-arrow-up',
            down: 'ion-ios-arrow-down',
            previous: 'ion-ios-arrow-thin-left',
            next: 'ion-ios-arrow-thin-right',
            today: 'ion-ios-reload',
            clear: 'ion-ios-trash-outline',
            close: 'ion-ios-close-outline'
        },
        tooltips: {
            today: i18n.today,
            clear: i18n.clear,
            close: i18n.close,
            selectMonth: i18n.selectMonth,
            prevMonth: i18n.prevMonth,
            nextMonth: i18n.nextMonth,
            selectYear: i18n.selectYear,
            prevYear: i18n.prevYear,
            nextYear: i18n.nextYear,
            selectDecade: i18n.selectDecade,
            prevDecade: i18n.prevDecade,
            nextDecade: i18n.nextDecade,
            prevCentury: i18n.prevCentury,
            nextCentury: i18n.nextCentury,
            pickHour: i18n.pickHour,
            incrementHour: i18n.incrementHour,
            decrementHour: i18n.decrementHour,
            pickMinute: i18n.pickMinute,
            incrementMinute: i18n.incrementMinute,
            decrementMinute: i18n.decrementMinute,
            pickSecond: i18n.pickSecond,
            incrementSecond: i18n.incrementSecond,
            decrementSecond: i18n.decrementSecond,
            togglePeriod: i18n.togglePeriod,
            selectTime: i18n.selectTime
        },
    });
})(jQuery, window);