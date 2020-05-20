var RbatApp = RbatApp || {};

(function (namespace) {
    namespace.saveRecord = function (formName, destination) {        
        $.ajax({
            type: "POST",
            dataType: "json",
            url: destination,
            data: $(formName).serialize(),
            success: function (response) {
                if (true === response.success) {
                    table.draw();
                    document.getElementById("close").click();
                } else {
                    $('input').removeClass("border-danger");

                    if (response.errors) {
                        console.log(" - got errors");
                        for (let error of response.errors) {
                            const key = error.key;
                            let errorList = "";
                            for (let errorDetail of error.errors) {
                                if (errorList.length > 0) {
                                    errorList += ", ";
                                }
                                errorList += errorDetail;
                            }

                            showValidationErrorAsToolTip(key, errorList);
                            console.log(`   - key = '${key}', error list = '${errorList}'`);
                        }
                    }

                    if (response.errorMessage) {
                        alert(response.errorMessage);
                    }
                }

            }
        });
    };

    function showValidationErrorAsToolTip(key, errorList) {
        // Add the tooltip
        $(`#${key}`)
            .addClass("border-danger")
            .tooltip({
                title: errorList
            });
        //Try to find element by name if Id doesn't exist
        if (!$(`#${key}`).length) {
            $("[name='" + key + "']")
                .addClass("border-danger")
                .tooltip({
                    title: errorList
                });
        }
    }

})(RbatApp.ModalForm = RbatApp.ModalForm || {});

    