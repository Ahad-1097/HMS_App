// Outcome.js
function getOutcomeFormModel() {
    debugger
    var model = {
        PatientID: ($("#Outcome_PatientID").val() !== "") ? $("#Outcome_PatientID").val() : 0,
        Id: ($("#Outcome_Id").val() !== "") ? $("#Outcome_Id").val() : 0,
        Date: $("#Outcome_Date").val(),
        Outcome: $("#Outcome_outcomeType").val()
    };

    $.ajax({
        url: '/Patient/Outcome',
        type: 'POST',
        data: {
            model: JSON.stringify(model)
        },
        success: function (response) {
            console.log(response);
            $("#OutcomeSuccessMessage").text(response).show();
        },
        error: function (error) {
            console.log(error);
            $("#OutcomeErrorMessage").text('Error saving outcome.').show();
        }
    });
}



