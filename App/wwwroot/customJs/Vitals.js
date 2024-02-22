﻿function getVItalFromModel() {
    var models = [];

    var model = {};
    model.CBC = $("#Progress_CBC").val();
    model.PatientID = $("#Progress_PatientID").val();
    if (model.PatientID == "") {
        model.PatientID = 0;
    }

    model.Id = $("#Progress_Id").val();
    if (model.Id == "") {
        model.Id = 0;
    }
    model.Date = $("#Progress_Date").val();
    model.Cc = $("#Progress_Cc").val();
    model.GeneralCondition = $("#Progress_GeneralCondition").val();
    model.PR = $("#Progress_PR").val();
    model.BP = $("#Progress_BP").val();
    model.RR = $("#Progress_RR").val();
    model.SpO2 = $("#Progress_SpO2").val();
    model.Temp = $("#Progress_Temp").val();
    model.GeneralExamination = $("#Progress_GeneralExamination").val();
    model.CNS = $("#Progress_CNS").val();
    model.CVS = $("#Progress_CVS").val();
    model.RS = $("#Progress_RS").val();
    model.PA = $("#Progress_PA").val();
    model.LocalExamination = $("#Progress_LocalExamination").val();
    model.Drains = $("#Progress_Drains").val();
    model.Urine = $("#Progress_Urine").val();
    model.Advice = $("#Progress_Advice").val();
    model.Remark = $("#Progress_Remark").val();

    models.push(model);

    var isAnyPropertyNull = false;
    $.each(model, function (key, value) {
        if ((value == '' || value == undefined || value == 0) && !isAnyPropertyNull) {
            isAnyPropertyNull = false;
        } else {

            isAnyPropertyNull = true;
            return isAnyPropertyNull;

        }
    });

    if (isAnyPropertyNull) {
        $("#VitalModelsJson").val(JSON.stringify(models));
        var modelsJson = $("#VitalModelsJson").val();

        // Submit the form using AJAX
        $.ajax({
            type: "POST",
            url: "/Patient/Vitals",
            data: {
                model: modelsJson
            },
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",

            success: function (msg) {
                $("#VitalSuccessMessage").text(msg).show();
                $("#VitalErrorMessage").hide();
                $("#VItalBtn").val("Successfull").prop("disabled", true);

            },
            error: function (xhr, status, error) {
                // Handle the error response
            }
        });

        $('#AddProgressForm input[type=text]').each(function () {
            $(this).val('');
        });
        $('#AddProgressForm textarea').each(function () {
            $(this).val('');
        });

    } else {
        $("#VitalErrorMessage").text("Please insert any value!!!").show();
    }

    
}
