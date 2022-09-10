
var ilms = ilms || {};
ilms.expirationDate = '';
ilms.lotNumber = '';
ilms.productMode = '';
ilms.routers = {
    // Controller Routes
    UpdateAppSetting: "/Users/UpdateAppSetting",
    UpsertUser: "/Users/UpsertUser",
    GetUserInfo: "/Users/GetUserInfo",
    ChangePassword: "/Account/ChangePassword",
    UpdateUserStatus: "/Users/UpdateUserStatus",
    UpsertRole: "/Users/UpsertRole",
    UpdateRoleStatus: "/Users/UpdateRoleStatus",
    SetRoleStatusAjax: "/Account/SetRoleStatusAjax",
    Forgot: "/Account/Forgot",
    UpsertCylinderType: "/Cylinder/UpsertCylinderType",
    GetCylinderTypeByID: "/Cylinder/GetCylinderTypeByID",
    UpsertCylinder: "/Cylinder/UpsertCylinder",
    GetCylinderByID: "/Cylinder/GetCylinderByID",
    ManageAddress: "/Customer/ManageAddress",
    GetCustomerByID: "/Customer/GetCustomerByID",
    UpsertCustomer: "/Customer/UpsertCustomer",
    GetRouteByID: "/Route/GetRouteByID",
    UpsertRoute: "/Route/UpsertRoute",
    UpdateRouteStatus: "/Route/UpdateRouteStatus",
    ManageDriverRoute: "/Route/ManageDriverRoute",
    ManageDriverRouteList: "/Route/ManageDriverRouteList",
    UpsertDriverRoute: "/Route/UpsertDriverRoute",
    GetDriverRouteInfo: "/Route/GetDriverRouteByID",
    UpdateDriverRouteStatus: "/Route/UpdateDriverRouteStatus",
    ManageLocation: "/Location/ManageLocations",
    ManageLocationList: "/Location/ManageLocationList",
    UpdateCustomerLocationStatus: "/Location/UpdateCustomerLocationStatus",
    UpsertCustomerLocation: "/Location/UpsertLocation",
    GetLocationByID: "/Location/GetLocationByID",
    GetContactList: "/Location/GetContactList",
    UpsertPriceLevel: "/Price/UpsertPriceLevel",
    GetPriceLevelByID: "/Price/GetPriceLevelByID",
    GetAllCylinderSizeByID: "/Cylinder/GetAllCylinderSizeByID",
    GetAllCylinderVolumeByID: "/Cylinder/GetAllCylinderVolumeByID",
    GetAllCylinderSize: "/Order/GetAllCylinderSizeByID",
    GetAllCylinderVolume: "/Order/GetAllCylinderVolumeByID",
    GetAllLocationByCustomerID: "/Order/GetAllLocationByCustomerID",
    UpsertOrder: "/Order/UpsertOrder",
    GetOrderByID: "/Order/GetOrderByID",
    GetCylinderSizesByProductID: "/Order/GetCylinderSizesByProductID",
    GetGasesVolumeByCylinderSizeId: "/Order/GetGasesVolumeByCylinderSizeId",
    CheckCylinderInventory: "/Order/CheckCylinderInventory",
    UpdateCylinderStatus: "/Cylinder/UpdateCylinderStatus",
    GetAllOrders: "/Driver/GetAllOrders",
    UpdateOrderStatus: "/Driver/UpdateOrderStatus",
    GetOrderItemsByOrderId: "/Driver/GetOrderItemsByOrderId",
    GetAllDriverByLocationId:"/Order/GetAllDriverByLocationId"
};

ilms.box = {
    Success: "Success",
    Alert: "Alert",
    Warning: "Warning",
    Error: "Error",
    Validation: "Validation"
};

ilms.commonFeedbackMessages = {
    ErrorMessage: "An unexpected error has been occured while processing your request.Please contact support."
};

