$(document).ready(function () {

    // INITIALIZATION
    // ======================
    setTimeout($.pageLoader(), 1000);

    $("body").on("click", "[data-ke-action]", function (e) {
        e.preventDefault();
        var $this = $(this),
            action = $(this).data("ke-action");
        switch (action) {
            case "sidebar-open":
                var target = $this.data("ke-target"),
                    backdrop = '<div data-ke-action="sidebar-close" class="ke-backdrop" />';
                $("body").addClass("sidebar-toggled");
                $("#header, #container").append(backdrop);
                $this.addClass("toggled"), $(target).addClass("toggled");
                break;
            case "sidebar-close":
                $("body").removeClass("sidebar-toggled");
                $(".ke-backdrop").remove();
                $(".sidebar, .header-toggle-nav").removeClass("toggled");
                break;

            case "sidebar-minimize":
                $("#header").addClass("header-alt");
                $("#content").addClass("content-alt");
                $("#sidebar").addClass("sidebar-alt");
                $("#footer").addClass("footer-alt");
                $this.addClass("hidden");
                $('[data-ke-action="sidebar-maximize"]').removeClass("hidden");
                $("body").removeClass("sidebar-toggled");
                $(".ke-backdrop").remove();
                $(".sidebar, .header-toggle-nav").removeClass("toggled");
                $.cookie("sidebar", "minimize");
                break;
            case "sidebar-maximize":
                $("#header").removeClass("header-alt");
                $("#content").removeClass("content-alt");
                $("#sidebar").removeClass("sidebar-alt");
                $("#footer").removeClass("footer-alt");
                $this.addClass("hidden");
                $('[data-ke-action="sidebar-minimize"]').removeClass("hidden");
                $(".ke-backdrop").remove();
                $.cookie("sidebar", "maximize");
                break;

            case "nav-main-top-close":
                $("#nav-main-top > li.subnav-open > .collapse.in").removeClass("in").attr('aria-expanded', false);
                $("#nav-main-top > li.subnav-open > a").attr("aria-expanded", false);
                $("#nav-main-top > li.subnav-open").removeClass("subnav-open");
                $(".ke-backdrop").remove();
                break;

            case "locale-picker":
                $.pageLoader("show");

                var locale = $this.data("ke-value");

                $.post(appRoot + "Locale", { culture: locale }, function () {
                    window.location.reload();
                }).fail(function () {
                    swal("Sorry", "Can't change language!", "error");
                }).always(function () {
                    $.pageLoader("hide");
                });
                break;
        }
    });


    // MOMENTJS
    typeof moment === "function" && moment.locale("th");


    // SIDEBAR
    // ======================
    var sidebar = $.cookie("sidebar");

    if (sidebar === "minimize") {
        $('[data-ke-action="sidebar-minimize"]').trigger("click");
    }
    else {
        $('[data-ke-action="sidebar-maximize"]').trigger("click");
    }

    var setScrollNav = function () {
        return $("#nav-main-top > li > ul.nav").css("max-height", $(window).height() - ($("#header").height() + 10));
    }

    setTimeout(function () {
        $("#sidebar")[0] && $("#sidebar").perfectScrollbar();
        $('#sidebar')[0] && setScrollNav().perfectScrollbar();
    }, 1000);
    $(window).resize(function () {
        $('#sidebar')[0] && $('#sidebar').perfectScrollbar('update');
        $('#sidebar')[0] && setScrollNav().perfectScrollbar("update");
    });


    // NAV MAIN
    // ======================
    $("#nav-main-top")[0] && $("#nav-main-top").metisMenu({
        activeClass: "subnav-open"
    }).on('show.metisMenu', function (event) {
        var backdrop = '<div data-ke-action="nav-main-top-close" class="ke-backdrop" />';
        $("#container").append(backdrop);
    }).on('hide.metisMenu', function (event) {
        $('.ke-backdrop[data-ke-action="nav-main-top-close"]').remove();
    });

    $("#nav-main")[0] && $("#nav-main").metisMenu();


    // AUTOSIZE
    // ======================
    $(".auto-size")[0] && autosize($(".auto-size"));


    // DATE TIME PICKER
    // ======================
    $(".date-time-picker")[0] && $(".date-time-picker").datetimepicker();
    $(".time-picker")[0] && $(".time-picker").datetimepicker({
        format: "LT"
    });
    $(".date-picker")[0] && $(".date-picker").datetimepicker({
        format: "DD/MM/YYYY"
    });
    $(".date-picker").on("dp.show", function () {
        var minDate = $(this).data('min-date') ? moment($(this).data('min-date'), 'DD/MM/YYYY') : false;
        var maxDate = $(this).data('max-date') ? moment($(this).data('max-date'), 'DD/MM/YYYY') : false;
        $(this).data("DateTimePicker").minDate(minDate);
        $(this).data("DateTimePicker").maxDate(maxDate);
    });
    $(".date-picker").on("dp.hide", function () {
        $(this).blur();
    });


    // DATE PICKER FORM SEARCH
    // ======================
    $("body").on("click", 'input[name="UseDateFrom"], input[name="UseDateTo"]', function (e) {
        var $this = $(this),
            target = $this.data("ke-target");
        $(target).prop("disabled", !$this.prop("checked"));
    });

    $('input[name="DateFrom"]')[0] && $('input[name="DateFrom"]').datetimepicker({
        locale: culture,
        format: "DD/MM/YYYY",
    })
    .on("dp.change", function (e) {
        $('input[name="DateTo"]')[0] && $('input[name="DateTo"]').data("DateTimePicker").minDate(e.date);
    });

    $('input[name="DateTo"]')[0] && $('input[name="DateTo"]').datetimepicker({
        locale: culture,
        useCurrent: false,
        format: "DD/MM/YYYY",
    })
    .on("dp.change", function (e) {
        $('input[name="DateFrom"]')[0] && $('input[name="DateFrom"]').data("DateTimePicker").maxDate(e.date);
    });

    $('input[name="MonthYear"]')[0] && $('input[name="MonthYear"]').datetimepicker({
        locale: culture,
        format: "MM/YYYY",
        viewMode: 'months'
    });


    // BOOTSTRAP TAGS INPUT
    // ======================
    $(".tagsinput")[0] && $(".tagsinput").tagsinput({
        tagClass: "label label-primary",
        confirmKeys: [13, 32, 44],
        delimiter: /[\s,]+/
    });


    // TOOLTIP AND POPOVER
    // ======================
    $(".maskmoney")[0] && $(".maskmoney").maskMoney();


    // TOOLTIP AND POPOVER
    // ======================
    $('[data-toggle="tooltip"]')[0] && $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]')[0] && $('[data-toggle="popover"]').popover();


    // RENDER BRACH TYPE TO SELECT
    // ======================
    var renderBranchType = function () {
        if (window.branchTypeList === false) {
            console.log("BranchType get fail");
            return;
        }
        else if (window.branchTypeList === undefined) {
            console.log("waiting BranchType");

            setTimeout(function () {
                renderBranchType();
            }, 1000);
            return;
        }

        var $BranchType = $('select[name="BranchType"]');
        $BranchType.prop("disabled", false).find('option[value="-1"]').remove();

        $.each(window.branchTypeList, function (key, value) {
            var option = $('<div>'),
                optgroup = $('<optgroup>');

            $.each(value.types, function (k, v) {
                option.append('<option class="text-uppercase" value="' + v.typeId + '" data-subtext="' + v.typeDescription + '">' + v.typeId + '</option>');
            });

            if (value.typeGroupId === null) {
                $BranchType.append(option.html());
            }
            else {
                optgroup.attr('label', value.typeGroupId).append(option.html());
                $BranchType.append(optgroup);
            }
        });

        $BranchType.selectpicker('refresh');
    };

    var renderBranchList = function (result) {
        var $BranchList = $('select[name="BranchList"]');
        var $RegionList = $('select[name="Region"]');
       
        $BranchList.prop("disabled", false).find('option[value="-1"]').remove();
        $RegionList.find("option").remove();
        var Rid = [];
        var Rname = [];
        $.each(result, function (key, value) {
            
            $BranchList.append('<option class="text-uppercase" value="' + value.bid + '">' + value.bName + '</option>');
            Rname.push(value.rName);
            Rid.push(value.rID);
            
        });
        var regionid = Rid.filter(function (itm, i, Rid) {return i === Rid.indexOf(itm);});
        var regionname = Rname.filter(function (itm, i, Rname) { return i === Rname.indexOf(itm); });
        console.log(regionid);
        console.log(regionname);

        $.each(regionname, function (index, value) {            
            $RegionList.append('<option class="text-uppercase" value="' + regionid[index] + '">' + value + '</option>');
        });
        $BranchList.selectpicker('refresh');
        $RegionList.selectpicker('refresh');
    };

    var renderBranchList_1 = function (result) {
        var $BranchList = $('select[name="BranchList"]');

        $BranchList.prop("disabled", false).find('option[value="-1"]').remove(); 
        $BranchList.find("option").remove();
        $.each(result, function (key, value) {

            $BranchList.append('<option class="text-uppercase" value="' + value.bid + '">' + value.bName + '</option>');
            
        });
       
        $BranchList.selectpicker('refresh');
    };

    if ($('select[data-ke-action="branch-type-list"]').length > 0) {
        var $BranchType = $('select[name="BranchType"]'),
            $BranchList = $('select[name="BranchList"]'),
            $RegionList = $('select[name="Region"]');

        $BranchType.prop("disabled", true);

        $BranchType.on("change", function () {
            var $this = $(this);
            $BranchList.prop("disabled", true).find("option").remove();           
            $RegionList.find("option").remove();           
            if ($this.val() === null) {
                $BranchList.selectpicker('refresh');
                $RegionList.selectpicker('refresh');
                return false;
            }

            $BranchList.append('<option value="-1" selected>Loading...</option>').selectpicker('refresh');
            $.post(endpoint + "Branch/Get", {
                type: (typeof $this.val() === 'object' ? $this.val().join() : $this.val())
                
            }, function (response) {
                renderBranchList(response.result);
            }).fail(function (response) {
                swal(response.status.toString(), response.statusText.toString(), "error");
            }).always(function (response) {
                $.pageLoader();
            });
        });

        $RegionList.on("change", function () {
            var $this = $(this);
            var BranchType = $BranchType;

            if ($this.val() === null) {
                $BranchList.selectpicker('refresh');
                return false;
            }
           
            $.post(endpoint + "Branch/GetBranList", {                
                type: (typeof BranchType.val() === 'object' ? BranchType.val().join() : BranchType.val()),
                region: (typeof $this.val() === 'object' ? $this.val().join() : $this.val())

            }, function (response) {
               renderBranchList_1(response.result);
            }).fail(function (response) {
                swal(response.status.toString(), response.statusText.toString(), "error");
            }).always(function (response) {
                $.pageLoader();
            });
        });


        $("body").addClass("ajax-loading");

        //setTimeout(function () {
        //    renderBranchType();
        //}, 500);
        $.get(endpoint + "Branch/Type", function (response) {
            if (response.success) {
                console.log(response.result);
                window.branchTypeList = response.result;
                renderBranchType();
            }
        }).fail(function (response) {
            window.branchTypeList = false;

            if (response.status === 0 || response.status === 500) {
                if (response.status === 0) {
                    $("#error-status").find(".error-status-code").text("Oops!");
                    $("#error-status").find(".error-status-title").text("Cannot Connect To Server");
                    $("#error-status").find(".error-status-desc").text("This one's your fault, not ours. Check your settings and retry.");
                }
                else if (response.status === 500) {
                    $("#error-status").find(".error-status-code").text("500");
                    $("#error-status").find(".error-status-title").text("Something went wrong");
                    $("#error-status").find(".error-status-desc").text("Try that again, and if it still doesn't work, let us know");
                }

                $("#wrapper").hide();
                $("#error-status").show();
                $("body").addClass("error-page").css("overflow-y", "hidden");
            }
            else {
                swal(response.status.toString(), response.statusText.toString(), "error");
            }
        }).always(function() {
            $.pageLoader();
            $("body").removeClass("ajax-loading");
        });
    }

    $.get(endpoint + "Branch/Discount", function (response) {
        if (response.success) {
            console.log(response.data);

            //getId DiscountSelection
            var $DiscountType = $('select[name="DiscountType"]');

            $(response.data).each(function (item, itemname) {
                //    var $option = $("<option/>").attr("value", a.).text(a.);
                //    $('[data-entry=item]').append($option);
                $DiscountType.append('<option class="text-uppercase"value=' + itemname.key + '>' + itemname.value + '</option>');
            });

            $DiscountType.selectpicker('refresh');
        }
    }).fail(function (response) {
        window.branchTypeList = false;

        if (response.status === 0 || response.status === 500) {
            if (response.status === 0) {
                $("#error-status").find(".error-status-code").text("Oops!");
                $("#error-status").find(".error-status-title").text("Cannot Connect To Server");
                $("#error-status").find(".error-status-desc").text("This one's your fault, not ours. Check your settings and retry.");
            }
            else if (response.status === 500) {
                $("#error-status").find(".error-status-code").text("500");
                $("#error-status").find(".error-status-title").text("Something went wrong");
                $("#error-status").find(".error-status-desc").text("Try that again, and if it still doesn't work, let us know");
            }

            $("#wrapper").hide();
            $("#error-status").show();
            $("body").addClass("error-page").css("overflow-y", "hidden");
        }
        else {
            swal(response.status.toString(), response.statusText.toString(), "error");
        }
    }).always(function () {
        $.pageLoader();
        $("body").removeClass("ajax-loading");
    });

    if ($('select[data-ke-action="fc-list"]').length > 0) {
        $('select[data-ke-action="fc-list"]').on("change", function () {
            var $this = $(this),
                $target = $($(this).data("ke-target"));

            $target.prop("disabled", true).find("option").remove().end().append('<option value="-1" selected>Loading...</option>').selectpicker('refresh');

            $.get(endpoint + "UserShops", { FC: $this.val() }, function (response) {
                renderBranchList(response.result);
            }).fail(function (response) {
                swal(response.status.toString(), response.statusText.toString(), "error");
            }).always(function (response) {
                $.pageLoader();
            });
        });
    }
});