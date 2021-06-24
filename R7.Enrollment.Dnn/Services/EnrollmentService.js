class EnrollmentService {

    constructor (moduleId) {
        this.moduleId = moduleId;
        this.baseServicepath = $.dnnSF (moduleId).getServiceRoot ("R7.Enrollment");
    }

    getUrl(controller, action, id) {
        return this.baseServicepath + controller + "/" + action + (id != null ? "/" + id : "");
    }

    ajaxJsonCall (type, url, data, success, fail) {
        $.ajax ({
            type: type,
            url: url,
            beforeSend: $.dnnSF (this.moduleId).setModuleHeaders,
            processData: false,
            contentType: "application/json",
            data: JSON.stringify (data)
        }).done ((retData) => {
            if (success != undefined) {
                success (retData);
            }
        }).fail ((xhr, status, err) => {
            if (fail != undefined) {
                fail (xhr, status, err);
            }
        });
    }

    getRatingLists (data, success, fail) {
        this.ajaxJsonCall ("POST", this.getUrl ("Enrollment", "SearchRatingLists", null), data, success, fail);
    }
}

window.EnrollmentService = EnrollmentService;
