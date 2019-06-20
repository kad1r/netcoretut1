/// <reference path="main.js" />

var postsend = true,
    frm = $("form"),
    toolbar_items = $("[id*='toolbar']");

$(function () {
    checkToolbarItems();

    $('[data-toggle="tooltip"]').tooltip();

    if (menu_id > 0) {
        // yeni kayit
        if ($("#toolbar_new").length) {
            var href = $("#toolbar_new").attr("href");

            if (href !== undefined) {
                $("#toolbar_new").attr("href", addMenuId(href));
            }
        }

        if ($("a._list_page").length) {
            // listeme sayfasi
            var query = $("a._list_page").attr("href");

            if (query.indexOf("?") > -1) {
                query = query + "&menu_id=" + menu_id;
            } else {
                query = query + "?menu_id=" + menu_id;
            }

            $("a._list_page").attr("href", query);
        }
    }

    actionMethod();
});

$("body").on("click", "#toolbar_view", function (e) {
    debugger;
    e.preventDefault();
    var _preview = "goruntule=1";

    if (rowIds.length == 0) {
        bootbox.alert("Lütfen kayıt seçiniz.");
    } else {
        if (rowIds.length > 1) {
            bootbox.alert("Görüntüleme yapmak için lütfen tek kayıt seçiniz.");
        } else {
            var item = $("input:checkbox.primary-check[value=" + "'" + rowIds[0] + "'" + "]");
            var query = item.data("href");

            if (query.indexOf("?") > -1) {
                query = query + "&" + _preview;
            } else {
                query = query + "?" + _preview;
            }

            if ($(".alimListe").val)
                window.open(addMenuId(query), '_blank');
            else
                location.href = addMenuId(query);
        }
    }
});

$("body").on("click", "#toolbar_new", function (e) {
    e.preventDefault();

    if ($(".showPopUpForNewRecord").length) {
        if ($(".showPopUpForNewRecord").val() == "true") {
            bootbox.confirm("Yeni kayıt açmak istediğinizden emin misiniz?", function (res) {
                if (res) {
                    var href = $("input:hidden.new_item").val();

                    location.href = addMenuId(href);
                }
            });
        }
    } else {
        var url = $(this).attr("href");

        location.href = url;
    }
});

$("body").on("click", "#toolbar_newdata", function (e) {
    e.preventDefault();

    if ($(".showPopUpForNewRecord").length) {
        if ($(".showPopUpForNewRecord").val() == "true") {
            var refdataid = $("input:hidden.new_item").data('id');

            if (refdataid > 0) {
                bootbox.confirm("Geçerli verilerle yeni kayıt açmak istediğinizden emin misiniz?", function (res) {
                    if (res) {
                        var url = $("input:hidden.new_item").val();

                        location.href = url + "&" + "refid=" + refdataid + "&menu_id=" + menu_id;
                    }
                });
            }
        }
    } else {
        var url = $(this).attr("href");

        location.href = url + "&" + "refid=" + $("input:hidden.new_item").data('id') + "&menu_id=" + menu_id;
    }
});

$("body").on("click", "#toolbar_save", function (e) {
    //<input type="hidden" value="true" id="isFormValid" />
    //debugger;
    e.preventDefault();
    var canSubmit = checkRequiredFields("form", false);

    if (canSubmit) {
        if ($("#isFormValid").length) {
            submitForm();
        }
        else {
            if (postsend) {
                if (frm.valid()) {
                    postsend = false;
                    toolbarLoading();

                    frm.submit();
                }
            }
        }
    }
});

$("body").on("click", "#toolbar_delete", function (e) {
    e.preventDefault();

    if ($("input:hidden.delete_item").data("id")) {
        bootbox.confirm("Silmek istediğinize emin misiniz?", function (res) {
            if (res) {
                toolbarLoadingDel(true);

                var url = $("input:hidden.delete_item").val();
                var id = $("input:hidden.delete_item").data("id");
                var Ids = [];

                if (parseInt(id) > 0) {
                    Ids.push(id);
                    deleteRecord(url, Ids);
                }
            }
        });
    } else {
        if (rowIds.length) {
            bootbox.confirm("Silmek istediğinize emin misiniz?", function (res) {
                if (res) {
                    toolbarLoadingDel(true);

                    var url = $("input:hidden.delete_item").val();
                    deleteRecord(url, rowIds);
                }
            });
        } else {
            bootbox.alert("Silmek için kayıt seçiniz.");
        }
    }
});

