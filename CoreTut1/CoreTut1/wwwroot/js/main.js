/// <reference path="../../lib/toastr/build/toastr.js" />
/// <reference path="helper.js" />
/// <reference path="toolbar.js" />

var rowIds = [], searchArr = [], sortArr = [],
    sortBy = "ID", sortWay = "DESC",
    status = false, d = new Date(), month = d.getMonth() + 1, day = d.getDate(),
    output = (day < 10 ? '0' : '') + day + '.' + (month < 10 ? '0' : '') + month + '.' + d.getFullYear(),
    menu_id = 0,
    selectizeTimer, // selectize typing icin timer
    selectizeInterval = 1000;  // kullanici 1 sn icinde tekrar yazmazsa sorgu gonderilebilir

bootbox.setDefaults({ locale: "tr" });
sortArr.push({ SortColumn: "ID", SortWay: "DESC" });

$(function () {
    clearSelected();
    BindICheck();
    ListSearchType();

    $("body .inputdouble").numericOnly();
    $("body .intOnly").intOnly();

    if ($(".date input").val() == "") {
        $(".date input").val(output);
    }

    $(".date").datetimepicker({ format: "DD.MM.YYYY", locale: "tr" });
    $(".datetime").datetimepicker({ format: "HH:mm", locale: "tr" });

    if ($(".datetime input").val() == "") {
        $(".datetime input").val(d.getHours() + ':' + d.getMinutes());
    }

    $(".list-records").on("ifChanged", ".check-all", function () {
        rowIds.length = 0;

        var $th = $(this);

        if ($(this).is(":checked")) {
            $(this).closest("table").find(".primary-check").each(function () {
                var chkBox = $(this);

                if (chkBox.val() != "" && chkBox.val() != "on") {
                    arrayExist(rowIds, $(this).val());
                }

                if ($th.is(":checked")) {
                    $(this).iCheck("check");
                } else {
                    $(this).iCheck("uncheck");
                }
            });
        } else {
            $(this).closest("table").find(".primary-check").each(function () {
                $(this).iCheck("uncheck");
            });
        }
    });

    // table row click for edit/delete etc...
    $(".list-records").on("click", "table tbody tr td:not(:first-child, .notclick)", function (e) {
        e.preventDefault();

        var $th = $(this);
        //var item = $th.closest("tr").find("td:first").find("input:checkbox");

        Fill($th);
    });

    $(".list-records").on("ifClicked", "input:checkbox.primary-check", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var $th = $(this);
        //var item = $th.closest("tr").find("td:first").find("input:checkbox");

        Fill($(this));
    });

    // paging functions
    $(".list-records").on("click", ".pagination li a", function (e) {
        e.preventDefault();
        clearSelected();

        if (!$(this).parent().hasClass("disabled")) {
            search($(this));
        }
    });

    $(".list-records").on("keyup", ".search-frm", function (e) {
        e.preventDefault();
        var $th = $(this);
        var keycode = (e.keyCode ? e.keyCode : e.which);

        if ($th.val().length > 0) {
            $th.parent().find(".clear-search-content").removeClass("muted").removeClass("pe-disabled");
        } else {
            $th.parent().find(".clear-search-content").addClass("muted").addClass("pe-enabled");
        }

        if (keycode == "13") {
            search($th);
        }
    });

    $(".list-records").on("click", "span.clear-search-content", function (e) {
        e.preventDefault();
        var $th = $(this);

        $th.parent().find(".search-frm").val("");
        $th.parent().find(".clear-search-content").addClass("muted").addClass("pe-enabled");
        search($th.parent().find(".search-frm"));
    });

    $(".list-records").on("click", "i.sort", function (e) {
        e.preventDefault();
        var $th = $(this);

        search($th.parent().parent());
    });

    $(".list-records").on("click", "a.removeSorting", function (e) {
        e.preventDefault();
        var $th = $(this);
        var data_sort_column = $th.attr("data-sort-column");
        var index = -1;

        for (var i = 0; i < sortArr.length; i++) {
            if (sortArr[i].SortColumn == data_sort_column) {
                index = i;
            }
        }

        if (i > -1) {
            sortArr.splice(index, 1);
        }

        search($th);
    });

    $(".list-records").on("click", "a.removeAllSorting", function (e) {
        e.preventDefault();
        var $th = $(this);

        sortArr.length = 0;
        search($th);
    });

    if ($(".gototop").length) {
        var scrollTrigger = 100,
            backToTop = function () {
                var scrollTop = $(window).scrollTop();

                if (scrollTop > scrollTrigger) {
                    $(".gototop").addClass('show');
                } else {
                    $(".gototop").removeClass('show');
                }
            };

        backToTop();

        $(window).on('scroll', function () {
            backToTop();
        });

        $(".gototop").on('click', function (e) {
            e.preventDefault();

            $('html,body').animate({
                scrollTop: 0
            }, 700);
        });
    }
});