ilms.partialPageLoadingToElement = function (pageUrl, dataObj, elementId, btnForLoading, callback, showOverlay = false) {
    if (!ilms.isNullOrEmpty(btnForLoading)) {
        var btnOriginalHtml = ilms.showLoadingToButtonAndReturnOriginalHtml(btnForLoading);
    }
    $.ajax({
        cache: false,
        url: pageUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        data: dataObj,
        //async: true,
        dataType: 'html',
        success: function (data) {
            if (data.indexOf('"error":true,') !== -1) {
                ilms.showAlertWithType(JSON.parse(data).msg, "error");
            } else {
                $("#" + elementId).empty().html(data);
            }
            ilms.handleAjaxCallBackAndLoadingButton(btnForLoading, btnOriginalHtml, callback, data);
            ilms.hideLoading();
        },
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            //Download progress
            xhr.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    $("#" + elementId).find('.progress-bar').css('width', Math.round(evt.loaded / evt.total * 100) + "%");
                }
            }, false);
            return xhr;
        },
        beforeSend: function () {
            if (showOverlay) {
                ilms.showLoading();
            }
            else if (ilms.isNullOrEmpty(btnForLoading)) {
                $("#" + elementId).empty().html(ilms.GetProgressBarHtml(ilms.ColorClass.Primary));
            }
        },
        error: function (ex) {
            if (!ilms.isNullOrEmpty($("#" + elementId).find('.progress-bar'))) {
                $("#" + elementId).empty();
            }
            ilms.handleAjaxCallBackAndLoadingButton(btnForLoading, btnOriginalHtml, callback, false);
            ilms.ajaxErrorCall(ex);
        }
    });
};

ilms.partialPageLoadingToElementPostCall = function (pageUrl, dataObj, elementId, btnForLoading, callback, showOverlay = false) {
    if (!ilms.isNullOrEmpty(btnForLoading)) {
        var btnOriginalHtml = ilms.showLoadingToButtonAndReturnOriginalHtml(btnForLoading);
    }
    $.ajax({
        cache: false,
        url: pageUrl,
        //contentType: 'application/html; charset=utf-8',
        type: 'POST',
        async: true,
        data: dataObj,
        //dataType: 'html',
        success: function (data) {
            if (data.indexOf('"error":true,') !== -1) {
                ilms.showAlertWithType(JSON.parse(data).msg, "Sorry, Something went wrong");
            } else {
                $("#" + elementId).empty().html(data);
            }
            ilms.handleAjaxCallBackAndLoadingButton(btnForLoading, btnOriginalHtml, callback, data);
            ilms.hideLoading();
        },
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            //Download progress
            xhr.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    $("#" + elementId).find('.progress-bar').css('width', Math.round(evt.loaded / evt.total * 100) + "%");
                }
            }, false);
            return xhr;
        },
        beforeSend: function () {
            if (showOverlay) {
                ilms.showLoading();
            }
            else if (ilms.isNullOrEmpty(btnForLoading)) {
                $("#" + elementId).empty().html(ilms.GetProgressBarHtml(ilms.ColorClass.Primary));
            }
        },
        error: function (ex) {
            if (!ilms.isNullOrEmpty($("#" + elementId).find('.progress-bar'))) {
                $("#" + elementId).empty();
            }
            ilms.handleAjaxCallBackAndLoadingButton(btnForLoading, btnOriginalHtml, callback, false);
            ilms.ajaxErrorCall(ex);
        }
    });
};

ilms.ajaxPostCall = function (pageUrl, dataObj, callback, btnForLoading, showOverlay = true) {
    if (!ilms.isNullOrEmpty(btnForLoading)) {
        var btnOriginalHtml = $(btnForLoading)[0].innerHTML;
        ilms.showLoadingToButtonAndReturnOriginalHtml(btnForLoading);
    }
    if (showOverlay) {
        ilms.showLoading();
    }
    $.ajax({
        cache: false,
        url: pageUrl,
        type: 'POST',
        data: dataObj,
        async: true,
        success: function (data, status, xhr) {
            ilms.hideLoading();
            if (!ilms.isNullOrEmpty(btnForLoading)) {
                ilms.hideLoadingFromButton(btnForLoading, btnOriginalHtml);
            }
            if (!ilms.isNullOrEmpty(callback)) {
                callback(data);
            }
        },
        error: function (xhr, ex) {
            if (!ilms.isNullOrEmpty(btnForLoading)) {
                ilms.hideLoadingFromButton(btnForLoading, btnOriginalHtml);
            }
            ilms.ajaxErrorCall(ex);
        }
    });
};
ilms.ajaxGetCall = function (pageUrl, dataObj, callback, btnForLoading, showOverlay = true) {
    if (!ilms.isNullOrEmpty(btnForLoading)) {
        var btnOriginalHtml = $(btnForLoading)[0].innerHTML;
        ilms.showLoadingToButtonAndReturnOriginalHtml(btnForLoading);
    }
    if (showOverlay) {
        ilms.showLoading();
    }
    $.ajax({
        cache: false,
        url: pageUrl,
        type: 'GET',
        data: dataObj,
        async: true,
        success: function (data, status, xhr) {
            ilms.hideLoading();
            if (!ilms.isNullOrEmpty(btnForLoading)) {
                ilms.hideLoadingFromButton(btnForLoading, btnOriginalHtml);
            }
            if (!ilms.isNullOrEmpty(callback)) {
                callback(data);
            }
        },
        error: function (xhr, ex) {
            if (!ilms.isNullOrEmpty(btnForLoading)) {
                ilms.hideLoadingFromButton(btnForLoading, btnOriginalHtml);
            }
            ilms.ajaxErrorCall(ex);
        }
    });
};

