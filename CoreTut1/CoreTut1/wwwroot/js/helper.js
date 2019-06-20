// tc kimlik doğrulama
function check_tcno(tcno) {
    var toplam =
        Number(tcno.substring(0, 1)) + Number(tcno.substring(1, 2)) +
        Number(tcno.substring(2, 3)) + Number(tcno.substring(3, 4)) +
        Number(tcno.substring(4, 5)) + Number(tcno.substring(5, 6)) +
        Number(tcno.substring(6, 7)) + Number(tcno.substring(7, 8)) +
        Number(tcno.substring(8, 9)) + Number(tcno.substring(9, 10));

    var strtoplam = String(toplam);
    onunbirlerbas = strtoplam.substring(strtoplam.length, strtoplam.length - 1);

    if (onunbirlerbas == tcno.substring(10, 11)) {
        return true;
    } else {
        return false;
    }
}
//Mac adresi kontrolü.
function check_macAddress(mac) {
    var regexp = /^(([A-Fa-f0-9]{2}[:]){5}[A-Fa-f0-9]{2}[,]?)+$/i;
    if (regexp.test(mac)) {
        return true;
    } else {
        return false;
    }
}

function validate_iban(value) {
    if (value.length) {
        var iban = value.replace(/ /g, "").toUpperCase(),
            ibancheckdigits = "",
            leadingZeroes = true,
            cRest = "",
            cOperator = "",
            countrycode, ibancheck, charAt, cChar, bbanpattern, bbancountrypatterns, ibanregexp, i, p;

        countrycode = iban.substring(0, 2);
        bbancountrypatterns = {
            "AL": "\\d{8}[\\dA-Z]{16}",
            "AD": "\\d{8}[\\dA-Z]{12}",
            "AT": "\\d{16}",
            "AZ": "[\\dA-Z]{4}\\d{20}",
            "BE": "\\d{12}",
            "BH": "[A-Z]{4}[\\dA-Z]{14}",
            "BA": "\\d{16}",
            "BR": "\\d{23}[A-Z][\\dA-Z]",
            "BG": "[A-Z]{4}\\d{6}[\\dA-Z]{8}",
            "CR": "\\d{17}",
            "HR": "\\d{17}",
            "CY": "\\d{8}[\\dA-Z]{16}",
            "CZ": "\\d{20}",
            "DK": "\\d{14}",
            "DO": "[A-Z]{4}\\d{20}",
            "EE": "\\d{16}",
            "FO": "\\d{14}",
            "FI": "\\d{14}",
            "FR": "\\d{10}[\\dA-Z]{11}\\d{2}",
            "GE": "[\\dA-Z]{2}\\d{16}",
            "DE": "\\d{18}",
            "GI": "[A-Z]{4}[\\dA-Z]{15}",
            "GR": "\\d{7}[\\dA-Z]{16}",
            "GL": "\\d{14}",
            "GT": "[\\dA-Z]{4}[\\dA-Z]{20}",
            "HU": "\\d{24}",
            "IS": "\\d{22}",
            "IE": "[\\dA-Z]{4}\\d{14}",
            "IL": "\\d{19}",
            "IT": "[A-Z]\\d{10}[\\dA-Z]{12}",
            "KZ": "\\d{3}[\\dA-Z]{13}",
            "KW": "[A-Z]{4}[\\dA-Z]{22}",
            "LV": "[A-Z]{4}[\\dA-Z]{13}",
            "LB": "\\d{4}[\\dA-Z]{20}",
            "LI": "\\d{5}[\\dA-Z]{12}",
            "LT": "\\d{16}",
            "LU": "\\d{3}[\\dA-Z]{13}",
            "MK": "\\d{3}[\\dA-Z]{10}\\d{2}",
            "MT": "[A-Z]{4}\\d{5}[\\dA-Z]{18}",
            "MR": "\\d{23}",
            "MU": "[A-Z]{4}\\d{19}[A-Z]{3}",
            "MC": "\\d{10}[\\dA-Z]{11}\\d{2}",
            "MD": "[\\dA-Z]{2}\\d{18}",
            "ME": "\\d{18}",
            "NL": "[A-Z]{4}\\d{10}",
            "NO": "\\d{11}",
            "PK": "[\\dA-Z]{4}\\d{16}",
            "PS": "[\\dA-Z]{4}\\d{21}",
            "PL": "\\d{24}",
            "PT": "\\d{21}",
            "RO": "[A-Z]{4}[\\dA-Z]{16}",
            "SM": "[A-Z]\\d{10}[\\dA-Z]{12}",
            "SA": "\\d{2}[\\dA-Z]{18}",
            "RS": "\\d{18}",
            "SK": "\\d{20}",
            "SI": "\\d{15}",
            "ES": "\\d{20}",
            "SE": "\\d{20}",
            "CH": "\\d{5}[\\dA-Z]{12}",
            "TN": "\\d{20}",
            "TR": "\\d{5}[\\dA-Z]{17}",
            "AE": "\\d{3}\\d{16}",
            "GB": "[A-Z]{4}\\d{14}",
            "VG": "[\\dA-Z]{4}\\d{16}"
        };

        if (typeof bbanpattern !== "undefined") {
            ibanregexp = new RegExp("^[A-Z]{2}\\d{2}" + bbanpattern + "$", "");
            if (!(ibanregexp.test(iban))) {
                return false; // Invalid country specific format
            }
        }

        ibancheck = iban.substring(4, iban.length) + iban.substring(0, 4);
        for (i = 0; i < ibancheck.length; i++) {
            charAt = ibancheck.charAt(i);
            if (charAt !== "0") {
                leadingZeroes = false;
            }
            if (!leadingZeroes) {
                ibancheckdigits += "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".indexOf(charAt);
            }
        }

        // Calculate the result of: ibancheckdigits % 97
        for (p = 0; p < ibancheckdigits.length; p++) {
            cChar = ibancheckdigits.charAt(p);
            cOperator = "" + cRest + "" + cChar;
            cRest = cOperator % 97;
        }

        return cRest === 1;
    }

    return false;
}