function clearPrimaryCheckboxes() {
    $.each($(".primary-check"), function (i) {
        $(this).iCheck("uncheck");
    });
}

function arrayExist(array, id) {
    var found = $.inArray(id, array);

    if (found >= 0) {
        array.splice(found, 1);
    } else {
        array.push(id);
    }
}

function Fill(x) {
    var $th = x;
    var item = $th.closest("tr").find("td:first").find("input:checkbox");

    if (item.is(":checked")) {
        item.iCheck("uncheck");
    } else {
        item.iCheck("check");
    }

    if (item.val() != "" && item.val() != "on") {
        arrayExist(rowIds, item.val());
    } else if (typeof item.attr("data-id") != "undefined" && item.attr("data-id") != "") {
        arrayExist(rowIds, item.attr("data-id"));
    }

    checkToolbarItems();
}

function BindICheck() {
    $("body .i-checks").iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
    });
}

function ListSearchType() {
    $("body .doubleOnly").numericOnly();
    $("body .intOnly").intOnly();
    $("body .dateOnly").datetimepicker({ format: "DD.MM.YYYY", locale: "tr", useCurrent: false });
    $("body .timeOnly").datetimepicker({ format: "HH:mm", locale: "tr" });

    $("body .dateOnly").on("dp.change", function (e) {
        if ($(this).val() != "") {
            $(this).parent().find(".clear-search-content").removeClass("muted").removeClass("pe-disabled");
        } else {
            $(this).parent().find(".clear-search-content").addClass("muted").addClass("pe-enabled");
        }
    });

    $("body .dateOnly").keydown(function (e) {
        var $th = $(this);
        var keycode = (e.keyCode ? e.keyCode : e.which);

        if ($th.val().length > 0) {
            $th.parent().find(".clear-search-content").removeClass("muted").removeClass("pe-disabled");
        } else {
            $th.parent().find(".clear-search-content").addClass("muted").addClass("pe-enabled");
        }

        if (keycode == "13") {
            search($th);
        }
    });
}

function search(item) {
    var $th = item,
        url = $th.data("href"),
        page = 1,
        clsName = "table",
        searchForms = $th.closest(clsName).find(".search-frm");

    searchArr = [];
    loadingPanel($th, true);

    if ($th.data("page")) {
        page = $th.data("page");
    }

    if ($(".search-container").length) {
        clsName = ".search-container";
    }

    $.each(searchForms, function (i) {
        var searchParams = {};

        if ($(this).val() != "") {
            searchParams.isChildCollection = $(this).data("ischildcollection");
            searchParams.DataColumnType = $(this).data("column-type");
            searchParams.DataColumn = $(this).data("column");
            searchParams.DataType = $(this).data("type");
            searchParams.DataEnumType = $(this).data("enum-type")
            searchParams.DataValue = $(this).val().toLower();
            searchArr.push(searchParams);
        }
    });

    if (typeof (GenerateSearchArr) != "undefined") {
        GenerateSearchArr();
    }

    if (item.hasClass("removeSorting")) {
        var index = -1;

        for (var i = 0; i < sortArr.length; i++) {
            if (sortArr[i].SortColumn == $th.data("sort-column")) {
                index = i;
            }
        }

        if (index > -1) {
            sortArr.splice(index, 1);
        }
    } else {
        if ($th.data("sort-column")) {
            var sortObj = {};

            sortObj.SortColumn = $th.data("sort-column");
            sortObj.SortWay = $th.data("sort-way");
            sortArr.addTo(sortObj);
        }
    }

    setTimeout(function () {
        $.post(url, { Page: page, SearchParams: searchArr, SortParams: sortArr }, function (result) {
            $(".list-records").empty().html(result);
            BindICheck();
            loadingPanel($th, false);
            ListSearchType();

            if (typeof (setSearchParameters) != "undefined") {
                setSearchParameters(searchArr);

                $("body .date").datetimepicker({ format: "DD.MM.YYYY", locale: "tr" });
            }
        });
    }, 750);
}