ilms.handleAjaxCallBackAndLoadingButton = function (btnForLoading, btnOriginalHtml, callback, data) {
    if (!ilms.isNullOrEmpty(btnForLoading)) {
        ilms.hideLoadingFromButton(btnForLoading, btnOriginalHtml);
    }
    if (!ilms.isNullOrEmpty(callback)) {
        callback(data);
    }
};

ilms.showLoading = function (msg) {
    $('#overlay-text').html(ilms.isNullOrEmpty(msg) ? "Loading please wait..." : msg);
    $('#overlay').removeClass('d-none');
};

ilms.hideLoading = function () {
    $('#overlay').addClass('d-none');
};

//=== Common ===//
ilms.partialPageLoadingPost = function (pageUrl, dataObj) {
    ilms.showLoading();
    $.ajax({
        cache: false,
        url: pageUrl,
        type: 'POST',
        data: dataObj,
        //async: true,
        success: function (data, status, xhr) {
            if (xhr.getResponseHeader('LOGIN_SCREEN_Ready') === '1') {
                ilms.WhenSessionIsExpired();
                return;
            }
            $('#RenderBody').empty().html(data);
            ilms.hideLoading();
        },
        error: function (xhr, ex) {
            if (xhr.getResponseHeader('LOGIN_SCREEN_Ready') === '1') {
                ilms.WhenSessionIsExpired();
                return;
            }
            ilms.ajaxErrorCall(ex);
            //alert(ex);
        }

    });

};

// Post form with files.
ilms.ajaxPostDataWithFiles = function (pageUrl, dataObj, successMessage, callBack, showMessage = true) {

    ilms.showLoading();
    $.ajax({
        type: "POST",
        url: pageUrl,
        dataType: "json",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: dataObj,
        success: function (result, status, xhr) {
            //if (showMessage) {
            //    if (result > 0 || result !== null || result !== "") {
            //        ilms.notification(ilms.box.Success, successMessage);
            //    }
            //    else {
            //        ilms.notification(ilms.box.Error, 'Something went wrong! Please Contact Support.....');
            //    }
            //}
            ilms.hideLoading();
            if (callBack !== undefined) { callBack(result); }
        },
        error: function (xhr, status, error) {

            ilms.hideLoading();
            if (xhr.getResponseHeader('LOGIN_SCREEN_Ready') === '1') {
                ilms.WhenSessionIsExpired();
                return;
            }
            ilms.notification(ilms.box.Error, 'Something went wrong! Please Contact Support!!!!');
        }
    });

};

ilms.ajaxErrorCall = function (ex) {
    ilms.hideLoading();
    if (ex.responseJSON !== undefined) {
        ilms.showAlertWithType(ex.responseJSON.message, "error");
    } else if (ex.statusText !== undefined) {
        ilms.showAlertWithType(ex.statusText, "error");
    } else if (ex.responseText !== undefined) {
        ilms.showAlertWithType(ex.responseText, "error");
    } else {
        ilms.showAlertWithType('Something went wrong.', "error");
    }
};

