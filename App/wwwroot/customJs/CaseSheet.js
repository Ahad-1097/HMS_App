function getCaseSheetFromModel() {
    var models = [];

    var model = {};

    model.PatientID = $("#CaseSheet_PatientID").val(); // this is for edit
    if (model.PatientID == "") {
        model.PatientID = $("#PatientModel_PatientID").val(); //this id for create after patient is created
    }else if (model.PatientID == "") {
        model.PatientID = 0;
    }
    model.Id = $("#CaseSheet_Id").val();
    if (model.Id == "") {
        model.Id = 0;
    }
    model.PRESENTING_COMPLAINTS = $("#CaseSheet_PRESENTING_COMPLAINTS").val();
    model.HistoryOfPresentingIllness = $("#CaseSheet_HistoryOfPresentingIllness").val();
    model.PastHistory = $("#CaseSheet_PastHistory").val();
    model.PersonalHistory = $("#CaseSheet_PersonalHistory").val();
    model.Diet = $("#CaseSheet_Diet").val();
    model.Appetite = $("#CaseSheet_Appetite").val();
    model.Sleep = $("#CaseSheet_Sleep").val();
    model.Bowel = $("#CaseSheet_Bowel").val();
    model.Bladder = $("#CaseSheet_Bladder").val();
    model.Addiction = $("#CaseSheet_Addiction").val();
    model.FamilyHistory = $("#CaseSheet_FamilyHistory").val();
   //Vitals
    model.BP = $("#CaseSheet_BP").val();
    model.PR = $("#CaseSheet_PR").val();
    model.RR = $("#CaseSheet_RR").val();
    model.Temp = $("#CaseSheet_Temp").val();
    model.SpO2 = $("#CaseSheet_SpO2").val();

    //GENERAL EXAMINATION
    model.Pallor = $("#CaseSheet_Pallor").val();
    model.Icterus = $("#CaseSheet_Icterus").val();
    model.Cyanosis = $("#CaseSheet_Cyanosis").val();
    model.Clubbing = $("#CaseSheet_Clubbing").val();
    model.Edema = $("#CaseSheet_Edema").val();
    model.Lymphadenopathy = $("#CaseSheet_Lymphadenopathy").val();

    //SYSTEMIC EXAMINATION
    model.RespiratorySystem = $("#CaseSheet_RespiratorySystem").val();
    model.CNS = $("#CaseSheet_CNS").val();
    model.CVS = $("#CaseSheet_CVS").val();
    model.PerAbdomen = $("#CaseSheet_PerAbdomen").val();
    model.LocoregionalExam = $("#CaseSheet_LocoregionalExam").val();
    
    models.push(model);
     $("#CaseSheetModelsJson").val(JSON.stringify(models));
    return true;
}

$("#CaseSheetBtn").click(function (event) {
    getCaseSheetFromModel();
    event.preventDefault();
    // var formData = $(this).serialize();
    var modelsJson = $("#CaseSheetModelsJson").val();
    // Submit the form using AJAX
    $.ajax({
        type: "POST",
        url: "/Patient/CaseSheet",
        data: {
            model: modelsJson
        },
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",

        success: function (msg) {
            $("#CaseSheetMessage").text(msg).show();
            $("#CaseSheetBtn").val("Successfull").prop("disabled", true);
        },
        error: function (xhr, status, error) {
            // Handle the error response
        }
    });

    $('#AddCaseSheetForm input[type=text]').each(function () {
        $(this).val('');
    });
    $('#AddCaseSheetForm textarea').each(function () {
        $(this).val('');
    });
});