function loadingPanel(item, d) {
    var _table = item.closest("table.list").parent(),
        width = _table.width(),
        height = _table.height() + 50,
        loadingPanel = "<tr id='tbLoadingPanel'><td colspan='100%'><div style='position:absolute;top:0;left:0;background-color:#fff;opacity:0.5;z-index:9998;width:100%;height:" + height + "px;'>" +
            "<div style='position:relative;width:100%;top:50%;opacity:1;z-index:9999;'><img class='img-responsive center-block text-center' src='/img/preloader.gif' alt='...' /></div>" +
            "</div></td></tr>";

    if (d) {
        _table.find("tbody").prepend(loadingPanel);
    } else {
        _table.find("tbody #tbLoadingPanel").remove();
    }
}

function showToastr(type, title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "showDuration": "500",
        "hideDuration": "2000",
        "timeOut": "4500",
        "extendedTimeOut": "2000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        "onHidden": function () {
            toastr.remove();
            toastr.clear();
        }
    };

    toastr[type](title, message);
}

function PageState(pageState) {
    if (pageState != "") {
        if (pageState == 1) {
            showToastr("success", "", PageStatus_Update);
        } else if (pageState == 2) {
            showToastr("error", "", PageStatus_NotUpdate);
        } else if (pageState == 3) {
            showToastr("success", "", PageStatus_Create);
        } else if (pageState == 4) {
            showToastr("error", "", PageStatus_NotCreate);
        } else if (pageState == 5) {
            showToastr("error", "", PageStatus_Deleted);
        } else if (pageState == 6) {
            showToastr("error", "", PageStatus_NotDeleted);
        } else if (pageState == 7) {
            showToastr("error", "", PageStatus_NotCompare);
        } else {
            showToastr("error", pageState);
        }
    }
}

// x: array
// y: aranacak sey
function searchInObjectArray(x, y) {
    var len = x.length;

    for (var i = 0; i < len; i++) {
        var keys = Object.keys(x[i]);

        for (var j = 0; j < keys.length; j++) {
            if (x[i][keys[j]] == y) {
                return i;
            }
        }
    }

    return false;
}

// x: array
// y: aranacak sey
// w: update edilecek alan
// z: update edilecek alanin degeri
// devam edilecek
function searchInObjectList(x, y, w, z) {
    var founds = [],
        len = x.length;

    for (var i = 0; i < len; i++) {
        var keys = Object.keys(x[i]);

        for (var j = 0; j < keys.length; j++) {
            if (x[i][keys[j]].toString().indexOf(y) != -1) {
                console.log(x[i][keys[j]].toString().indexOf(y));
            }
        }
    }
}

/*
// kullanimi: array.sort(soryBy("id"))
// kullanimi: array.sort(soryBy("ad-soyad"))
// devam edilecek
function sortBy(property) {
    return function (a, b) {
        if (a[property] > b[property]) {
            return 1;
        } else if (a[property] < b[property]) {
            return -1;
        }

        return 0;
    };
}
*/

function sortBySequence(a, b, prop) {
    return function (a, b) {
        var A = a[prop];
        var B = b[prop];

        return A > B ? 1 : -1;
    };
}

function sortByText(a, b) {
    if (a.TEXT < b.TEXT)
        return -1;
    else if (a.TEXT > b.TEXT)
        return 1;
    else
        return 0;
}

function sortBySiraNo(a, b) {
    var A = a.SIRANO,
        B = b.SIRANO;

    return A > B ? 1 : -1;
};

// * Turkce karakterleri buyuk harfe cevirir
String.prototype.toUpper = function () {
    var string = this,
        letters = { "i": "İ", "ş": "Ş", "ğ": "Ğ", "ü": "Ü", "ö": "Ö", "ç": "Ç", "ı": "I" };

    string = string.replace(/(([iışğüçö]))/g, function (letter) { return letters[letter]; });

    return string.toUpperCase();
};

// * Turkce karakterleri kucuk harfe cevirir
String.prototype.toLower = function () {
    var string = this,
        letters = { "İ": "i", "I": "ı", "Ş": "ş", "Ğ": "ğ", "Ü": "ü", "Ö": "ö", "Ç": "ç" };

    string = string.replace(/(([İIŞĞÜÇÖ]))/g, function (letter) { return letters[letter]; });

    return string.toLowerCase();
};

function emailValidation(email) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    return reg.test(email);
}

Array.prototype.addTo = function (x) {
    var index = -1, exist = false;

    for (var i = 0; i < sortArr.length; i++) {
        if (JSON.stringify(sortArr[i]) === JSON.stringify({ SortColumn: "ID", SortWay: "ASC" })) {
            index = i;
        }
    }

    if (parseInt(index) >= 0) {
        sortArr.splice(index, 1);
    }

    for (var i = 0; i < sortArr.length; i++) {
        if (sortArr[i].SortColumn === x.SortColumn) {
            exist = true;
            sortArr[i].SortWay = x.SortWay;
        }
    }

    if (!exist) {
        sortArr.push(x);
    }
};