ilms.showLoadingToButtonAndReturnOriginalHtml = function (btn) {
    let buttonPrevHtml = $(btn)[0].innerHTML;
    $(btn)[0].innerHTML = '<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span> Please wait..';
    $(btn).attr("disabled", "disabled");
    return buttonPrevHtml;
};

ilms.hideLoadingFromButton = function (btn, btnPrevHtml) {
    $(btn)[0].innerHTML = btnPrevHtml;
    $(btn).removeAttr("disabled");
};
// Http Status Codes
ilms.StatusCodes = {
    Success: 200,
    Duplication: 400,
    Error: 404,
    Forbidden: 403
};

//<!-- Smart Notification -->
ilms.showNotificationByStatusCode = function (statusCode, feedBackMessage) {
    if (statusCode === ilms.StatusCodes.Success) {
        ilms.showAlertWithType(feedBackMessage, "success");
    } else if (statusCode === ilms.StatusCodes.Duplication || statusCode === ilms.StatusCodes.Warning) {
        ilms.showAlertWithType(feedBackMessage, null, "warning", "Warning");
    }
    else {
        ilms.showAlertWithType(feedBackMessage, "error");
    }
};

ilms.showAlertWithType = function (msg, type = "success", timer = 3000) {
    Swal.fire(
        {
            //position: "top-end",
            icon: type,
            title: msg,
            showConfirmButton: false,
            timer: timer,
            customClass: "swal-size-sm"
        });
};

ilms.getFormData = function ($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
};

/*----/*check for null or empty*/
ilms.isNullOrEmpty = function (source) {
    if (source === "" || source === null || source === undefined) {
        return true;
    }
    return false;
};

jQuery(document).ready(function ($) {

});

ilms.ValidateEmail = function (email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
};

ilms.ValidateDateOfBirth = function (dob) {
    var myDate = new Date(dob);
    if (myDate === 'Invalid Date') {
        ilms.notification(ilms.box.Validation, 'Wrong Date Of Birth! because it is invalid Date');
        return false;
    }
    var today = new Date();
    var startDate = new Date('1900, 01, 01');
    if (myDate > today) {
        ilms.notification(ilms.box.Validation, 'Wrong Date Of Birth! because it is future Date');
        return false;
    } else {
        if (myDate < startDate) {
            ilms.notification(ilms.box.Validation, 'Wrong Date Of Birth! because it is old Date');
            return false;
        } else {
            return true;
        }
    }
};

ilms.ValidateDate = function (date, fieldName) {
    var myDate = new Date(date);
    if (myDate === 'Invalid Date') {
        ilms.notification(ilms.box.Validation, 'Wrong ' + fieldName + ' Date! because it is invalid Date');
        return false;
    }
    var today = new Date();
    var startDate = new Date('1900, 01, 01');
    if (myDate > today) {
        ilms.notification(ilms.box.Validation, 'Wrong ' + fieldName + ' Date! because it is future Date');
        return false;
    } else {
        if (myDate < startDate) {
            ilms.notification(ilms.box.Validation, 'Wrong ' + fieldName + ' Date! because it is old Date');
            return false;
        } else {
            return true;
        }
    }
};

ilms.ValidatePhoneNumber = function validatePhoneNumber(elementValue) {
    var phoneNumberPattern = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/;
    return phoneNumberPattern.test(elementValue);
};
/*---------------------------------*/

ilms.CopyToClipboard = function (textToCopy, message) {
    //debugger
    const el = document.createElement('textarea');
    //message = message || 'copied to clipboard';
    el.value = textToCopy;
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": 100,
        "hideDuration": 100,
        "timeOut": 2000,
        "extendedTimeOut": 1000,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr["success"]((message || 'copied to clipboard'), "Success");
};

