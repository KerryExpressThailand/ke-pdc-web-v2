/*! 
 * Kerry Express jQuery Thumbnail Grid v0.0.1
 * Copyright (c) 2016 Chettapong Pinsuwan (https://github.com/chettapong)
 * Licensed under MIT http://www.opensource.org/licenses/MIT
 */
; (function ($, window, undefined) {
    /*jshint validthis: true */
    "use strict";


    // THUMBNAIL GRID INTERNAL FIELDS
    // ====================

    var namespace = ".ke.jquery.thumbnailgrid";


    // THUMBNAIL GRID INTERNAL FUNCTIONS
    // =====================

    function appendRow(row) {
        var that = this;

        function exists(item) {
            return that.identifier && item[that.identifier] === row[that.identifier];
        }

        if (!this.rows.contains(exists)) {
            this.rows.push(row);
            return true;
        }

        return false;
    }

    function findFooterAndHeaderItems(selector) {
        var footer = (this.footer) ? this.footer.find(selector) : $(),
            header = (this.header) ? this.header.find(selector) : $();
        return $.merge(footer, header);
    }

    function getParams(context) {
        return (context)
            ? $.extend({}, this.cachedParams, { ctx: context })
            : this.cachedParams;
    }

    function getRequest() {
        var request = {
            current: this.current,
            rowCount: this.rowCount,
            sort: this.sortDictionary,
            searchPhrase: this.searchPhrase
        },
            post = this.options.post;

        post = ($.isFunction(post)) ? post() : post;
        return this.options.requestHandler($.extend(true, request, post));
    }

    function getCssSelector(css) {
        return "." + $.trim(css).replace(/\s+/gm, ".");
    }

    function getUrl() {
        var url = this.options.url;
        return ($.isFunction(url)) ? url() : url;
    }

    function init() {
        this.element.trigger("initialize" + namespace);

        loadColumns.call(this);
        loadRows.call(this); // Loads rows from HTML tbody tag if ajax is false
        prepareThumbnailGrid.call(this);
        renderSearchField.call(this);
        renderActions.call(this);
        loadData.call(this);
        this.element.trigger("initialized" + namespace);
    }

    function highlightAppendedRows(rows) {
        if (this.options.highlightRows) {
            // todo: implement
        }
    }

    function loadColumns() {
        var that = this,
            sorted = false;

        /*jshint -W018*/
        $.each(this.options.columns, function (key, data) {
            var column = {
                key: key,
                id: data.key,
                identifier: that.identifier === null && data.identifier || false,
                converter: that.options.converters[data.converter] || that.options.converters["string"],
                formatter: that.options.formatters[data.formatter] || null,
                order: (!sorted && (data.order === "asc" || data.order === "desc")) ? data.order : null,
                searchable: !(data.searchable === false), // default: true
            };

            that.columns.push(column);

            if (column.order != null) {
                that.sortDictionary[column.id] = column.order;
            }

            // Prevents multiple identifiers
            if (column.identifier) {
                that.identifier = column.id;
                that.converter = column.converter;
            }

            // ensures that only the first order will be applied in case of multi sorting is disabled
            if (!that.options.multiSort && column.order !== null) {
                sorted = true;
            }
        });
        /*jshint +W018*/
    }

    function loadData() {
        var that = this;

        this.element._bgBusyAria(true).trigger("load" + namespace);
        showLoading.call(this);

        function containsPhrase(row) {
            var column,
                searchPattern = new RegExp(that.searchPhrase, (that.options.caseSensitive) ? "g" : "gi");

            for (var i = 0; i < that.columns.length; i++) {
                column = that.columns[i];
                if (column.searchable &&
                    column.converter.to(row[column.id]).search(searchPattern) > -1) {
                    return true;
                }
            }

            return false;
        }

        function update(rows, total) {
            that.currentRows = rows;
            setTotals.call(that, total);

            if (!that.options.keepSelection) {
                that.selectedRows = [];
            }

            renderRows.call(that, rows);

            if (that.options.mode === 2) {
                renderInfos.call(that);
                renderPagination.call(that);
            }

            that.element._bgBusyAria(false).trigger("loaded" + namespace);
        }

        if (this.options.ajax) {
            var request = getRequest.call(this),
                url = getUrl.call(this);

            if (url === null || typeof url !== "string" || url.length === 0) {
                throw new Error("Url setting must be a none empty string or a function that returns one.");
            }

            // aborts the previous ajax request if not already finished or failed
            if (this.xqr) {
                this.xqr.abort();
            }

            var settings = {
                url: url,
                data: request,
                success: function (response) {
                    that.xqr = null;

                    if (typeof (response) === "string") {
                        response = $.parseJSON(response);
                    }

                    response = that.options.responseHandler(response);

                    that.current = response.current;
                    update(response.rows, response.total);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    that.xqr = null;

                    if (textStatus !== "abort") {
                        renderNoResultsRow.call(that); // overrides loading mask
                        that.element._bgBusyAria(false).trigger("loaded" + namespace);
                    }
                }
            };
            settings = $.extend(this.options.ajaxSettings, settings);

            this.xqr = $.ajax(settings);
        }
        else {
            var rows = (this.searchPhrase.length > 0) ? this.rows.where(containsPhrase) : this.rows,
                total = rows.length;
            if (this.rowCount !== -1) {
                rows = rows.page(this.current, this.rowCount);
            }

            // todo: improve the following comment
            // setTimeout decouples the initialization so that adding event handlers happens before
            window.setTimeout(function () { update(rows, total); }, 10);
        }
    }

    function loadRows() {
        if (!this.options.ajax) {
            var that = this,
                //rows = this.element.find("tbody > tr");
                rows = this.options.data;

            $.each(rows, function (key, data) {
                var row = {};
                $.each(that.columns, function (i, column) {
                    row[column.id] = column.converter.from(data[column.id]);
                });

                if (that.identifier === null || data[that.identifier]) {
                    appendRow.call(that, row);
                }
            });

            setTotals.call(this, this.rows.length);
            sortRows.call(this);
        }
    }

    function setTotals(total) {
        this.total = total;
        this.totalPages = (this.rowCount === -1) ? 1 :
            Math.ceil(this.total / this.rowCount);
    }

    function prepareThumbnailGrid() {
        var tpl = this.options.templates,
            wrapper = this.element;

        this.element.addClass(this.options.css.thumbnailgrid);
        // checks whether there is an inner element; otherwise creates one
        if (this.element.children(this.options.css.inner).length === 0) {
            this.element.append(tpl.inner.resolve(getParams.call(this)));
        }

        if (this.options.navigation & 1) {
            this.header = $(tpl.header.resolve(getParams.call(this, { id: this.element._bgId() + "-header" })));
            wrapper.prepend(this.header);
        }

        if ((this.options.navigation & 2) && this.options.mode !== 0) {
            this.footer = $(tpl.footer.resolve(getParams.call(this, { id: this.element._bgId() + "-footer" })));
            wrapper.append(this.footer);
        }
    }

    function renderActions() {
        if (this.options.navigation !== 0) {
            var css = this.options.css,
                selector = getCssSelector(css.actions),
                actionItems = findFooterAndHeaderItems.call(this, selector);

            if (actionItems.length > 0) {
                var that = this,
                    tpl = this.options.templates,
                    actions = $(tpl.actions.resolve(getParams.call(this)));

                // Refresh Button
                if (this.options.ajax) {
                    var refreshIcon = tpl.icon.resolve(getParams.call(this, { iconCss: css.iconRefresh })),
                        refresh = $(tpl.actionButton.resolve(getParams.call(this,
                        { content: refreshIcon, text: this.options.labels.refresh })))
                            .on("click" + namespace, function (e) {
                                // todo: prevent multiple fast clicks (fast click detection)
                                e.stopPropagation();
                                that.current = 1;
                                loadData.call(that);
                            });
                    actions.append(refresh);
                }

                if (this.options.mode === 2) {
                    // Row count selection
                    renderRowCountSelection.call(this, actions);
                }

                // Switch View Button
                if (this.options.switchview) {
                    var that = this,
                        switchWrapper = $(tpl.switchWrapper.resolve(getParams.call(this))),
                        switchIconGrid = tpl.icon.resolve(getParams.call(this, { iconCss: css.iconSwitchGrid })),
                        grid = $(tpl.actionButton.resolve(getParams.call(this,
                        { content: switchIconGrid, text: this.options.labels.gridView })))
                            .on("click" + namespace, function (e) {
                                // todo: action for grid
                                console.log("action for grid");
                                that.element.trigger("switch" + namespace, "grid");
                            }),

                        switchIconThumbnailGrid = tpl.icon.resolve(getParams.call(this, { iconCss: css.iconSwitchThumbnailGrid })),
                        thumbnailGrid = $(tpl.actionButtonActive.resolve(getParams.call(this,
                        { content: switchIconThumbnailGrid, text: this.options.labels.thumbnailGridView })))
                            .on("click" + namespace, function (e) {
                                // todo: action for thumbnailGrid
                                console.log("action for thumbnailGrid");
                                that.element.trigger("switch" + namespace, "thumbnail");
                            });

                    switchWrapper.append([grid, thumbnailGrid]);
                    actions.append(switchWrapper);
                }

                replacePlaceHolder.call(this, actionItems, actions);
            }
        }
    }

    function renderInfos() {
        if (this.options.navigation !== 0) {
            var selector = getCssSelector(this.options.css.infos),
                infoItems = findFooterAndHeaderItems.call(this, selector);

            if (infoItems.length > 0) {
                var end = (this.current * this.rowCount),
                    infos = $(this.options.templates.infos.resolve(getParams.call(this, {
                        end: (this.total === 0 || end === -1 || end > this.total) ? this.total : end,
                        start: (this.total === 0) ? 0 : (end - this.rowCount + 1),
                        total: this.total
                    })));

                replacePlaceHolder.call(this, infoItems, infos);
            }
        }
    }

    function renderNoResultsRow() {
        var selector = getCssSelector(this.options.css.inner),
            inner = this.element.children(selector).first(),
            tpl = this.options.templates,
            button = "";

        if (this.options.create) {
            button = tpl.actionButtonCreate.resolve(getParams.call(this))
        }

        inner.html(tpl.noResults.resolve(getParams.call(this, { content: button })));
    }

    function renderPagination() {
        if (this.options.navigation !== 0) {
            var selector = getCssSelector(this.options.css.pagination),
                paginationItems = findFooterAndHeaderItems.call(this, selector)._bgShowAria(this.rowCount !== -1);

            if (this.rowCount !== -1 && paginationItems.length > 0) {
                var tpl = this.options.templates,
                    current = this.current,
                    totalPages = this.totalPages,
                    pagination = $(tpl.pagination.resolve(getParams.call(this))),
                    offsetRight = totalPages - current,
                    offsetLeft = (this.options.padding - current) * -1,
                    startWith = ((offsetRight >= this.options.padding) ?
                        Math.max(offsetLeft, 1) :
                        Math.max((offsetLeft - this.options.padding + offsetRight), 1)),
                    maxCount = this.options.padding * 2 + 1,
                    count = (totalPages >= maxCount) ? maxCount : totalPages;

                renderPaginationItem.call(this, pagination, "first", "&laquo;", "first")
                    ._bgEnableAria(current > 1);
                renderPaginationItem.call(this, pagination, "prev", "&lt;", "prev")
                    ._bgEnableAria(current > 1);
                for (var i = 0; i < count; i++) {
                    var pos = i + startWith;
                    renderPaginationItem.call(this, pagination, pos, pos, "page-" + pos)
                        ._bgEnableAria()._bgSelectAria(pos === current);
                }

                if (count === 0) {
                    renderPaginationItem.call(this, pagination, 1, 1, "page-" + 1)
                        ._bgEnableAria(false)._bgSelectAria();
                }

                renderPaginationItem.call(this, pagination, "next", "&gt;", "next")
                    ._bgEnableAria(totalPages > current);
                renderPaginationItem.call(this, pagination, "last", "&raquo;", "last")
                    ._bgEnableAria(totalPages > current);

                replacePlaceHolder.call(this, paginationItems, pagination);
            }
        }
    }

    function renderPaginationItem(list, page, text, markerCss) {
        var that = this,
            tpl = this.options.templates,
            css = this.options.css,
            values = getParams.call(this, { css: markerCss, text: text, page: page }),
            item = $(tpl.paginationItem.resolve(values))
                .on("click" + namespace, getCssSelector(css.paginationButton), function (e) {
                    e.stopPropagation();
                    e.preventDefault();

                    var $this = $(this),
                        parent = $this.parent();
                    if (!parent.hasClass("active") && !parent.hasClass("disabled")) {
                        var commandList = {
                            first: 1,
                            prev: that.current - 1,
                            next: that.current + 1,
                            last: that.totalPages
                        };
                        var command = $this.data("page");
                        that.current = commandList[command] || command;
                        loadData.call(that);
                    }
                    $this.trigger("blur");
                });

        list.append(item);
        return item;
    }

    function renderRowCountSelection(actions) {
        var that = this,
            rowCountList = this.options.rowCount;

        function getText(value) {
            return (value === -1) ? that.options.labels.all : value;
        }

        if ($.isArray(rowCountList)) {
            var css = this.options.css,
                tpl = this.options.templates,
                dropDown = $(tpl.actionDropDown.resolve(getParams.call(this, { content: getText(this.rowCount) }))),
                menuSelector = getCssSelector(css.dropDownMenu),
                menuTextSelector = getCssSelector(css.dropDownMenuText),
                menuItemsSelector = getCssSelector(css.dropDownMenuItems),
                menuItemSelector = getCssSelector(css.dropDownItemButton);

            $.each(rowCountList, function (index, value) {
                var item = $(tpl.actionDropDownItem.resolve(getParams.call(that,
                    { text: getText(value), action: value })))
                        ._bgSelectAria(value === that.rowCount)
                        .on("click" + namespace, menuItemSelector, function (e) {
                            e.preventDefault();

                            var $this = $(this),
                                newRowCount = $this.data("action");
                            if (newRowCount !== that.rowCount) {
                                // todo: sophisticated solution needed for calculating which page is selected
                                that.current = 1; // that.rowCount === -1 ---> All
                                that.rowCount = newRowCount;
                                $this.parents(menuItemsSelector).children().each(function () {
                                    var $item = $(this),
                                        currentRowCount = $item.find(menuItemSelector).data("action");
                                    $item._bgSelectAria(currentRowCount === newRowCount);
                                });
                                $this.parents(menuSelector).find(menuTextSelector).text(getText(newRowCount));
                                loadData.call(that);
                            }
                        });
                dropDown.find(menuItemsSelector).append(item);
            });
            actions.append(dropDown);
        }
    }

    function renderRows(rows) {
        if (rows.length > 0) {
            var that = this,
                css = this.options.css,
                tpl = this.options.templates,
                selector = getCssSelector(this.options.css.inner),
                inner = this.element.children(selector).first(),
                actionThumbnailItem = tpl.actionThumbnailItem.resolve(getParams.call(this)),
                content = {
                    name: "",
                    description: "",
                    image: "",
                },
                //allRowsSelected = true,
                html = "";

            if (this.options.create) {
                var thumbnail = tpl.thumbnailCreate.resolve(getParams.call(this, {
                        icon: tpl.icon.resolve(getParams.call(this, { iconCss: css.iconCreate })),
                        text: this.options.labels.create
                    }));

                html += tpl.item.resolve(getParams.call(this,
                    {
                        attr: " class=\"" + this.options.css.itemCreate + "\"",
                        content: $("<div></div>").html(thumbnail).html()
                    }))
            }

            $.each(rows, function (index, row) {
                var wrapper = $("<div></div>"),
                    thumbnail = $(tpl.thumbnail.resolve(getParams.call(that))),
                    caption = $(tpl.caption.resolve(getParams.call(that))),
                    itemAttr = " data-item-id=\"" + ((that.identifier === null) ? index : row[that.identifier]) + "\"",
                    itemCss = that.options.css.item;

                //if (that.selection) {
                //    var selected = ($.inArray(row[that.identifier], that.selectedRows) !== -1),
                //        selectBox = tpl.select.resolve(getParams.call(that,
                //            { type: "checkbox", value: row[that.identifier], checked: selected }));
                //    cells += tpl.cell.resolve(getParams.call(that, { content: selectBox, css: css.selectCell }));
                //    allRowsSelected = (allRowsSelected && selected);
                //    if (selected) {
                //        itemCss += css.selected;
                //        itemAttr += " aria-selected=\"true\"";
                //    }
                //}

                //var status = row.status != null && that.options.statusMapping[row.status];
                //if (status) {
                //    itemCss += status;
                //}

                $.each(that.columns, function (j, column) {
                    if (typeof content[column.key]) {
                        content[column.key] = ($.isFunction(column.formatter))
                            ? column.formatter.call(that, column, row)
                            : column.converter.to(row[column.id]);
                    }
                });

                caption.append([
                    $(tpl.name.resolve(getParams.call(that, { content: content.name }))),
                    $(tpl.description.resolve(getParams.call(that, { content: content.description })))
                ]);

                if (that.options.actionThumbnail) {
                    thumbnail.append(tpl.actionThumbnail.resolve(getParams.call(that, { attr: itemAttr, content: actionThumbnailItem })));
                }

                thumbnail.append([
                    $(tpl.image.resolve(getParams.call(that, { src: content.image }))),
                    caption
                ])._bgClickableAria(that.options.clickable);

                if (itemCss.length > 0) {
                    itemAttr += " class=\"" + itemCss + "\"";
                }

                wrapper.html(thumbnail);

                html += tpl.item.resolve(getParams.call(that, { attr: itemAttr, content: wrapper.html() }));
            });

            // sets or clears multi selectbox state
            //that.element.find("thead " + getCssSelector(that.options.css.selectBox))
            //    .prop("checked", allRowsSelected);

            inner.html(html);

            registerRowEvents.call(this, inner);
        }
        else {
            renderNoResultsRow.call(this);
        }
    }

    function registerRowEvents(inner) {
        var that = this,
            selector = getCssSelector(this.options.css.thumbnail);
            //selectBoxSelector = getCssSelector(this.options.css.selectBox);

        //if (this.selection) {
        //    inner.off("click" + namespace, selectBoxSelector)
        //        .on("click" + namespace, selectBoxSelector, function (e) {
        //            e.stopPropagation();

        //            var $this = $(this),
        //                id = that.converter.from($this.val());

        //            if ($this.prop("checked")) {
        //                that.select([id]);
        //            }
        //            else {
        //                that.deselect([id]);
        //            }
        //        });
        //}

        inner.off("click" + namespace, selector)
            .on("click" + namespace, selector, function (e) {
                e.stopPropagation();

                var $this = $(this),
                    id = (that.identifier === null) ? $this.data("item-id") :
                        that.converter.from($this.data("item-id") + ""),
                    row = (that.identifier === null) ? that.currentRows[id] :
                        that.currentRows.first(function (item) { return item[that.identifier] === id; });

                if (that.selection && that.options.rowSelect) {
                    if ($this.hasClass(that.options.css.selected)) {
                        that.deselect([id]);
                    }
                    else {
                        that.select([id]);
                    }
                }

                that.element.trigger("click" + namespace, [that.columns, row]);
            });
    }

    function renderSearchField() {
        if (this.options.navigation !== 0) {
            var css = this.options.css,
                selector = getCssSelector(css.search),
                searchItems = findFooterAndHeaderItems.call(this, selector);

            if (searchItems.length > 0) {
                var that = this,
                    tpl = this.options.templates,
                    timer = null, // fast keyup detection
                    currentValue = "",
                    searchFieldSelector = getCssSelector(css.searchField),
                    search = $(tpl.search.resolve(getParams.call(this))),
                    searchField = (search.is(searchFieldSelector)) ? search :
                        search.find(searchFieldSelector);

                searchField.on("keyup" + namespace, function (e) {
                    e.stopPropagation();
                    var newValue = $(this).val();
                    if (currentValue !== newValue || (e.which === 13 && newValue !== "")) {
                        currentValue = newValue;
                        if (e.which === 13 || newValue.length === 0 || newValue.length >= that.options.searchSettings.characters) {
                            window.clearTimeout(timer);
                            timer = window.setTimeout(function () {
                                executeSearch.call(that, newValue);
                            }, that.options.searchSettings.delay);
                        }
                    }
                });

                replacePlaceHolder.call(this, searchItems, search);
            }
        }
    }

    function executeSearch(phrase) {
        if (this.searchPhrase !== phrase) {
            this.current = 1;
            this.searchPhrase = phrase;
            loadData.call(this);
        }
    }

    function replacePlaceHolder(placeholder, element) {
        placeholder.each(function (index, item) {
            // todo: check how append is implemented. Perhaps cloning here is superfluous.
            $(item).before(element.clone(true)).remove();
        });
    }

    function sortRows() {
        var sortArray = [];

        function sort(x, y, current) {
            current = current || 0;
            var next = current + 1,
                item = sortArray[current];

            function sortOrder(value) {
                return (item.order === "asc") ? value : value * -1;
            }

            return (x[item.id] > y[item.id]) ? sortOrder(1) :
                (x[item.id] < y[item.id]) ? sortOrder(-1) :
                    (sortArray.length > next) ? sort(x, y, next) : 0;
        }

        if (!this.options.ajax) {
            var that = this;

            for (var key in this.sortDictionary) {
                if (this.options.multiSort || sortArray.length === 0) {
                    sortArray.push({
                        id: key,
                        order: this.sortDictionary[key]
                    });
                }
            }

            if (sortArray.length > 0) {
                this.rows.sort(sort);
            }
        }
    }

    function showLoading() {
        var that = this;

        window.setTimeout(function () {
            if (that.element._bgAria("busy") === "true") {
                var tpl = that.options.templates,
                    selector = getCssSelector(that.options.css.inner),
                    inner = that.element.children(selector).first();

                inner.html(tpl.loading.resolve(getParams.call(that)));
            }
        }, 250);
    }


    // THUMBNAIL GRID PUBLIC CLASS DEFINITION
    // ====================

    /**
     * Represents the Kerry Express jQuery Thumbnail Grid plugin.
     *
     * @class ThumbnailGrid
     * @constructor
     * @param element {Object} The corresponding DOM element.
     * @param options {Object} The options to override default settings.
     * @chainable
     **/
    var ThumbnailGrid = function (element, options) {
        this.element = $(element);
        this.origin = this.element.clone();
        this.options = $.extend(true, {}, ThumbnailGrid.defaults, this.element.data(), options);
        // overrides rowCount explicitly because deep copy ($.extend) leads to strange behaviour
        var rowCount = this.options.rowCount = this.element.data().rowCount || options.rowCount || this.options.rowCount;
        this.columns = [];
        this.current = 1;
        this.currentRows = [];
        this.identifier = null; // The first column ID that is marked as identifier
        this.selection = false;
        this.converter = null; // The converter for the column that is marked as identifier
        this.rowCount = ($.isArray(rowCount)) ? rowCount[0] : rowCount;
        this.rows = [];
        this.searchPhrase = "";
        this.selectedRows = [];
        this.sortDictionary = {};
        this.total = 0;
        this.totalPages = 0;
        this.cachedParams = {
            lbl: this.options.labels,
            css: this.options.css,
            ctx: {}
        };
        this.header = null;
        this.footer = null;
        this.xqr = null;

        // todo: implement cache
    };

    /**
     * An object that represents the default settings.
     *
     * @static
     * @class defaults
     * @for ThumbnailGrid
     * @example
     *   // Global approach
     *   $.thumbnailgrid.defaults.selection = true;
     * @example
     *   // Initialization approach
     *   $("#thumbnailgrid").thumbnailgrid({ selection = true });
     **/
    ThumbnailGrid.defaults = {
        mode: 2, // it's a flag: 0 = infinite scrolling, 1 = button load more, 2 = pagination
        navigation: 3, // it's a flag: 0 = none, 1 = top, 2 = bottom, 3 = both (top and bottom)
        padding: 2, // page padding (pagination)
        columnSelection: true,
        rowCount: [12, 24, 48, 72, -1], // rows per page int or array of int (-1 represents "All")
        switchview: true, // Switch view between the  jQuery Bootgrid and Kerry Express jQuery Thumbnail Grid
        actionThumbnail: false, // Show Action for Item
        create: false,

        /**
         * Enables row selection (to enable multi selection see also `multiSelect`). Default value is `false`.
         *
         * @property selection
         * @type Boolean
         * @default false
         * @for defaults
         **/
        selection: false,

        /**
         * Enables multi selection (`selection` must be set to `true` as well). Default value is `false`.
         *
         * @property multiSelect
         * @type Boolean
         * @default false
         * @for defaults
         **/
        multiSelect: false,

        /**
         * Enables entire row click selection (`selection` must be set to `true` as well). Default value is `false`.
         *
         * @property rowSelect
         * @type Boolean
         * @default false
         * @for defaults
         **/
        rowSelect: false,

        /**
         * Defines whether the row selection is saved internally on filtering, paging and sorting
         * (even if the selected rows are not visible).
         *
         * @property keepSelection
         * @type Boolean
         * @default false
         * @for defaults
         **/
        keepSelection: false,

        highlightRows: false, // highlights new rows (find the page of the first new row)
        sorting: true,
        multiSort: false,

        /**
         * General search settings to configure the search field behaviour.
         *
         * @property searchSettings
         * @type Object
         * @for defaults
         **/
        searchSettings: {
            /**
             * The time in milliseconds to wait before search gets executed.
             *
             * @property delay
             * @type Number
             * @default 250
             * @for searchSettings
             **/
            delay: 250,

            /**
             * The characters to type before the search gets executed.
             *
             * @property characters
             * @type Number
             * @default 1
             * @for searchSettings
             **/
            characters: 1
        },

        /**
         * Defines whether the data shall be loaded via an asynchronous HTTP (Ajax) request.
         *
         * @property ajax
         * @type Boolean
         * @default false
         * @for defaults
         **/
        ajax: false,

        /**
         * Ajax request settings that shall be used for server-side communication.
         * All setting except data, error, success and url can be overridden.
         * For the full list of settings go to http://api.jquery.com/jQuery.ajax/.
         *
         * @property ajaxSettings
         * @type Object
         * @for defaults
         **/
        ajaxSettings: {
            /**
             * Specifies the HTTP method which shall be used when sending data to the server.
             * Go to http://api.jquery.com/jQuery.ajax/ for more details.
             * This setting is overriden for backward compatibility.
             *
             * @property method
             * @type String
             * @default "POST"
             * @for ajaxSettings
             **/
            method: "POST"
        },

        /**
         * Enriches the request object with additional properties. Either a `PlainObject` or a `Function`
         * that returns a `PlainObject` can be passed. Default value is `{}`.
         *
         * @property post
         * @type Object|Function
         * @default function (request) { return request; }
         * @for defaults
         * @deprecated Use instead `requestHandler`
         **/
        post: {}, // or use function () { return {}; } (reserved properties are "current", "rowCount", "sort" and "searchPhrase")

        /**
         * Sets the data URL to a data service (e.g. a REST service). Either a `String` or a `Function`
         * that returns a `String` can be passed. Default value is `""`.
         *
         * @property url
         * @type String|Function
         * @default ""
         * @for defaults
         **/
        url: "", // or use function () { return ""; }

        /**
         * Defines whether the search is case sensitive or insensitive.
         *
         * @property caseSensitive
         * @type Boolean
         * @default true
         * @for defaults
         **/
        caseSensitive: true,

        // note: The following properties should not be used via data-api attributes

        /**
         * Transforms the JSON request object in what ever is needed on the server-side implementation.
         *
         * @property requestHandler
         * @type Function
         * @default function (request) { return request; }
         * @for defaults
         **/
        requestHandler: function (request) { return request; },

        /**
         * Transforms the response object into the expected JSON response object.
         *
         * @property responseHandler
         * @type Function
         * @default function (response) { return response; }
         * @for defaults
         **/
        responseHandler: function (response) { return response; },

        /**
         * A list of converters.
         *
         * @property converters
         * @type Object
         * @for defaults
         **/
        converters: {
            numeric: {
                from: function (value) { return +value; }, // converts from string to numeric
                to: function (value) { return value + ""; } // converts from numeric to string
            },
            string: {
                // default converter
                from: function (value) { return value; },
                to: function (value) { return value === null ? "" : value; }
            }
        },

        /**
         * Columns of Thumbnail.
         *
         * @property columns
         * @type Object
         * @for defaults
         **/
        columns: {
            id: {
                key: "id",
                identifier: true,
                converter: "string",
                formatter: null,
                order: "desc",
                searchable: true,
            },
            image: {
                key: "image",
                identifier: false,
                converter: "string",
                formatter: "image",
                order: null,
                searchable: false,
            },
            name: {
                key: "name",
                identifier: false,
                converter: "string",
                formatter: null,
                order: "asc",
                searchable: true,
            },
            description: {
                key: "description",
                identifier: false,
                converter: "string",
                formatter: "description",
                order: "asc",
                searchable: true,
            },

        },

        clickable: false,

        /**
         * Set data if not use Ajax.
         *
         * @property data
         * @type array
         * @for defaults
         **/
        data: [],

        /**
         * Contains all css classes.
         *
         * @property css
         * @type Object
         * @for defaults
         **/
        css: {
            actions: "actions form-group", // must be a unique class name or constellation of class names within the header and footer
            actionThumbnail: "action",
            center: "text-center",
            create: "create",
            dropDownItem: "dropdown-item", // must be a unique class name or constellation of class names within the actionDropDown,
            dropDownItemButton: "dropdown-item-button", // must be a unique class name or constellation of class names within the actionDropDown
            dropDownMenu: "dropdown btn-group", // must be a unique class name or constellation of class names within the actionDropDown
            dropDownMenuItems: "dropdown-menu pull-right", // must be a unique class name or constellation of class names within the actionDropDown
            dropDownMenuText: "dropdown-text", // must be a unique class name or constellation of class names within the actionDropDown
            footer: "thumbnail-grid-footer",
            header: "thumbnail-grid-header form-inline",
            item: "col-sm-6 col-md-4 col-lg-3 item",
            itemCreate: "col-sm-6 col-md-4 col-lg-3 item-create",
            thumbnail: "thumbnail",
            image: "image",
            caption: "caption",
            name: "name",
            description: "description",
            itemActions: "action",
            icon: "icon",
            iconRefresh: "ion-android-refresh",
            iconSearch: "ion-ios-search-strong",
            iconCreate: "ion-plus",
            iconSwitchGrid: "ion-android-menu",
            iconSwitchThumbnailGrid: "ion-grid",
            infos: "infos", // must be a unique class name or constellation of class names within the header and footer,
            left: "text-left",
            loading: "thumbnail-grid-loading",
            inner: "thumbnail-grid-inner row",
            pagination: "pagination", // must be a unique class name or constellation of class names within the header and footer
            paginationButton: "button", // must be a unique class name or constellation of class names within the pagination
            right: "text-right",
            search: "search input-group input-group-icon", // must be a unique class name or constellation of class names within the header and footer
            searchField: "search-field form-control",
            //selectBox: "select-box", // must be a unique class name or constellation of class names within the entire table
            //selectCell: "select-cell", // must be a unique class name or constellation of class names within the entire table

            /**
             * CSS class to highlight selected rows.
             *
             * @property selected
             * @type String
             * @default "active"
             * @for css
             **/
            //selected: "active",

            thumbnailgrid: "thumbnail-grid"
        },

        /**
         * A dictionary of formatters.
         *
         * @property formatters
         * @type Object
         * @for defaults
         **/
        formatters: {
            image: function (column, row) {
                return row[column.id] === null ? "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz48c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgaGVpZ2h0PSIzMDBweCIgd2lkdGg9IjMwMHB4IiB2ZXJzaW9uPSIxLjAiIHZpZXdCb3g9Ii0zMDAgLTMwMCA2MDAgNjAwIiB4bWw6c3BhY2U9InByZXNlcnZlIj48Y2lyY2xlIHN0cm9rZT0iI0FBQSIgc3Ryb2tlLXdpZHRoPSIxMCIgcj0iMjgwIiBmaWxsPSIjRkZGIi8+PHRleHQgc3R5bGU9ImxldHRlci1zcGFjaW5nOjE7dGV4dC1hbmNob3I6bWlkZGxlO3RleHQtYWxpZ246Y2VudGVyO3N0cm9rZS1vcGFjaXR5Oi41O3N0cm9rZTojMDAwO3N0cm9rZS13aWR0aDoyO2ZpbGw6IzQ0NDtmb250LXNpemU6MzYwcHg7Zm9udC1mYW1pbHk6Qml0c3RyZWFtIFZlcmEgU2FucyxMaWJlcmF0aW9uIFNhbnMsIEFyaWFsLCBzYW5zLXNlcmlmO2xpbmUtaGVpZ2h0OjEyNSU7d3JpdGluZy1tb2RlOmxyLXRiOyIgdHJhbnNmb3JtPSJzY2FsZSguMikiPjx0c3BhbiB5PSItNDAiIHg9IjgiPk5PIElNQUdFPC90c3Bhbj48dHNwYW4geT0iNDAwIiB4PSI4Ij5BVkFJTEFCTEU8L3RzcGFuPjwvdGV4dD48L3N2Zz4=" : row[column.id];
            },
            description: function (column, row) {
                return row[column.id] === null ? "<em class=\"text-muted\">No description</em>" : row[column.id];
            },
        },

        /**
         * Contains all labels.
         *
         * @property labels
         * @type Object
         * @for defaults
         **/
        labels: {
            all: "All",
            infos: "Showing {{ctx.start}} to {{ctx.end}} of {{ctx.total}} entries",
            noResults: "No results found!",
            refresh: "Refresh",
            search: "Search",
            create: "Create new",
            gridView: "Grid View",
            thumbnailGridView: "Thumbnail View"
        },

        /**
         * Specifies the mapping between status and contextual classes to color rows.
         *
         * @property statusMapping
         * @type Object
         * @for defaults
         **/
        statusMapping: {
            /**
             * Specifies a successful or positive action.
             *
             * @property 0
             * @type String
             * @for statusMapping
             **/
            0: "success",

            /**
             * Specifies a neutral informative change or action.
             *
             * @property 1
             * @type String
             * @for statusMapping
             **/
            1: "info",

            /**
             * Specifies a warning that might need attention.
             *
             * @property 2
             * @type String
             * @for statusMapping
             **/
            2: "warning",

            /**
             * Specifies a dangerous or potentially negative action.
             *
             * @property 3
             * @type String
             * @for statusMapping
             **/
            3: "danger"
        },

        /**
         * Contains all templates.
         *
         * @property templates
         * @type Object
         * @for defaults
         **/
        templates: {
            actionButton: "<button class=\"btn btn-icon btn-default\" type=\"button\" title=\"{{ctx.text}}\">{{ctx.content}}</button>",
            actionButtonActive: "<button class=\"btn btn-icon btn-outline-primary active\" type=\"button\" title=\"{{ctx.text}}\">{{ctx.content}}</button>",
            actionButtonCreate: "<button class=\"btn btn-primary\" data-command=\"create\">{{lbl.create}}</button>",
            actionDropDown: "<div class=\"{{css.dropDownMenu}}\"><button class=\"btn btn-default dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\"><span class=\"{{css.dropDownMenuText}}\">{{ctx.content}}</span> <span class=\"caret\"></span></button><ul class=\"{{css.dropDownMenuItems}}\" role=\"menu\"></ul></div>",
            actionDropDownItem: "<li><a data-action=\"{{ctx.action}}\" class=\"{{css.dropDownItem}} {{css.dropDownItemButton}}\">{{ctx.text}}</a></li>",
            actions: "<div class=\"{{css.actions}}\"></div>",
            actionThumbnail: "<div class=\"{{css.actionThumbnail}}\">{{ctx.content}}</div>",
            actionThumbnailItem: "<button class=\"btn btn-icon btn-circle btn-default\"{{ctx.attr}} title=\"Edit\" data-command=\"edit\"><i class=\"ion-edit\"></i></button><button class=\"btn btn-icon btn-circle btn-default\"{{ctx.attr}} title=\"Delete\" data-command=\"delete\"><i class=\"ion-android-delete\"></i></button>",
            inner: "<div class=\"{{css.inner}}\"></div>",
            item: "<div{{ctx.attr}}>{{ctx.content}}</div>",
            thumbnail: "<div class=\"{{css.thumbnail}}\"></div>",
            thumbnailCreate: "<div class=\"{{css.thumbnail}} {{css.create}}\" data-command=\"create\"><div class=\"wrapper\"><div class=\"icon\">{{ctx.icon}}</div><div class=\"text\">{{ctx.text}}</div></div></div>",
            image: "<div class=\"{{css.image}}\"><img src=\"{{ctx.src}}\"></div>",
            caption: "<div class=\"{{css.caption}}\"></div>",
            name: "<div class=\"{{css.name}}\">{{ctx.content}}</div>",
            description: "<div class=\"{{css.description}}\">{{ctx.content}}</div>",
            itemActions: "<div class=\"{{css.itemActions}}\"></div>",
            footer: "<div id=\"{{ctx.id}}\" class=\"{{css.footer}}\"><div class=\"row\"><div class=\"col-sm-6\"><p class=\"{{css.pagination}}\"></p></div><div class=\"col-sm-6 info-bar\"><p class=\"{{css.infos}}\"></p></div></div></div>",
            header: "<div id=\"{{ctx.id}}\" class=\"{{css.header}}\"><div class=\"row\"><div class=\"col-sm-12 text-right\"><div class=\"{{css.search}}\"></div><div class=\"{{css.actions}}\"></div></div></div></div>",
            icon: "<i class=\"{{css.icon}} {{ctx.iconCss}}\"></i>",
            infos: "<div class=\"{{css.infos}}\">{{lbl.infos}}</div>",
            loading: "<div class=\"{{css.loading}}\"><div class=\"loader loader-pulse loader-sm\"></div></div>",
            noResults: "<div class=\"not-found\"><h3>{{lbl.noResults}}</h3>{{ctx.content}}</div>",
            pagination: "<ul class=\"{{css.pagination}}\"></ul>",
            paginationItem: "<li class=\"{{ctx.css}}\"><a data-page=\"{{ctx.page}}\" class=\"{{css.paginationButton}}\">{{ctx.text}}</a></li>",
            search: "<div class=\"{{css.search}}\"><span class=\"input-group-addon-icon\"><i class=\"{{css.icon}} {{css.iconSearch}}\"></i></span><input class=\"{{css.searchField}}\" placeholder=\"{{lbl.search}}\" type=\"text\"></div>",
            //select: "<input name=\"select\" type=\"{{ctx.type}}\" class=\"{{css.selectBox}}\" value=\"{{ctx.value}}\" {{ctx.checked}} />"
            switchWrapper: "<div class=\"btn-group\" role=\"group\" aria-label=\"Switch View\">",
        }
    };

    /**
     * Appends rows.
     *
     * @method append
     * @param rows {Array} An array of rows to append
     * @chainable
     **/
    ThumbnailGrid.prototype.append = function (rows) {
        if (this.options.ajax) {
            // todo: implement ajax PUT
        }
        else {
            var appendedRows = [];
            for (var i = 0; i < rows.length; i++) {
                if (appendRow.call(this, rows[i])) {
                    appendedRows.push(rows[i]);
                }
            }
            sortRows.call(this);
            highlightAppendedRows.call(this, appendedRows);
            loadData.call(this);
            this.element.trigger("appended" + namespace, [appendedRows]);
        }

        return this;
    };

    /**
     * Removes all rows.
     *
     * @method clear
     * @chainable
     **/
    ThumbnailGrid.prototype.clear = function () {
        if (this.options.ajax) {
            // todo: implement ajax POST
        }
        else {
            var removedRows = $.extend([], this.rows);
            this.rows = [];
            this.current = 1;
            this.total = 0;
            loadData.call(this);
            this.element.trigger("cleared" + namespace, [removedRows]);
        }

        return this;
    };

    /**
     * Removes the control functionality completely and transforms the current state to the initial HTML structure.
     *
     * @method destroy
     * @chainable
     **/
    ThumbnailGrid.prototype.destroy = function () {
        // todo: this method has to be optimized (the complete initial state must be restored)
        $(window).off(namespace);
        if (this.options.navigation & 1) {
            this.header.remove();
        }
        if (this.options.navigation & 2) {
            this.footer.remove();
        }
        this.element.before(this.origin).remove();

        return this;
    };

    /**
     * Resets the state and reloads rows.
     *
     * @method reload
     * @chainable
     **/
    ThumbnailGrid.prototype.reload = function () {
        this.current = 1; // reset
        loadData.call(this);

        return this;
    };

    /**
     * Removes rows by ids. Removes selected rows if no ids are provided.
     *
     * @method remove
     * @param [rowsIds] {Array} An array of rows ids to remove
     * @chainable
     **/
    ThumbnailGrid.prototype.remove = function (rowIds) {
        if (this.identifier != null) {
            var that = this;

            if (this.options.ajax) {
                // todo: implement ajax DELETE
            }
            else {
                rowIds = rowIds || this.selectedRows;
                var id,
                    removedRows = [];

                for (var i = 0; i < rowIds.length; i++) {
                    id = rowIds[i];

                    for (var j = 0; j < this.rows.length; j++) {
                        if (this.rows[j][this.identifier] === id) {
                            removedRows.push(this.rows[j]);
                            this.rows.splice(j, 1);
                            break;
                        }
                    }
                }

                this.current = 1; // reset
                loadData.call(this);
                this.element.trigger("removed" + namespace, [removedRows]);
            }
        }

        return this;
    };

    /**
     * Searches in all rows for a specific phrase (but only in visible cells). 
     * The search filter will be reseted, if no argument is provided.
     *
     * @method search
     * @param [phrase] {String} The phrase to search for
     * @chainable
     **/
    ThumbnailGrid.prototype.search = function (phrase) {
        phrase = phrase || "";

        if (this.searchPhrase !== phrase) {
            var selector = getCssSelector(this.options.css.searchField),
                searchFields = findFooterAndHeaderItems.call(this, selector);
            searchFields.val(phrase);
        }

        executeSearch.call(this, phrase);


        return this;
    };

    /**
     * Selects rows by ids. Selects all visible rows if no ids are provided.
     * In server-side scenarios only visible rows are selectable.
     *
     * @method select
     * @param [rowsIds] {Array} An array of rows ids to select
     * @chainable
     **/
    ThumbnailGrid.prototype.select = function (rowIds) {
        if (this.selection) {
            rowIds = rowIds || this.currentRows.propValues(this.identifier);

            var id, i,
                selectedRows = [];

            while (rowIds.length > 0 && !(!this.options.multiSelect && selectedRows.length === 1)) {
                id = rowIds.pop();
                if ($.inArray(id, this.selectedRows) === -1) {
                    for (i = 0; i < this.currentRows.length; i++) {
                        if (this.currentRows[i][this.identifier] === id) {
                            selectedRows.push(this.currentRows[i]);
                            this.selectedRows.push(id);
                            break;
                        }
                    }
                }
            }

            if (selectedRows.length > 0) {
                var selectBoxSelector = getCssSelector(this.options.css.selectBox),
                    selectMultiSelectBox = this.selectedRows.length >= this.currentRows.length;

                i = 0;
                while (!this.options.keepSelection && selectMultiSelectBox && i < this.currentRows.length) {
                    selectMultiSelectBox = ($.inArray(this.currentRows[i++][this.identifier], this.selectedRows) !== -1);
                }
                this.element.find("thead " + selectBoxSelector).prop("checked", selectMultiSelectBox);

                if (!this.options.multiSelect) {
                    this.element.find("tbody > tr " + selectBoxSelector + ":checked")
                        .trigger("click" + namespace);
                }

                for (i = 0; i < this.selectedRows.length; i++) {
                    this.element.find("tbody > tr[data-item-id=\"" + this.selectedRows[i] + "\"]")
                        .addClass(this.options.css.selected)._bgAria("selected", "true")
                        .find(selectBoxSelector).prop("checked", true);
                }

                this.element.trigger("selected" + namespace, [selectedRows]);
            }
        }

        return this;
    };

    /**
     * Deselects rows by ids. Deselects all visible rows if no ids are provided.
     * In server-side scenarios only visible rows are deselectable.
     *
     * @method deselect
     * @param [rowsIds] {Array} An array of rows ids to deselect
     * @chainable
     **/
    ThumbnailGrid.prototype.deselect = function (rowIds) {
        if (this.selection) {
            rowIds = rowIds || this.currentRows.propValues(this.identifier);

            var id, i, pos,
                deselectedRows = [];

            while (rowIds.length > 0) {
                id = rowIds.pop();
                pos = $.inArray(id, this.selectedRows);
                if (pos !== -1) {
                    for (i = 0; i < this.currentRows.length; i++) {
                        if (this.currentRows[i][this.identifier] === id) {
                            deselectedRows.push(this.currentRows[i]);
                            this.selectedRows.splice(pos, 1);
                            break;
                        }
                    }
                }
            }

            if (deselectedRows.length > 0) {
                var selectBoxSelector = getCssSelector(this.options.css.selectBox);

                this.element.find("thead " + selectBoxSelector).prop("checked", false);
                for (i = 0; i < deselectedRows.length; i++) {
                    this.element.find("tbody > tr[data-item-id=\"" + deselectedRows[i][this.identifier] + "\"]")
                        .removeClass(this.options.css.selected)._bgAria("selected", "false")
                        .find(selectBoxSelector).prop("checked", false);
                }

                this.element.trigger("deselected" + namespace, [deselectedRows]);
            }
        }

        return this;
    };

    /**
     * Sorts the rows by a given sort descriptor dictionary. 
     * The sort filter will be reseted, if no argument is provided.
     *
     * @method sort
     * @param [dictionary] {Object} A sort descriptor dictionary that contains the sort information
     * @chainable
     **/
    ThumbnailGrid.prototype.sort = function (dictionary) {
        var values = (dictionary) ? $.extend({}, dictionary) : {};

        if (values === this.sortDictionary) {
            return this;
        }

        this.sortDictionary = values;
        renderTableHeader.call(this);
        sortRows.call(this);
        loadData.call(this);

        return this;
    };

    /**
     * Gets a list of the column settings.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getColumnSettings
     * @return {Array} Returns a list of the column settings.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getColumnSettings = function () {
        return $.merge([], this.columns);
    };

    /**
     * Gets the current page index.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getCurrentPage
     * @return {Number} Returns the current page index.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getCurrentPage = function () {
        return this.current;
    };

    /**
     * Gets the current rows.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getCurrentPage
     * @return {Array} Returns the current rows.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getCurrentRows = function () {
        return $.merge([], this.currentRows);
    };

    /**
     * Gets a number represents the row count per page.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getRowCount
     * @return {Number} Returns the row count per page.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getRowCount = function () {
        return this.rowCount;
    };

    /**
     * Gets the actual search phrase.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getSearchPhrase
     * @return {String} Returns the actual search phrase.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getSearchPhrase = function () {
        return this.searchPhrase;
    };

    /**
     * Gets the complete list of currently selected rows.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getSelectedRows
     * @return {Array} Returns all selected rows.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getSelectedRows = function () {
        return $.merge([], this.selectedRows);
    };

    /**
     * Gets the sort dictionary which represents the state of column sorting.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getSortDictionary
     * @return {Object} Returns the sort dictionary.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getSortDictionary = function () {
        return $.extend({}, this.sortDictionary);
    };

    /**
     * Gets a number represents the total page count.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getTotalPageCount
     * @return {Number} Returns the total page count.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getTotalPageCount = function () {
        return this.totalPages;
    };

    /**
     * Gets a number represents the total row count.
     * This method returns only for the first grid instance a value.
     * Therefore be sure that only one grid instance is catched by your selector.
     *
     * @method getTotalRowCount
     * @return {Number} Returns the total row count.
     * @since 1.2.0
     **/
    ThumbnailGrid.prototype.getTotalRowCount = function () {
        return this.total;
    };


    // THUMBNAIL GRID COMMON TYPE EXTENSIONS
    // ============

    $.fn.extend({
        _bgAria: function (name, value) {
            return (value) ? this.attr("aria-" + name, value) : this.attr("aria-" + name);
        },

        _bgBusyAria: function (busy) {
            return (busy === null || busy) ?
                this._bgAria("busy", "true") :
                this._bgAria("busy", "false");
        },

        _bgRemoveAria: function (name) {
            return this.removeAttr("aria-" + name);
        },

        _bgClickableAria: function (enable) {
            return enable ?
                this.addClass("link")._bgAria("clickable", "true") :
                this.removeClass("link")._bgAria("clickable", "false");
        },

        _bgEnableAria: function (enable) {
            return (enable == null || enable) ?
                this.removeClass("disabled")._bgAria("disabled", "false") :
                this.addClass("disabled")._bgAria("disabled", "true");
        },

        _bgEnableField: function (enable) {
            return (enable == null || enable) ?
                this.removeAttr("disabled") :
                this.attr("disabled", "disable");
        },

        _bgShowAria: function (show) {
            return (show === null || show) ?
                this.show()._bgAria("hidden", "false") :
                this.hide()._bgAria("hidden", "true");
        },

        _bgSelectAria: function (select) {
            return (select === null || select) ?
                this.addClass("active")._bgAria("selected", "true") :
                this.removeClass("active")._bgAria("selected", "false");
        },

        _bgId: function (id) {
            return (id) ? this.attr("id", id) : this.attr("id");
        }
    });

    if (!String.prototype.resolve) {
        var formatter = {
            "checked": function (value) {
                if (typeof value === "boolean") {
                    return (value) ? "checked=\"checked\"" : "";
                }
                return value;
            }
        };

        String.prototype.resolve = function (substitutes, prefixes) {
            var result = this;
            $.each(substitutes, function (key, value) {
                if (value != null && typeof value !== "function") {
                    if (typeof value === "object") {
                        var keys = (prefixes) ? $.extend([], prefixes) : [];
                        keys.push(key);
                        result = result.resolve(value, keys) + "";
                    }
                    else {
                        if (formatter && formatter[key] && typeof formatter[key] === "function") {
                            value = formatter[key](value);
                        }
                        key = (prefixes) ? prefixes.join(".") + "." + key : key;
                        var pattern = new RegExp("\\{\\{" + key + "\\}\\}", "gm");
                        result = result.replace(pattern, (value.replace) ? value.replace(/\$/gi, "&#36;") : value);
                    }
                }
            });
            return result;
        };
    }

    if (!Array.prototype.contains) {
        Array.prototype.contains = function (condition) {
            for (var i = 0; i < this.length; i++) {
                var item = this[i];
                if (condition(item)) {
                    return true;
                }
            }
            return false;
        };
    }

    if (!Array.prototype.page) {
        Array.prototype.page = function (page, size) {
            var skip = (page - 1) * size,
                end = skip + size;
            return (this.length > skip) ?
                (this.length > end) ? this.slice(skip, end) :
                    this.slice(skip) : [];
        };
    }

    if (!Array.prototype.where) {
        Array.prototype.where = function (condition) {
            var result = [];
            for (var i = 0; i < this.length; i++) {
                var item = this[i];
                if (condition(item)) {
                    result.push(item);
                }
            }
            return result;
        };
    }


    // THUMBNAIL GRID PLUGIN DEFINITION
    // =====================

    var old = $.fn.thumbnailgrid;

    $.fn.thumbnailgrid = function (option) {
        var args = Array.prototype.slice.call(arguments, 1),
            returnValue = null,
            elements = this.each(function (index) {
                var $this = $(this),
                    instance = $this.data(namespace),
                    options = typeof option === "object" && option;

                if (!instance && option === "destroy") {
                    return;
                }
                if (!instance) {
                    $this.data(namespace, (instance = new ThumbnailGrid(this, options)));
                    init.call(instance);
                }
                if (typeof option === "string") {
                    if (option.indexOf("get") === 0 && index === 0) {
                        returnValue = instance[option].apply(instance, args);
                    }
                    else if (option.indexOf("get") !== 0) {
                        return instance[option].apply(instance, args);
                    }
                }
            });
        return (typeof option === "string" && option.indexOf("get") === 0) ? returnValue : elements;
    };

    $.fn.thumbnailgrid.Constructor = ThumbnailGrid;

    // THUMBNAIL GRID NO CONFLICT
    // ===============

    $.fn.thumbnailgrid.noConflict = function () {
        $.fn.thumbnailgrid = old;
        return this;
    };

    // THUMBNAIL GRID DATA-API
    // ============
    $("[data-toggle=\"thumbnailgrid\"]").thumbnailgrid();
})(jQuery, window);