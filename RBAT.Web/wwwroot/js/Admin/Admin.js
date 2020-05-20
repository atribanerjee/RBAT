(function () {
    function showAlert(alertID, text) {
        $(alertID).html(text);
        $(alertID).removeClass("in").show();
        $(alertID).delay(200).addClass("in").fadeOut(5000);
        $(alertID).show();
    }

    function showSuccessfullySaved(text) {
        showAlert("#savedAlert", text);
    }

    function showNotSaved(text) {
        showAlert("#badInputAlert", text);
    }

    $("#saveButton").click(function () {
        $('#overlay').removeClass("d-none").addClass("d-block");

        var users = [];
        $('tbody tr').each(function () {
            $(this).find('td input[type=checkbox]').each(function () {
                var id = $(this).data().userid;
                var user = users.find(x => x.Id === id);

                if (!user) {
                    user = {
                        Id: id,
                        IsSubscriber: false,
                        IsApplicationUser: false,
                        IsWebServiceUser: false
                    };
                }

                if (this.id === 'user_IsSubscriber') {
                    user.IsSubscriber = $(this).prop("checked");
                }

                if (this.id === 'user_IsApplicationUser') {

                    user.IsApplicationUser = $(this).prop("checked");
                }

                if (this.id === 'user_IsWebServiceUser') {
                    user.IsWebServiceUser = $(this).prop("checked");
                }

                if (!users.find(x => x.Id === id)) {
                    users.push(user);
                }
            });
        });

        $.ajax({
            url: "Admin/SaveUserRoles",
            data: { users },
            method: 'POST'
        }).done(function (result) {
            if (result.type === "Success") {
                $('#overlay').removeClass("d-block").addClass("d-none");
                showSuccessfullySaved("Changes saved");
            } else {
                $('#overlay').removeClass("d-block").addClass("d-none");
                showNotSaved("Error has occurred. " + result.message ? result.message : "");
            }
        });
    });
}());