ilms.showDecisionAlert = function (title, message, type = "warning", confirmationText, confirmationCallBack, cancelCallBack, cancelButtonText = "Cancel", showCancelButton = true, showDefaultTitle = true) {
    Swal.fire(
        {
            title: ilms.isNullOrEmpty(title) && showDefaultTitle ? "Are you sure?" : title,
            text: message,
            icon: ilms.isNullOrEmpty(type) ? "warning" : type,
            showCancelButton: showCancelButton,
            reverseButtons: true,
            confirmButtonText: '<i class="fal fa-check mr-1"></i>' + confirmationText,
            cancelButtonText: '<i class="fal fa-times mr-1"></i>' + cancelButtonText,
            onBeforeOpen: function (ele) {
                $(ele).find('button.swal2-confirm.swal2-styled').toggleClass('swal2-confirm swal2-styled swal2-confirm btn btn-sm btn-info ml-1')
                $(ele).find('button.swal2-cancel.swal2-styled').toggleClass('swal2-cancel swal2-styled swal2-cancel btn btn-sm btn-danger mr-1')
            },

        }).then(function (result) {
            if (result.value) {
                if (!ilms.isNullOrEmpty(confirmationCallBack)) {
                    confirmationCallBack();
                }
            }
            else {
                if (!ilms.isNullOrEmpty(cancelCallBack)) {
                    cancelCallBack();
                }
            }
        });
};

ilms.showDecisionAlertWithHTml = function (title, html, type = "warning", confirmationText, confirmationCallBack, cancelCallBack, cancelButtonText = "Cancel", showCancelButton = true, width = "") {
    Swal.fire(
        {
            title: ilms.isNullOrEmpty(title) ? "Are you sure?" : title,
            html: html,
            icon: type,
            showCancelButton: showCancelButton,
            reverseButtons: true,
            width: width,
            confirmButtonText: '<i class="fal fa-check mr-1"></i>' + confirmationText,
            cancelButtonText: '<i class="fal fa-times mr-1"></i>' + cancelButtonText,
            onBeforeOpen: function (ele) {
                $(ele).find('button.swal2-confirm.swal2-styled').toggleClass('swal2-confirm swal2-styled swal2-confirm btn btn-sm btn-info ml-1')
                $(ele).find('button.swal2-cancel.swal2-styled').toggleClass('swal2-cancel swal2-styled swal2-cancel btn btn-sm btn-danger mr-1')
            }

        }).then(function (result) {
            if (result.value) {
                if (!ilms.isNullOrEmpty(confirmationCallBack)) {
                    confirmationCallBack();
                }
            }
            else {
                if (!ilms.isNullOrEmpty(cancelCallBack)) {
                    cancelCallBack();
                }
            }
        });
};

ilms.setOldValuesInContainerFileds = function ($container, flagPopulateOldValue) {
    if (flagPopulateOldValue) {
        $.each($($container.find('select')), function (key, element) {
            $(element).val($(element).data('oldvalue')).trigger('change');
        });
        $.each($($container.find('input')), function (key, element) {
            $(element).val($(element).data('oldvalue'));
        });
        $.each($($container.find('input[type=checkbox]')), function (key, element) {

            if ($(element).val() === 'True') {
                $(element).prop('checked', true);
            } else {
                $(element).prop('checked', false);
            }
        });
        $container.find('.btn_save_patient').html(`<span class="fal fa-pencil mr-1"></span>Update`);
    }
    else {
        $($container.find('input')).val('');
        $($container.find('select')).val(null).trigger('change');
        $container.find('.btn_save_patient').html(`<span class="fal fa-save mr-1"></span>Save`);
    }
};

ilms.checkIfContainerStateChangedAndPromptSave = function ($container, flagShowAlert) {
    let state = false;
    $.each($($container.find('input')), function (key, element) {
        if ($(element).attr('id') === 'PatientPhoneNumber1' || $(element).attr('id') === 'PatientMobileNumber' || $(element).attr('id') === 'RPMobile' || $(element).attr('id') === 'RPPhoneNumber') {
            if ($(element).data('oldvalue') !== $(element).val().replace(/\D/g, '')) {
                return state = true;
            }
        }
        else if ($(element).data('oldvalue') !== $(element).val()) {
            flipEmailVerificationButton(false);

            return state = true;
        }
    });

    $.each($($container.find('select')), function (key, element) {
        if ($(element).data('oldvalue') !== $(element).val()) {
            return state = true;
        }
    });
    $.each($($container.find('input[type=checkbox]')), function (key, element) {
        let oldvalue = $(element).data('oldvalue') === 'True' ? true : false;
        if (oldvalue !== $(element).is(':checked')) {
            return state = true;
        }
    });
    if (flagShowAlert && state) {
        ilms.showDecisionAlert("You have Pending changes!", "Do you want to save them?", "warning", 'Yes', function () {
            $($container).find('.btn_save_patient').trigger('click');
        }, function () {
            $($container).modal('hide');
        }, "No");
    } else {
        $($container).modal('hide');
    }
    return state;
};

