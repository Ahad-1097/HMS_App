function getOperationFromModel() {
    var formData = new FormData();
    debugger
    formData.append('PatientID', $("#Operation_PatientID").val() || 0);
    formData.append('Id', $("#Operation_Id").val() || 0);
    formData.append('Dr_ID', $("#Operation_Dr_ID").val());
    formData.append('Date', $("#Operation_Date").val());
    formData.append('Indication', $("#Operation_Indication").val());
    formData.append('Anaesthetist', $("#Operation_Anaesthetist").val());
    formData.append('OpertingSurgeon', $("#Operation_OpertingSurgeon").val());
    formData.append('Position', $("#Operation_Position").val());
    formData.append('Anaesthesia', $("#Operation_Anaesthesia").val());
    formData.append('PreoperativeDiagnosis', $("#Operation_PreoperativeDiagnosis").val())
    formData.append('OperationTitle', $("#Operation_OperationTitle").val());
    formData.append('Findings', $("#Operation_Findings").val());
    formData.append('Duration', $("#Operation_Duration").val());
    formData.append('StepsOfOperation', $("#Operation_StepsOfOperation").val());
    formData.append('Antibiotics', $("#Operation_Antibiotics").val());
    formData.append('SpecimensSentFor', $("#Operation_SpecimensSentFor").val());
    formData.append('PostOperativeInstructions', $("#Operation_PostOperativeInstructions").val());

    // Handle the image input
    var imageInput = document.getElementById('Operation_PerOPImage'); // Replace 'ImageInput' with the actual ID of your file input
    if (imageInput.files.length > 0) {
        formData.append('PerOPImageFile', imageInput.files[0]);
    }

    $.ajax({
        type: "POST",
        url: "/Patient/OperationSheet",
        data: formData,
        contentType: false,
        processData: false,
        success: function (msg) {
            $("#OperationSuccessMessage").text(msg).show();
            $("#OperationBtn").val("Successful").prop("disabled", true);
        },
        error: function (xhr, status, error) {
            // Handle the error response
        }
    });

    // Clear input fields after submission
    $('#AddOperationForm input[type=text], #AddOperationForm textarea').val('');
}
