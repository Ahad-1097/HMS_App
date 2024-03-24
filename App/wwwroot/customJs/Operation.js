function getOperationFromModel() {
    var formData = new FormData();
    debugger
    formData.append('PatientID', $("#Operation_PatientID").val() || $("#PatientModel_PatientID").val() || 0);
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
    var imageInput1 = document.getElementById('Operation_PerOPImage1'); // Replace 'ImageInput' with the actual ID of your file input
    if (imageInput1.files.length > 0) {
        formData.append('PerOPImageFile1', imageInput1.files[0]);
    }
    var imageInput2 = document.getElementById('Operation_PerOPImage2'); // Replace 'ImageInput' with the actual ID of your file input
    if (imageInput2.files.length > 0) {
        formData.append('PerOPImageFile2', imageInput2.files[0]);
    }
    var imageInput3 = document.getElementById('Operation_PerOPImage3'); // Replace 'ImageInput' with the actual ID of your file input
    if (imageInput3.files.length > 0) {
        formData.append('PerOPImageFile3', imageInput3.files[0]);
    }
    var imageInput4 = document.getElementById('Operation_PerOPImage4'); // Replace 'ImageInput' with the actual ID of your file input
    if (imageInput4.files.length > 0) {
        formData.append('PerOPImageFile4', imageInput4.files[0]);
    }
    var imageInput5 = document.getElementById('Operation_PerOPImage5'); // Replace 'ImageInput' with the actual ID of your file input
    if (imageInput5.files.length > 0) {
        formData.append('PerOPImageFile5', imageInput5.files[0]);
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