$("body").on("click", "#toolbar_edit", function (e) {
    e.preventDefault();

    if (rowIds.length == 0) {
        bootbox.alert("Lütfen kayıt seçiniz.");
    } else {
        if (rowIds.length > 1) {
            bootbox.alert("Düzenleme yapmak için lütfen tek kayıt seçiniz.");
        }
        else {
            var item = $("input:checkbox.primary-check[value=" + "'" + rowIds[0] + "'" + "]");
            var href = item.data("href");

            location.href = addMenuId(href);
        }
    }
});

$("body").on("click", "#qr_reader", function (e) {
    e.preventDefault();

    //if (rowIds.length == 0) {
    //    bootbox.alert("Lütfen kayıt seçiniz.");
    //} else {
    //    if (rowIds.length > 1) {
    //        bootbox.alert("Düzenleme yapmak için lütfen tek kayıt seçiniz.");
    //    }
    //    else {
    //        var item = $("input:checkbox.primary-check[value=" + "'" + rowIds[0] + "'" + "]");
    //        var href = item.data("href");

    location.href = "/IsEmri/QrReaderCamera";
    //    }
    //}
});

$("body").on("click", "#toolbar_approve", function (e) {
    e.preventDefault();
    //debugger;
    if ($("input:hidden.approve_item").data("id")) {
        bootbox.confirm("Onaylamak istediğinize emin misiniz?", function (res) {
            if (res) {
                toolbarLoadingDel(true);

                var url = $("input:hidden.approve_item").val();
                var id = $("input:hidden.approve_item").data("id");
                var Ids = [];

                if (parseInt(id) > 0) {
                    Ids.push(id);
                    deleteRecord(url, Ids);
                }
            }
        });
    } else {
        if (rowIds.length) {
            bootbox.confirm("Onaylamak istediğinize emin misiniz?", function (res) {
                if (res) {
                    //toolbarLoadingDel(true);
                    //debugger;
                    var url = $("input:hidden.approve_item").val();
                    deleteRecord(url, rowIds);
                }
            });
        } else {
            bootbox.alert("Onaylamak için kayıt seçiniz.");
        }
    }
});

$("body").on("click", "#toolbar_denied", function (e) {
    e.preventDefault();

    if ($("input:hidden.denied_item").data("id")) {
        bootbox.confirm("Reddetmek istediğinize emin misiniz?", function (res) {
            if (res) {
                toolbarLoadingDel(true);

                var url = $("input:hidden.denied_item").val();
                var id = $("input:hidden.denied_item").data("id");
                var Ids = [];

                if (parseInt(id) > 0) {
                    Ids.push(id);
                    deleteRecord(url, Ids);
                }
            }
        });
    } else {
        if (rowIds.length) {
            bootbox.confirm("Reddetmek istediğinize emin misiniz?", function (res) {
                if (res) {
                    //toolbarLoadingDel(true);

                    var url = $("input:hidden.denied_item").val();
                    deleteRecord(url, rowIds);
                }
            });
        } else {
            bootbox.alert("Reddetmek için kayıt seçiniz.");
        }
    }
});

$("body").on("click", "#toolbar_cancel", function (e) {
    e.preventDefault();

    if ($("input:hidden.cancel_item").data("id")) {
        bootbox.confirm("İptal etmek istediğinize emin misiniz?", function (res) {
            if (res) {
                toolbarLoadingDel(true);

                var url = $("input:hidden.cancel_item").val();
                var id = $("input:hidden.cancel_item").data("id");
                var Ids = [];

                if (parseInt(id) > 0) {
                    Ids.push(id);
                    deleteRecord(url, Ids);
                }
            }
        });
    } else {
        if (rowIds.length) {
            bootbox.confirm("İptal istediğinize emin misiniz?", function (res) {
                if (res) {
                    //toolbarLoadingDel(true);

                    var url = $("input:hidden.cancel_item").val();
                    deleteRecord(url, rowIds);
                }
            });
        } else {
            bootbox.alert("İptal için kayıt seçiniz.");
        }
    }
});

