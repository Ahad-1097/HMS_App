

//function AddPicture(id, imgId) {
//    $.ajax({
//        url: '/Patient/AddPicture', // Replace "ControllerName" with the actual name of your controller
//        type: 'GET',
//        data: { PatientID: id, ViewName: "Edit", ImgId: imgId },
//        success: function (result) {
//            $('#EditPicture').html(result); // Replace the content of the container with the loaded partial view
//        },
//        error: function (xhr, status, error) {
//            console.log(xhr.responseText); // Log any error that occurred during the AJAX request
//        }
//    });
//    if (imgId == "-1") {
//        $("#Imagebtn").val("Add Image");
//    } else {
//        $("#Imagebtn").val("Update");
//    }
//}
function AddMultipleImage() { 
    debugger
    var imgModel = {};
    imgModel.PatientID = $("#InvestigationImages_PatientId").val();

    if ($("#viewbegMethod").val() == "save")
    {
        imgModel.PatientID = $("#viewbegPatientId").val();
        imgModel.Msg = "save";
    }

    if (imgModel.PatientID == "") {
        imgModel.PatientID = 0;
    }

    imgModel.Id = $("#InvestigationImages_Id").val();
    if (imgModel.Id == "") {
        imgModel.Id = 0;
    }

    imgModel.InvestigationID = $("#InvestigationImages_InvestigationID").val();
    if (imgModel.InvestigationID == "") {
        imgModel.InvestigationID = 0;
    }
   
    imgModel.BloodSugar_img = $("#BloodSugar_img")[0].files;
    imgModel.TFT_img = $("#TFT_img")[0].files;
    imgModel.USG_img = $("#USG_img")[0].files;
    imgModel.SONOMMOGRAPHY_img = $("#SONOMMOGRAPHY_img")[0].files;
    imgModel.CECT_img = $("#CECT_img")[0].files;
    imgModel.MRI_img = $("#MRI_img")[0].files;
    imgModel.FNAC_img = $("#FNAC_img")[0].files;
    imgModel.TrucutBiopsy_img = $("#TrucutBiopsy_img")[0].files;
    imgModel.ReceptorStatus_img = $("#ReceptorStatus_img")[0].files;
    imgModel.MRCP_img = $("#MRCP_img")[0].files;
    imgModel.ERCP_img = $("#ERCP_img")[0].files;
    imgModel.EndoscopyUpperGI_img = $("#EndoscopyUpperGI_img")[0].files;
    imgModel.EndoscopyLowerGI_img = $("#EndoscopyLowerGI_img")[0].files;
    imgModel.PETCT_img = $("#PETCT_img")[0].files;
    imgModel.TumorMarkers_img = $("#TumorMarkers_img")[0].files;

    imgModel.IVP_img = $("#IVP_img")[0].files;
    imgModel.MCU_img = $("#MCU_img")[0].files;
    imgModel.RGU_img = $("#RGU_img")[0].files;

    imgModel.ABG_img = $("#ABG_img")[0].files;
    imgModel.OtherO = $("#OtherO")[0].files;
    imgModel.CBC_img = $("#CBC_img")[0].files;
    imgModel.RFT_img = $("#RFT_img")[0].files;
    imgModel.LFT_img = $("#LFT_img")[0].files;
    imgModel.PTINR_img = $("#PTINR_img")[0].files;
    imgModel.LIPIDPROFILE_img = $("#LIPIDPROFILE_img")[0].files;
    imgModel.UrineRM_img = $("#UrineRM_img")[0].files;

    var formData = new FormData();

    formData.append("PatientID", imgModel.PatientID);
    formData.append("Id", imgModel.Id);
    formData.append("InvestigationID", imgModel.InvestigationID);
    formData.append("BloodSugar_img", imgModel.BloodSugar_img);
    formData.append("TFT_img", imgModel.TFT_img);
    formData.append("USG_img", imgModel.USG_img);
    formData.append("SONOMMOGRAPHY_img", imgModel.SONOMMOGRAPHY_img);
    formData.append("CECT_img", imgModel.CECT_img);
    formData.append("MRI_img", imgModel.MRI_img);
    formData.append("FNAC_img", imgModel.FNAC_img);
    formData.append("TrucutBiopsy_img", imgModel.TrucutBiopsy_img);
    formData.append("ReceptorStatus_img", imgModel.ReceptorStatus_img);
    formData.append("MRCP_img", imgModel.MRCP_img);
    formData.append("ERCP_img", imgModel.ERCP_img);
    formData.append("EndoscopyUpperGI_img", imgModel.EndoscopyUpperGI_img);
    formData.append("EndoscopyLowerGI_img", imgModel.EndoscopyLowerGI_img);
    formData.append("PETCT_img", imgModel.PETCT_img);
    formData.append("TumorMarkers_img", imgModel.TumorMarkers_img);
   
    formData.append("IVP_img", imgModel.IVP_img);
    formData.append("MCU_img", imgModel.MCU_img);
    formData.append("RGU_img", imgModel.RGU_img);
    formData.append("Msg", imgModel.Msg);

    formData.append("ABG_img", imgModel.ABG_img);
    formData.append("OtherO", imgModel.OtherO);
    formData.append("CBC_img", imgModel.CBC_img);
    formData.append("RFT_img", imgModel.RFT_img);
    formData.append("LFT_img", imgModel.LFT_img);
    formData.append("PTINR_img", imgModel.PTINR_img);
    formData.append("LIPIDPROFILE_img", imgModel.LIPIDPROFILE_img);
    formData.append("UrineRM_img", imgModel.UrineRM_img);

    $.ajax({
        url: '/Patient/AddPicture',
        type: 'POST',
        data:  formData,
        processData: false,
        contentType: false,
        success: function (msg) {

            $("#ImageSuccessMessage").text(msg).show();
            ///$("#Imagebtn").prop("disabled", true);
        },
        error: function (xhr, status, error) {
        
            console.log('Error uploading image.');
        }
    });

    $('#investigation input[type=text]').each(function () {
        $(this).val('');
    });
};




