function getDischargeFromModel() {
    var models = [];

    var model = {};
    model.PatientID = $("#Patient_PatientID").val();
    if (model.PatientID == "") {
        model.PatientID = 0;
    }
    model.Id = $("#Discharge_Id").val();
    if (model.Id == "") {
        model.Id = 0;
    }
    model.DOA = $("#Discharge_DOA").val();
    model.DOD = $("#Discharge_DOD").val();
    model.Diagnosis = $("#Discharge_Diagnosis").val();
    model.CaseSummary = $("#Discharge_CaseSummary").val();
    model.Investigations = $("#Discharge_Investigations").val();
    model.TreatmentGiven = $("#Discharge_TreatmentGiven").val();
    model.AdviceOndischarge = $("#Discharge_AdviceOndischarge").val();
    model.SeniorResident = $("#Discharge_SeniorResident").val();

    models.push(model);
    $("#DischargeModelsJson").val(JSON.stringify(models));

    var modelsJson = $("#DischargeModelsJson").val();

    // Submit the form using AJAX
    $.ajax({
        type: "POST",
        url: "/Patient/DischargePost",
        data: {
            model: modelsJson
        },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",

        success: function (msg) {
                $("#DischargeSuccessMessage").text(msg).show();
                $("#DischargeBtn").val("Successfull").prop("disabled", true);
        },
        error: function (xhr, status, error) {
            // Handle the error response
        }
    });

    $('#AddDischargeForm input[type=text]').each(function () {
        $(this).val('');
    });
    $('#AddDischargeForm textarea').each(function () {
        $(this).val('');
    });
}