$("body").on("click", "#toolbar_export", function (e) {
    e.preventDefault();
    var item = $("#recordCount");

    if (item) {
        var val = $("#recordCount").text();

        if (val.indexOf(",") != -1) {
            val = val.replace(/,/g, '');
        }

        if (val.indexOf(".") != -1) {
            val = val.replace(/./g, '');
        }

        var recordCount = parseFloat(val);

        if (recordCount > parseFloat(65000)) {
            showToastr("warning", "", ExcelRecordWarning)
        } else {
            var excel_items = $('.excel_item').val();

            if (excel_items != "") {
                window.open(excel_items);
            }
        }
    }
});

$("body").on("click", "#toolbar_quick", function (e) {
    e.preventDefault();
    if ($(".showPopUpForNewRecord").length) {
        if ($(".showPopUpForNewRecord").val() == "true") {
            bootbox.confirm("Yeni kayıt açmak istediğinizden emin misiniz?", function (res) {
                if (res) {
                    var href = $("input:hidden.new_item").val();

                    location.href = addMenuId(href);
                }
            });
        }
    } else {
        var url = $(this).attr("href");

        location.href = url;
    }
});

function checkToolbarItems() {
    var url = window.location.search; // direk querystring'i veriyor

    if (url.indexOf("goruntule") > -1) {
        $.each(toolbar_items, function (i, toolbar_items) {
            $(this).removeAttr("id").addClass("toolbar_disabled");
        });
    } else {
        if (frm.length > 0) {
            // form exists is true
            $.each(toolbar_items, function (i, toolbar_items) {
                var toolbardisabledState = "";

                if (toolbar_items.id == "toolbar_new" || toolbar_items.id == "toolbar_newdata") {
                    toolbardisabledState = $("input:hidden.new_item").data("disabled");
                } else if (toolbar_items.id == "toolbar_edit") {
                    toolbardisabledState = $("input:hidden.toolbar_edit").data("disabled");
                } else if (toolbar_items.id == "toolbar_save") {
                    if ($("#isFormValid").length) {
                        toolbardisabledState = $("#isFormValid").data("disabled");
                    }
                } else if (toolbar_items.id == "toolbar_delete") {
                    toolbardisabledState = $("input:hidden.delete_item").data("disabled");
                } else if (toolbar_items.id == "toolbar_export")
                    toolbardisabledState = $("input:hidden.excel_item").data("disabled");

                if (toolbardisabledState == true) {
                    $(this).removeAttr("id").addClass("toolbar_disabled");
                } else {
                    if (toolbar_items.id != "toolbar_save" && toolbar_items.id != "toolbar_new" && toolbar_items.id != "toolbar_newdata") {
                        if (toolbar_items.id == "toolbar_delete") {
                            var id = $("input:hidden.delete_item").data("id");

                            if (parseInt(id) <= 0) {
                                $(this).removeAttr("id").addClass("toolbar_disabled");
                            }
                        } else {
                            $(this).removeAttr("id").addClass("toolbar_disabled");
                        }
                    } else {
                        if (toolbar_items.id != "toolbar_new") {
                            var new_item = $("body .new_item").val();
                            var ispagenew = $("input:hidden.new_item").data("id");
                            var issaveas = $("input:hidden.new_item").data("issaveas");

                            if (typeof (issaveas) === "undefined") {
                                issaveas = true;
                            }

                            if (ispagenew === undefined) {
                                $("body #toolbar_new").removeClass("toolbar_disabled");
                            } else {
                                if (ispagenew > 0) {
                                    var refdataid = $("input:hidden.new_item").data('id');

                                    new_item += "&refid=" + refdataid + "&menu_id=" + menu_id;
                                    $("body #toolbar_new").attr("href", new_item);

                                    if (issaveas) {
                                        $("body #toolbar_newdata").attr("href", new_item);
                                    } else {
                                        $("body #toolbar_newdata").addClass("toolbar_disabled");
                                    }
                                } else {
                                    $("body #toolbar_new, body #toolbar_newdata").attr("href", new_item).addClass("toolbar_disabled");
                                }
                            }
                        } else if (toolbar_items.id != "toolbar_delete") {
                            var new_item = $("body .new_item").val();
                            var ispagenew = $("input:hidden.new_item").data("id");
                            var issaveas = $("input:hidden.new_item").data("issaveas");

                            if (typeof (issaveas) === "undefined") {
                                issaveas = true;
                            }

                            if (ispagenew > 0) {
                                $("body #toolbar_new").attr("href", new_item);

                                if (issaveas) {
                                    $("body #toolbar_newdata").attr("href", new_item);
                                } else {
                                    $("body #toolbar_newdata").addClass("toolbar_disabled");
                                }
                            } else {
                                $("body #toolbar_new, body #toolbar_newdata").attr("href", new_item).addClass("toolbar_disabled");
                            }
                        }
                    }
                }
            });
        } else {
            // form is not exist
            $.each(toolbar_items, function (i, toolbar_items) {
                var toolbardisabledState = "";

                if (toolbar_items.id == "toolbar_new") // || toolbar_items.id == "toolbar_edit"
                    toolbardisabledState = $("input:hidden.new_item").data('disabled');
                else if (toolbar_items.id == "toolbar_edit")
                    toolbardisabledState = $("input:hidden.edit_item").data('disabled');
                else if (toolbar_items.id == "toolbar_delete")
                    toolbardisabledState = $("input:hidden.delete_item").data('disabled');
                else if (toolbar_items.id == "toolbar_view")
                    toolbardisabledState = $("input:hidden.view_item").data('disabled');
                else if (toolbar_items.id == "toolbar_export")
                    toolbardisabledState = $("input:hidden.export_item").data('disabled');

                if (toolbardisabledState == true) {
                    $(this).removeAttr("id").addClass("toolbar_disabled");
                } else {
                    if (toolbar_items.id == "toolbar_save" || toolbar_items.id == "toolbar_newdata") {
                        $(this).removeAttr("id").addClass("toolbar_disabled");
                    }
                }
            });

            var new_item = $("body .new_item").val();

            $("body #toolbar_new").prop("href", new_item);
        }
    }
}