(function ($) {
    //"use strict";

    //if (!Array.prototype.indexOf) {
    //    Array.prototype.indexOf = function (item) {
    //        var i, l;

    //        for (i = 0, l = this.length; i < l; i++) {
    //            var t = this[i];
    //            if (item === t) {
    //                return i;
    //            }
    //        }

    //        return -1;
    //    };
    //}

    //if (!Array.prototype.lastIndexOf) {
    //    Array.prototype.lastIndexOf = function (item) {
    //        var i;

    //        for (i = this.length; i <= 0; i--) {
    //            var t = this[i];
    //            if (item === t) {
    //                return i;
    //            }
    //        }

    //        return -1;
    //    };
    //}

    //if (!Array.prototype.indexOfObjectProperty) {
    //    Array.prototype.indexOfObjectProperty = function (name, value) {
    //        var i, l, item;

    //        for (i = 0, l = this.length; i < l; i++) {
    //            item = this[i];

    //            if (name in item && item[name] === value) {
    //                return i;
    //            }
    //        }

    //        return -1;
    //    };
    //}

    //if (!Array.prototype.forEach) {
    //    Array.prototype.forEach = function (func, thisElement) {
    //        var i, l;

    //        thisElement = thisElement || this;

    //        for (i = 0, l = this.length; i < l; i++) {
    //            func.call(thisElement, this[i], i, this);
    //        }
    //    };
    //}

    //if (!Array.prototype.remove) {
    //    Array.prototype.remove = function (item) {
    //        var index = this.indexOf(item);

    //        if (index >= 0) {
    //            return this.splice(index, 1);
    //        }

    //        return null;
    //    }
    //}

    //if (!Array.prototype.insert) {
    //    Array.prototype.insert = function (item, index) {
    //        index = index || 0;
    //        this.splice(index, 0, item);
    //    }
    //}
});

$.fn.numericOnly = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            return (
                key == 8 ||
                key == 9 ||
                key == 13 ||
                key == 46 ||
                key == 109 ||
                //key == 110 ||
                key == 190 ||
                (key >= 35 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        });
    });
};

$.fn.intOnly = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            return (
                key == 8 ||
                key == 9 ||
                key == 13 ||
                key == 46 ||
                (key >= 35 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        });
    });
};

$.fn.letterOnly = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
                e.preventDefault();
            }
        });
    });
};