Array.prototype.clearMe = function () {
    this.length = 0;
};

Array.prototype.sortBy = function (p) {
    return this.slice(0).sort(function (a, b) {
        return (a[p] > b[p]) ? 1 : (a[p] < b[p]) ? -1 : 0;
    });
};

function checkSorting() {
    if ($("table.list")) {
        if (sortArr.length > 0) {
            var canAppend = false,
                html = "<thead><tr><td colspan='100%'><span class='sort-options label mb10 mr10'>Sıralama: <a data-href='" + $(".list_item").val() + "' class='removeAllSorting text-black'>(Temizle <i class='fa fa-remove text-black'></i>)</a></span>";

            for (var i = 0; i < sortArr.length; i++) {
                var col = sortArr[i].SortColumn;

                if (col != "ID") {
                    var $div = $("div[data-sort-column='" + col + "']");

                    canAppend = true;
                    html += "<span class='sort-options label label-primary mb10 mr10'>" + $div.text() +
                        "&nbsp;&nbsp; <a data-href='" + $div.data("href") + "' class='removeSorting' data-sort-column='" +
                        $div.attr("data-sort-column") + "'><i class='fa fa-remove'></i></a></span>";
                }
            }

            html += "</td></tr></thead>";

            if (canAppend) {
                $("table.list").prepend(html);
            }
        }
    }
}

function round(value, decimals) {
    return Number(Math.round(value + 'e' + decimals) + 'e-' + decimals);
}

function clearSelected() {
    if ($(".i-checks").length > 0 && $(".list").length > 0) {
        $(".list").find(".i-checks").iCheck("uncheck");
        rowIds.length = 0;
    }
}

function toolbarLoading() {
    if ($("#toolbar_save").length) {
        var th = $("#toolbar_save");

        th.addClass("input-pe-disabled");
        th.attr("disabled", true);
        th.find("i").removeClass("fa-floppy-o").addClass("fa-spin fa-cog");
    }
}

function toolbarLoadingDel(status) {
    if ($("#toolbar_delete").length) {
        var th = $("#toolbar_delete");

        th.addClass("input-pe-disabled");
        th.attr("disabled", true);
        th.find("i").removeClass("fa-trash-o").addClass("fa-spin fa-cog");

        if (!status) {
            th.removeClass("input-pe-disabled");
            th.attr("disabled", false);
            th.find("i").addClass("fa-trash-o").removeClass("fa-spin fa-cog");
        }
    }
}

function disableKey(e) {
    var ev = e ? e : window.event;

    if (ev) {
        if (ev.preventDefault) {
            ev.preventDefault();
        } else {
            ev.returnValue = false;
        }
    }
}

function FormatNumberBy3(num, decpoint, sep) {
    // check for missing parameters and use defaults if so
    if (arguments.length == 2) {
        sep = ",";
    }
    if (arguments.length == 1) {
        sep = ",";
        decpoint = ".";
    }
    // need a string for operations
    num = num.toString();
    // separate the whole number and the fraction if possible
    a = num.split(decpoint);
    x = a[0]; // decimal
    y = a[1]; // fraction
    z = "";

    if (typeof (x) != "undefined") {
        // reverse the digits. regexp works from left to right.
        for (i = x.length - 1; i >= 0; i--)
            z += x.charAt(i);
        // add seperators. but undo the trailing one, if there
        z = z.replace(/(\d{3})/g, "$1" + sep);
        if (z.slice(-sep.length) == sep)
            z = z.slice(0, -sep.length);
        x = "";
        // reverse again to get back the number
        for (i = z.length - 1; i >= 0; i--)
            x += z.charAt(i);
        // add the fraction back in, if it was there
        if (typeof (y) != "undefined" && y.length > 0)
            x += decpoint + y;
    }
    return x;
}

function showPageLoading(d) {
    var loadingPanel = "<div id='pageLoading' style='position:absolute;top:0;left:0;background-color:#fff;opacity:0.5;z-index:9999998;width:100%;height:100%;'>" +
        "<div style='position:relative;width:100%;top:50%;opacity:1;z-index:9999;'><img class='img-responsive center-block text-center' src='" + app_base_url + "img/preloader.gif' alt='' /></div>" +
        "</div>";

    (d) ? $("body").append(loadingPanel) : setTimeout(function () { $("body").find("#pageLoading").remove(); }, 350);
}