ilms.cloneValuesByName = function ($source, $destination) {
    $.each($($source.find('select')), function (key, element) {
        $destination.find(`select[name *= ${$(element).attr('name')}]`).val($(element).val()).trigger('change');
    });
    $.each($($source.find('input')), function (key, element) {
        $destination.find(`input[name *= ${$(element).attr('name')}]`).val($(element).val());
    });
};

ilms.formatDate = function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
};

ilms.getTimeDiffInMinutes = function diff_minutes(dt2, dt1) {

    var diff = (dt2.getTime() - dt1.getTime()) / 1000;
    diff /= 60;
    return Math.abs(Math.round(diff));

};

ilms.formatPhoneNumber = function (phoneNumberString) {
    var cleaned = ('' + phoneNumberString).replace(/\D/g, '');
    var match = cleaned.match(/^(1|)?(\d{3})(\d{3})(\d{4})$/);
    if (match) {
        var intlCode = (match[1] ? '+1 ' : '');
        return [intlCode, '(', match[2], ') ', match[3], '-', match[4]].join('');
    }
    return null;
};

ilms.DateRangeFilter = {
    Today: 1,
    Yesterday: 2,
    Last2Days: 15,
    Last7Days: 3,
    Last14Days: 16,
    Last30Days: 4,
    Last60Days: 5,
    Last90Days: 6,
    MonthToDate: 7,
    PreviousMonth: 8,
    CurrentQuarter: 9,
    PreviousQuarter: 10,
    YearToDate: 11,
    PreviousYear: 12,
    CustomDateRange: 13,
    All: 14
};

ilms.GetSpinnerHtml = function (spinnerType, spinnerColor) {
    return `<div class="spinner-${spinnerType} text-${spinnerColor}" role="status"></div>`;
};

ilms.SpinnerType = {
    Border: "border",
    Grow: "grow",
    CubeBorder: "border rounded-0",
    GrowCube: "grow rounded-0"
};

ilms.ColorClass = {
    Success: "success",
    Secondary: "secondary",
    Primary: "primary",
    Danger: "danger",
    Warning: "warning",
    Info: "info",
    Light: "light",
    Dark: "dark"
};

ilms.ShowSpinnerToElement = function (element, spinnerType, spinnerColor) {
    let buttonPrevHtml = $(element)[0].innerHTML;
    $(element)[0].innerHTML = ilms.GetSpinnerHtml(spinnerType, spinnerColor);
    return buttonPrevHtml;
};

ilms.RemoveSpinnerFromElementAnAppendOrignalHtml = function (element, prevHtml) {
    $(element)[0].innerHTML = prevHtml;
};

ilms.GetProgressBarHtml = function (className) {
    return `<div class="fs-xl color-danger-300 mb-1"><i class="fal fa-cog fa-spin mr-1"></i>Loading please wait...</div>
            <div class="progress">
                <div class="progress-bar progress-bar-striped bg-${className}-500 progress-bar-animated" role="progressbar" style="width: 10%" aria-valuenow="80" aria-valuemin="0" aria-valuemax="100"></div>
            </div>`;
};

ilms.validateForm = function ($form) {
    $form.addClass('was-validated');
    return $form[0].checkValidity();
};

ilms.resetFormForAdd = function ($form) {
    $form.removeClass('was-validated');
    $form.find('input').val('');
    $form.find('textarea').val('');
    $form.find('select').val('').trigger('change');
};

ilms.toggleSwitch = function (_that, flag, dataAttr = 'isactive') {
    $(_that).prop('checked', flag);
    $(_that).data(dataAttr, flag);
};
ilms.getFormDataWithSerializedArray = function (formID) {
    var form = $('#' + formID);
    formData = new FormData()
    formParams = form.serializeArray();
    $.each(form.find('input[type="file"]'), function (i, tag) {
        $.each($(tag)[0].files, function (i, file) {
            formData.append(tag.name, file);
        });
    });
    $.each(formParams, function (i, val) {
        formData.append(val.name, val.value);
    });
    return formData;
}