$.fn.alphanumericOnly = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
        });
    });
};

$.fn.turkishID = function () {
    return this.each(function () {
        $(this).blur(function (e) {
            var val = $(this).val();
            if (!check_tcno(val)) {
                $(this).removeClass("valid").addClass("input-validation-error");
                $(this).attr("aria-invalid", "true");
            } else {
                $(this).removeClass("input-validation-error").addClass("valid");
                $(this).attr("aria-invalid", "false");
            }
        });
    });
}

$.fn.Iban = function () {
    return this.blur(function (e) {
        return validate_iban($(this).val());
    });
}

Array.prototype.groupBy = function (keyFunction) {
    var groups = {};
    this.forEach(function (el) {
        var key = keyFunction(el);
        if (key in groups == false) {
            groups[key] = [];
        }
        groups[key].push(el);
    });
    return Object.keys(groups).map(function (key) {
        return {
            key: key,
            values: groups[key]
        };
    });
};

String.prototype.replaceAll = function (target, replacement) {
    return this.split(target).join(replacement);
};

String.prototype.toDateFrom = function () {
    var dte = eval("new " + this.replace(/\//g, '') + ";");
    dte.setMinutes(dte.getMinutes() - dte.getTimezoneOffset());
    return dte;
}

function displayKeyCode(evt) {
    var textBox = getObject('txtChar');
    var charCode = (evt.which) ? evt.which : event.keyCode
    textBox.value = String.fromCharCode(charCode);
    if (charCode == 8) textBox.value = "backspace"; //  backspace
    if (charCode == 9) textBox.value = "tab"; //  tab
    if (charCode == 13) textBox.value = "enter"; //  enter
    if (charCode == 16) textBox.value = "shift"; //  shift
    if (charCode == 17) textBox.value = "ctrl"; //  ctrl
    if (charCode == 18) textBox.value = "alt"; //  alt
    if (charCode == 19) textBox.value = "pause/break"; //  pause/break
    if (charCode == 20) textBox.value = "caps lock"; //  caps lock
    if (charCode == 27) textBox.value = "escape"; //  escape
    if (charCode == 33) textBox.value = "page up"; // page up, to avoid displaying alternate character and confusing people
    if (charCode == 34) textBox.value = "page down"; // page down
    if (charCode == 35) textBox.value = "end"; // end
    if (charCode == 36) textBox.value = "home"; // home
    if (charCode == 37) textBox.value = "left arrow"; // left arrow
    if (charCode == 38) textBox.value = "up arrow"; // up arrow
    if (charCode == 39) textBox.value = "right arrow"; // right arrow
    if (charCode == 40) textBox.value = "down arrow"; // down arrow
    if (charCode == 45) textBox.value = "insert"; // insert
    if (charCode == 46) textBox.value = "delete"; // delete
    if (charCode == 91) textBox.value = "left window"; // left window
    if (charCode == 92) textBox.value = "right window"; // right window
    if (charCode == 93) textBox.value = "select key"; // select key
    if (charCode == 96) textBox.value = "numpad 0"; // numpad 0
    if (charCode == 97) textBox.value = "numpad 1"; // numpad 1
    if (charCode == 98) textBox.value = "numpad 2"; // numpad 2
    if (charCode == 99) textBox.value = "numpad 3"; // numpad 3
    if (charCode == 100) textBox.value = "numpad 4"; // numpad 4
    if (charCode == 101) textBox.value = "numpad 5"; // numpad 5
    if (charCode == 102) textBox.value = "numpad 6"; // numpad 6
    if (charCode == 103) textBox.value = "numpad 7"; // numpad 7
    if (charCode == 104) textBox.value = "numpad 8"; // numpad 8
    if (charCode == 105) textBox.value = "numpad 9"; // numpad 9
    if (charCode == 106) textBox.value = "multiply"; // multiply
    if (charCode == 107) textBox.value = "add"; // add
    if (charCode == 109) textBox.value = "subtract"; // subtract
    if (charCode == 110) textBox.value = "decimal point"; // decimal point
    if (charCode == 111) textBox.value = "divide"; // divide
    if (charCode == 112) textBox.value = "F1"; // F1
    if (charCode == 113) textBox.value = "F2"; // F2
    if (charCode == 114) textBox.value = "F3"; // F3
    if (charCode == 115) textBox.value = "F4"; // F4
    if (charCode == 116) textBox.value = "F5"; // F5
    if (charCode == 117) textBox.value = "F6"; // F6
    if (charCode == 118) textBox.value = "F7"; // F7
    if (charCode == 119) textBox.value = "F8"; // F8
    if (charCode == 120) textBox.value = "F9"; // F9
    if (charCode == 121) textBox.value = "F10"; // F10
    if (charCode == 122) textBox.value = "F11"; // F11
    if (charCode == 123) textBox.value = "F12"; // F12
    if (charCode == 144) textBox.value = "num lock"; // num lock
    if (charCode == 145) textBox.value = "scroll lock"; // scroll lock
    if (charCode == 186) textBox.value = ";"; // semi-colon
    if (charCode == 187) textBox.value = "="; // equal-sign
    if (charCode == 188) textBox.value = ","; // comma
    if (charCode == 189) textBox.value = "-"; // dash
    if (charCode == 190) textBox.value = "."; // period
    if (charCode == 191) textBox.value = "/"; // forward slash
    if (charCode == 192) textBox.value = "`"; // grave accent
    if (charCode == 219) textBox.value = "["; // open bracket
    if (charCode == 220) textBox.value = "\\"; // back slash
    if (charCode == 221) textBox.value = "]"; // close bracket
    if (charCode == 222) textBox.value = "'"; // single quote
    var lblCharCode = getObject('spnCode');
    lblCharCode.innerHTML = 'KeyCode:  ' + charCode;
    return false;
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));

    // query string: ?foo=lorem&bar=&baz
    // var foo = getParameterByName('foo');
}

