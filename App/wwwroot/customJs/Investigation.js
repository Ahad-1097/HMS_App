

$("#AddInvestigationBtn").click(function (event) {
    var models = [];
    var model = {};

    model.Day = $("#InvestigationModel_Day").val();
    //CBC
    model.HB = $("#InvestigationModel_HB").val();
    model.TLC = $("#InvestigationModel_TLC").val();
    model.PLT = $("#InvestigationModel_PLT").val();

    //RFT
    model.SGeat = $("#InvestigationModel_SGeat").val();
    model.BUN = $("#InvestigationModel_BUN").val();

    //BLOOD SUGAR
    model.Fasting = $("#InvestigationModel_Fasting").val();
    model.PP = $("#InvestigationModel_PP").val();
    model.Random = $("#InvestigationModel_Random").val();

    //LFT
    model.TotalBil = $("#InvestigationModel_TotalBil").val();
    model.DirectBil = $("#InvestigationModel_DirectBil").val();
    model.AlkPhosphate = $("#InvestigationModel_AlkPhosphate").val();
    model.SGDT = $("#InvestigationModel_SGDT").val();
    model.SGPT = $("#InvestigationModel_SGPT").val();

    //TFT
    model.T3 = $("#InvestigationModel_T3").val();
    model.T4 = $("#InvestigationModel_T4").val();
    model.TSH = $("#InvestigationModel_TSH").val();
    model.FT3 = $("#InvestigationModel_FT3").val();
    model.FT4 = $("#InvestigationModel_FT4").val();


    //SERUM ELECTROLYTES
    model.Sodium = $("#InvestigationModel_Sodium").val();
    model.Potassium = $("#InvestigationModel_Potassium").val();
    model.Calcium = $("#InvestigationModel_Calcium").val();

    //PT-INR IMG
    model.PT = $("#InvestigationModel_PT").val();
    model.INR = $("#InvestigationModel_INR").val();

    //LIPID PROFILE
    model.Cholesterol = $("#InvestigationModel_Cholesterol").val();
    model.Triglyceride = $("#InvestigationModel_Triglyceride").val();
    model.HDL = $("#InvestigationModel_HDL").val();
    model.LDL = $("#InvestigationModel_LDL").val();

    //URINE RM IMG
    model.Blood = $("#InvestigationModel_Blood").val();
    model.PusCell = $("#InvestigationModel_PusCell").val();
    model.EpithelialCell = $("#InvestigationModel_EpithelialCell").val();
    model.Crystals = $("#InvestigationModel_Crystals").val();
    model.Sugar = $("#InvestigationModel_Sugar").val();
    model.Color = $("#InvestigationModel_Color").val();
    model.Appearance = $("#InvestigationModel_Appearance").val();
    model.Albumin = $("#InvestigationModel_Albumin").val();

    model.ABG = $("#InvestigationModel_ABG").val();

    model.USG = $("#InvestigationModel_USG").val();
    model.SONOMMOGRAPHY = $("#InvestigationModel_SONOMMOGRAPHY").val();
    model.CECT = $("#InvestigationModel_CECT").val();
    model.MRI = $("#InvestigationModel_MRI").val();
    model.FNAC = $("#InvestigationModel_FNAC").val();
    model.TrucutBiopsy = $("#InvestigationModel_TrucutBiopsy").val();
    model.ReceptorStatus = $("#InvestigationModel_ReceptorStatus").val();
    model.MRCP = $("#InvestigationModel_MRCP").val();
    model.ERCP = $("#InvestigationModel_ERCP").val();
    model.EndoscopyUpperGI = $("#InvestigationModel_EndoscopyUpperGI").val();
    model.EndoscopyLowerGI = $("#InvestigationModel_EndoscopyLowerGI").val();
    model.PETCT = $("#InvestigationModel_PETCT").val();
    model.TumorMarkers = $("#InvestigationModel_TumorMarkers").val();
    model.IVP = $("#InvestigationModel_IVP").val();
    model.MCU = $("#InvestigationModel_MCU").val();
    model.RGU = $("#InvestigationModel_RGU").val();
    model.OtherO = $("#InvestigationModel_OtherO").val();
    model.OtherT = $("#InvestigationModel_OtherT").val(); // Serum Amylase 
    model.OtherTh = $("#InvestigationModel_OtherTh").val(); // Serum lipase

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
        $("#ModelsJson").val(JSON.stringify(models));
        //event.preventDefault();
        // var formData = $(this).serialize();
        var modelsJson = $("#ModelsJson").val();

        // Submit the form using AJAX
        $.ajax({
            type: "POST",
            url: "/Patient/AddInvestigation",
            data: {
                data: modelsJson
            },
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",

            success: function (result) {

                var tableData = JSON.parse(result);

                if (tableData != null) {
                    $("#InvestigationsuccessMessage").text("Data added successfully").show();
                    $("#InvestigationerrorMessage").hide();
                    $("#AddInvestigation").val("Add more").prop("disabled", false);

                } else {
                    $("#InvestigationerrorMessage").text("something went wrong").show();
                }

                $("#example5 tbody").empty();

                // Loop through the data and add a row for each item
                $.each(tableData, function (index, item) {
                    $("#InvestigationModel_PatientId").val(item.PatientID)
                    var row = $("<tr></tr>");
                    row.append($("<td></td>").text(item.Day));
                    row.append($("<td></td>").text(item.HB));  //CBC
                    row.append($("<td></td>").text(item.SGeat)); //RFT
                    row.append($("<td></td>").text(item.Fasting));//BloodSugar
                    row.append($("<td></td>").text(item.TotalBil));//LFT
                    row.append($("<td></td>").text(item.T3)); //TFT
                    row.append($("<td></td>").html("<a id='EditDay' onclick='OnDeleteInvestigation(" + item.Id + ")' class='btn btn-primary shadow btn-xs sharp mr-1'><i class='fa fa-trash'></i></a>"));
                    $("#example5 tbody").append(row);
                });
            },
            error: function (xhr, status, error) {
                // Handle the error response
            }
        });

        $('#Investigationform input[type=text]').each(function () {
            $(this).val('');
        });
    } else {
        $("#InvestigationerrorMessage").text("Please insert any value!!!").show();
    }
});


