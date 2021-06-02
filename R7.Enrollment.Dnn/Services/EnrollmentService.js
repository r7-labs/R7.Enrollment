class EnrollmentService {
    
    constructor(moduleId) {
        this.moduleId = moduleId;
        this.baseServicepath = $.dnnSF(moduleId).getServiceRoot("R7.Enrollment");
    }

    getUrl(controller, action, id) {
        return this.baseServicepath + controller + "/" + action + (id != null ? "/" + id : "");
    }

    ajaxCall(type, url, data, processData, contentType, success, fail) {
        $.ajax({
            type: type,
            url: url,
            beforeSend: $.dnnSF(this.moduleId).setModuleHeaders,
            processData: processData,
            contentType: contentType,
            data: data
        }).done(function (retData) {
            if (success != undefined) {
                success(retData);
            }
        }).fail(function (xhr, status, err) {
            if (fail != undefined) {
                fail(xhr, status, err);
            }
        });
    }
    
    getRatingLists (data, success, fail) {
        this.ajaxCall ("POST", this.getUrl ("Enrollment", "GetRatingLists", null), data, false, false, success, fail);
    }
}

window.EnrollmentService = EnrollmentService;