function clearFields(region) {
    var inputs = $(region).find(":input");

    $.each(inputs, function (e) {
        $(this).val("");
    });
}

// send region id and check all requiredtxt fields
//checkData verilerin range aralığı kontrolu için.
function checkRequiredFields(region, checkData) {
    var status = true;
    var inputs = $(region).find(":input");
    var requiredFields = $(region).find(":input.required");

    if (checkData != true) {
        $.each(inputs, function (e) {
            var th = $(this);

            if (!th.hasClass("selectized")) {
                th.removeClass("input-validation-error");
            } else {
                th.next("div.selectize-control").find("div.selectize-input").removeClass("input-validation-error");
            }
        });

        $.each(requiredFields, function (e) {
            var th = $(this);

            if (th.val() == "" || th.val() == null) {
                if (!th.hasClass("selectized")) {
                    th.addClass("input-validation-error");
                } else {
                    th.next("div.selectize-control").find("div.selectize-input").addClass("input-validation-error");
                }

                status = false;
            }
        });
    } else {
        $.each(requiredFields, function (e) {
            var th = $(this);

            if (parseFloat(th.val()).toFixed(2) > parseFloat(th.attr("data-val-range-max")) || parseFloat(th.val()).toFixed(2) < parseFloat(th.attr("data-val-range-min"))) {
                th.addClass("input-validation-error");
                status = false;
            }
        });
    }

    return status;
}

function addObjToArray(obj, array, selected) {
    var result = true;
    var status = true;

    if (selected == 0) {
        for (var i = 0; i < array.length; i++) {
            if (JSON.stringify(array[i]) === JSON.stringify(obj)) {
                result = false;
                status = false;
                break;
            }
        }
    } else {
        for (var i = 0; i < array.length; i++) {
            if (array[i].ROW_ID) {
                if (array[i].ROW_ID == obj.ROW_ID) {
                    var prevObj = array[i];
                    $.each(prevObj, function (index, elem) {
                        if (index != "ROW_ID") {
                            array[i][index] = obj[index];
                            if (array[i].Id > 0)
                                array[i] == 3;
                        }
                    });

                    result = false;
                    break;
                }
            } else {
                if (JSON.stringify(array[i]) === JSON.stringify(obj)) {
                    result = false;
                    status = false;
                    break;
                }
            }
        }
    }

    if (result) {
        array.push(obj);
    }

    return status;
}