function EditInvestigationModels(_req) {
    var models = [];
    var model = {};
    model.Id = $("#InvestigationList_0__Id").val();
    model.PatientID = $("#InvestigationList_0__PatientID").val();
    if (model.PatientID == "") {
        model.PatientID = 0;
    }

    if (model.Id == "" || _req =='New') {
        model.Id = 0;
    }

    model.Day = $("#InvestigationList_0__Day").val();
    //CBC
    model.HB = $("#InvestigationList_0__HB").val();
    model.TLC = $("#InvestigationList_0__TLC").val();
    model.PLT = $("#InvestigationList_0__PLT").val();

    //RFT
    model.SGeat = $("#InvestigationList_0__SGeat").val();
    model.BUN = $("#InvestigationList_0__BUN").val();

    //BLOOD SUGAR
    model.Fasting = $("#InvestigationList_0__Fasting").val();
    model.PP = $("#InvestigationList_0__PP").val();
    model.Random = $("#InvestigationList_0__Random").val();

    //LFT
    model.TotalBil = $("#InvestigationList_0__TotalBil").val();
    model.DirectBil = $("#InvestigationList_0__DirectBil").val();
    model.AlkPhosphate = $("#InvestigationList_0__AlkPhosphate").val();
    model.SGDT = $("#InvestigationList_0__SGDT").val();
    model.SGPT = $("#InvestigationList_0__SGPT").val();

    //TFT
    model.T3 = $("#InvestigationList_0__T3").val();
    model.T4 = $("#InvestigationList_0__T4").val();
    model.TSH = $("#InvestigationList_0__TSH").val();
    model.FT3 = $("#InvestigationList_0__FT3").val();
    model.FT4 = $("#InvestigationList_0__FT4").val();


    //SERUM ELECTROLYTES
    model.Sodium = $("#InvestigationList_0__Sodium").val();
    model.Potassium = $("#InvestigationList_0__Potassium").val();
    model.Calcium = $("#InvestigationList_0__Calcium").val();

    //PT-INR IMG
    model.PT = $("#InvestigationList_0__PT").val();
    model.INR = $("#InvestigationList_0__INR").val();

    //LIPID PROFILE
    model.Cholesterol = $("#InvestigationList_0__Cholesterol").val();
    model.Triglyceride = $("#InvestigationList_0__Triglyceride").val();
    model.HDL = $("#InvestigationList_0__HDL").val();
    model.LDL = $("#InvestigationList_0__LDL").val();

    //URINE RM IMG
    model.Blood = $("#InvestigationList_0__Blood").val();
    model.PusCell = $("#InvestigationList_0__PusCell").val();
    model.EpithelialCell = $("#InvestigationList_0__EpithelialCell").val();
    model.Crystals = $("#InvestigationList_0__Crystals").val();
    model.Sugar = $("#InvestigationList_0__Sugar").val();
    model.Color = $("#InvestigationList_0__Color").val();
    model.Appearance = $("#InvestigationList_0__Appearance").val();
    model.Albumin = $("#InvestigationList_0__Albumin").val();

    model.ABG = $("#InvestigationList_0__ABG").val();

    model.USG = $("#InvestigationList_0__USG").val();
    model.SONOMMOGRAPHY = $("#InvestigationList_0__SONOMMOGRAPHY").val();
    model.CECT = $("#InvestigationList_0__CECT").val();
    model.MRI = $("#InvestigationList_0__MRI").val();
    model.FNAC = $("#InvestigationList_0__FNAC").val();
    model.TrucutBiopsy = $("#InvestigationList_0__TrucutBiopsy").val();
    model.ReceptorStatus = $("#InvestigationList_0__ReceptorStatus").val();
    model.MRCP = $("#InvestigationList_0__MRCP").val();
    model.ERCP = $("#InvestigationList_0__ERCP").val();
    model.EndoscopyUpperGI = $("#InvestigationList_0__EndoscopyUpperGI").val();
    model.EndoscopyLowerGI = $("#InvestigationList_0__EndoscopyLowerGI").val();
    model.PETCT = $("#InvestigationList_0__PETCT").val();
    model.TumorMarkers = $("#InvestigationList_0__TumorMarkers").val();
    model.IVP = $("#InvestigationList_0__IVP").val();
    model.MCU = $("#InvestigationList_0__MCU").val();
    model.RGU = $("#InvestigationList_0__RGU").val();
    model.OtherO = $("#InvestigationList_0__OtherO").val();
    model.OtherT = $("#InvestigationList_0__OtherT").val(); // Serum Amylase 
    model.OtherTh = $("#InvestigationList_0__OtherTh").val(); // Serum lipase
    models.push(model);

    $("#InvestigationModelsJson").val(JSON.stringify(models));

    var modelsJson = $("#InvestigationModelsJson").val();

    // Submit the form using AJAX
    $.ajax({
        type: "POST",
        url: "/Patient/AddInvestigation",
        data: {
            data: modelsJson
        },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",

        success: function (result) {

            var tableData = JSON.parse(result);

            if (tableData != null) {
                $("#InvestigationsuccessMessage").text("Data updated successfully").show();

            } else {
                $("#InvestigationerrorMessage").text("something went wrong").show();
            }

            $("#myTable tbody").empty();

            // Loop through the data and add a row for each item
            $.each(tableData, function (index, item) {
                var row = $("<tr></tr>");
                row.append($("<td></td>").text(item.Day));
                row.append($("<td></td>").text(item.HB));  //CBC
                row.append($("<td></td>").text(item.SGeat)); //RFT
                row.append($("<td></td>").text(item.Fasting));//BloodSugar
                row.append($("<td></td>").text(item.TotalBil));//LFT
                row.append($("<td></td>").text(item.T3)); //TFT
                row.append($("<td></td>").html("<a id='EditDay' onclick='OnClickEdit(" + item.Id + ")' class='btn btn-primary shadow btn-xs sharp mr-1'><i class='fa fa-pencil'></i></a>"));
                $("#myTable tbody").append(row);
            });
        },
        error: function (xhr, status, error) {
            // Handle the error response
        }
    });

    $('#investigation input[type=text]').each(function () {
        $(this).val('');
    });
    $('#investigation input[type=date]').each(function () {
        $(this).val('');
    });

}

function OnDeleteInvestigation(Id) {
    var selectedValue = Id;
    var PatientID = $("#InvestigationModel_PatientId").val();
    if (PatientID == "") {
        PatientID = 0;
    }
    $.ajax({
        url: "/Patient/DeleteInvestigation",
        type: 'GET',
        data: { InvestigationId: Id, PatientID: PatientID },
        success: function (result) {
            var tableData = JSON.parse(result);
            $("#example5 tbody").empty();

            // Loop through the data and add a row for each item
            $.each(tableData, function (index, item) {
                var row = $("<tr></tr>");
                row.append($("<td></td>").text(item.Day));
                row.append($("<td></td>").text(item.HB));  //CBC
                row.append($("<td></td>").text(item.SGeat)); //RFT
                row.append($("<td></td>").text(item.Fasting));//BloodSugar
                row.append($("<td></td>").text(item.TotalBil));//LFT
                row.append($("<td></td>").text(item.T3)); //TFT
                row.append($("<td></td>").html("<a id='EditDay' onclick='OnDeleteInvestigation(" + item.Id + ")' class='btn btn-primary shadow btn-xs sharp mr-1'><i class='fa fa-trash'></i></a>"));
                $("#example5 tbody").append(row);
            });

        }
    });
}