function deleteRecord(url, arrayIds) {
    if (arrayIds.length > 0) {
        $.post(url, { list: arrayIds }, function (data) {
            if (data.result) {
                rowIds = [];
                showToastr("success", "", data.message);

                // Bazen cok hizli islem oldugunda bilgi mesaji goruntulenemeden yonlenme oluyor.
                // Bu nedenle 0.5 saniye bekletip yeniden yonlendiriyoruz.
                setTimeout(function () {
                    if (document.forms.length) {
                        var refreshUrl = $("body .list_item").val();

                        $("body .list-records").empty().load(refreshUrl, function () {
                            BindICheck();
                        });

                        if ($("body .new_item").length) {
                            // Farklı bir formdetay içerisinden tekrar oluiturma yaptığımız
                            // form içerisine yönlendirmek için
                            if ($("body #overideDelete").length > 0) {
                                var href = $("body #overridedNewUrl").val();

                                $("body .new_item").attr("href", (addMenuId(href)));
                                location.href = addMenuId(href);
                            } else {
                                var href = $("body .new_item").val();

                                $("body .new_item").attr("href", (addMenuId(href)));
                                location.href = addMenuId(href);
                            }
                        }
                    } else {
                        if ($("body .new_item").length) {
                            var href = $("body .new_item").val();

                            $("body .new_item").attr("href", (addMenuId(href)));
                        }

                        location.reload(true);
                    }

                    toolbarLoadingDel(false);
                }, 500);
            }
            else {
                if (data.message != null) {
                    showToastr("error", "", data.message);
                }
                else {
                    showToastr("error", "", "Bazı verileriniz ilişkili olduğundan silinemedi.");
                }

                if ($("body .new_item").length) {
                    var href = $("body .new_item").val();

                    $("body .new_item").val(addMenuId(href));
                }

                toolbarLoadingDel(false);
            }
        });
    }
}

// * Form uzerindeki action url'ye menu_id eklenir ve yeniden yazdirilir
function actionMethod() {
    var frm = $("form");

    if (frm.length) {
        var href = frm.attr("action");

        if (href.indexOf("?") > -1) {
            href = href + "&menu_id=" + menu_id;
        } else {
            href = href + "?menu_id=" + menu_id;
        }

        frm.attr("action", href);
    }
}

function addMenuId(href) {
    if (href.indexOf("menu_id") === -1) {
        if (href.indexOf("?") > -1) {
            href = href + "&menu_id=" + menu_id;
        } else {
            href = href + "?menu_id=" + menu_id;
        }
    }

    return href;
}