function removeObjFromArray(value, array) {
    if (array.length) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == value) {
                array.splice(i, 1);
                break;
            }
        }
    }
}

function isObjectEmpty(_obj) {
    if (typeof _obj === "undefined") {
        return true;
    }

    for (var key in _obj) {
        if (key == "length" && _obj.length == 0) {
            return true;
        }

        if (_obj.hasOwnProperty(key)) {
            return false;
        }
    }

    return true;

    //if (_obj.length != 0) {
    //	return Object.keys(_obj).length === 0;
    //}

    //if (typeof _obj.length === "undefined") {
    //	return true;
    //}

    //return _obj.length === 0 ? true : false;
    //return Object.keys(_obj).length === 0;
}

function getRowId(array, selected) {
    var index = 1;

    if (selected) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].ROW_ID == selected) {
                index = array[i].ROW_ID;
                break;
            }
        }
    } else {
        if (array != null && array.length) {
            array.sortBy("ROW_ID");
            index = parseInt(array[array.length - 1].ROW_ID) + 1;

            var isExist = $.grep(array, function (d) { return d.ROW_ID == index });
            if (isExist.length > 0) index++;
        }
    }

    return index;
}

function convertJsonDate(date) {
    if (date != null) {
        if (date.indexOf("Date(") > -1) {
            return moment(new Date(parseInt(date.substr(6)))).format("DD.MM.YYYY");
        } else {
            return date;
        }
    } else {
        return "";
    }
}

function ToJavaScriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return dt.getDate() + "." + (dt.getMonth() + 1) + "." + dt.getFullYear();
}

function ToJavaScriptDateUTC(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return dt.getFullYear() + "," + (dt.getMonth() + 1) + "," + dt.getDate() + "," + dt.getHours()
}

function ConvertStringtoDate(str) {
    if (str != "") {
        var from = str.split(".");
        var f = new Date(from[2], from[1] - 1, from[0]);
        return f;
    }

    return "";
}

function addSearchParam(array, dataColumn, dataType, dataEnumType, dataValue, isChild, isQueryString, isSkip) {
    var searchObj = {
        "DataColumn": dataColumn,
        "DataColumnType": dataType,
        "DataEnumType": dataEnumType,
        "DataType": dataType,
        "DataValue": dataValue,
        "IsChildCollection": isChild,
        "IsQueryString": isQueryString,
        "IsSkip": isSkip
    };

    array.push(searchObj);
}

function removeObjFromArrayByColumn(column, value, array) {
    if (array.length) {
        for (var i = 0; i < array.length; i++) {
            if (typeof array[i][column] !== "undefined" && array[i][column] == value) {
                array.splice(i, 1);
                break;
            }
        }
    }

    return array;
}

function clearArray(firstArr, secondArr) {
    for (var i = 0; i < secondArr.length; i++) {
        for (var j = 0; j < firstArr.length; j++) {
            if (firstArr[j].DataColumn == secondArr[i].DataColumn) {
                firstArr.splice(j, 1);
            }
        }
    }

    return firstArr;
}

function openBootbox(content, animate, title, size, className) {
    bootbox.dialog({
        animate: animate,
        message: content,
        title: title,
        size: size,
        className: className,
        onEscape: function () { bootbox.hideAll(); $("body #ekle").unbind("click"); }
    });
}

function confirmBox(content, confirmBtn, cancelBtn, callback) {
    bootbox.confirm({
        message: content,
        buttons: {
            confirm: {
                label: confirmBtn,
                className: "btn-primary"
            },
            cancel: {
                label: cancelBtn,
                className: "btn-default"
            }
        },
        callback: function (result) {
            if (result) {
                eval(callback);
            }
        }
    });
}

function updateAllColumnValueOfAnArray(array, column, value, container, template) {
    if (array.length > 0) {
        for (var i = 0; i < array.length; i++) {
            array[i][column] = value;

            if (array[i]["Status"] != 5) {
                array[i]["Status"] = array[i]["Status"] != 1 ? 3 : 1;
            }
        }

        $(container).empty();
        $(template).tmpl(indirectDetails).appendTo(container);
    }
}

function setToLocalStorage(name, value) {
    localStorage.setItem(name, value);
}

function getFromLocalStorage(name) {
    return localStorage.getItem(name);
}
