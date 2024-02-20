function getDiagnosisFromModel() {
    var models = [];
    var model = {};
    model.Daignosis = $("#PatientModel_Daignosis").val();
    model.Side = $("#PatientModel_Side").val();
    model.CoMorbity = $("#PatientModel_CoMorbity").val();
    model.PatientID = $("#PatientModel_PatientID").val();
    if (model.PatientID == "") {
        model.PatientID = 0;
    }

    models.push(model);
    $("#DiagnosisModelsJson").val(JSON.stringify(models));

    var modelsJson = $("#DiagnosisModelsJson").val();

    // Submit the form using AJAX
    $.ajax({
        type: "POST",
        url: "/Patient/Diagnosis",
        data: {
            model: modelsJson
        },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",

        success: function (msg) {
            $("#DiagnosisMessage").text(msg).show();
            $("#DiagnosisBtn").val("Successfull").prop("disabled", true);

        },
        error: function (xhr, status, error) {
            // Handle the error response
        }
    });

    $('#AddDiagnosisForm input[type=text]').each(function () {
        $(this).val('');
    });
}





