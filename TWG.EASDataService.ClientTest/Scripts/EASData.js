$(document).ready(function () {
   
   
    $('select#SelectedMethod').change(function () {
        var selectedMethod = $(this).val();
        if (selectedMethod.indexOf("<") != -1) {            
            $('div#divMethodParam1').css("display", "block");
            $('input#MethodParam1').val("");
            $('span#spnMethodParam1').css("display", "block");
            if (selectedMethod.indexOf("<Date>") != -1) {
                $('span#spnMethodParam1').html("<b>Date (Format: yyyymmdd_hhmmss)</b>");
                $('input#MethodParam1').css("width", "150px");
            }
            else
            {
                $('span#spnMethodParam1').html("<b>ID </b>");
                $('input#MethodParam1').css("width", "80px");
            }            

        }
        else
        {
            $('div#divMethodParam1').css("display", "none");
            $('span#spnMethodParam1').css("display", "none");
        }

    });



    $('#btnSubmit').click(function (evt) {
        
       /* evt.preventDefault();
        var methodName = $("#MethodName").val();
        var instanceUrl = $("#SelectedInstance").val()+"/";
        
        var url = "/easdata/getservice";
        if (methodName.toLowerCase().indexOf("mediacontent") != -1)
        {
            url = "/easdata/mediacontent";
        }
        
       
        var $form = $('form');
        if ($form.valid()) {
            
            $("#spnEndPointUrl").html(instanceUrl + methodName + "<br/>&nbsp;");

            $.ajax({
                type: "POST",
                url: url,
                data: "instanceUrl=" + instanceUrl + "&methodName=" + methodName,
                beforeSend: function () {
                     $("#resultDiv").css('display', 'none');
                     $("#divLoading").css('display', 'table-cell');
                },
                success: function (data) {                   
                    $("#resultDiv").css('display', 'block');
                    $("#divLoading").css('display', 'none');
                    $('#resultDiv').html("");
                    if (methodName.toLowerCase().indexOf("mediacontent") != -1) {                                                
                        $('#resultDiv').html('<img src="data:' + data.itype + ';base64,' + data.idata + '" />');
                    }
                    else {
                        $('#resultDiv').jsonView(data);
                    }

                    
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("An error occured");
                }
            });
            return false;
            
        }
        else {
            return false;
        }
*/       
        
         evt.preventDefault();
         var instanceUrl = $("#SelectedInstance").val()+"/";
         var methodName = $("#SelectedMethod").val().toLowerCase();
         var methodParam = $('#MethodParam1').val().trim();

         if (methodName.indexOf("<") != -1) {
             methodName = methodName.replace("<id>", methodParam);
             methodName = methodName.replace("<date>", methodParam);
         }         
         var url = "/easdata/getservice";
                 
         if (  methodName.indexOf("mediacontent") != -1  &&  methodName.indexOf("modified") == -1   )
         {
             url = "/easdata/mediacontent";
         }        
               var $form = $('form');
               if ($form.valid()) {
                                      
                   $("#spnEndPointUrl").html(instanceUrl + methodName + "<br/>&nbsp;");
       
                   $.ajax({
                       type: "POST",
                       url: url,
                       data: "instanceUrl=" + instanceUrl + "&methodName=" + methodName,
                       beforeSend: function () {
                            $("#resultDiv").css('display', 'none');
                            $("#divLoading").css('display', 'table-cell');
                       },
                       success: function (data) {
                           
                           $("#resultDiv").css('display', 'block');
                           $("#divLoading").css('display', 'none');
                           $('#resultDiv').html("");
                           if (methodName.indexOf("mediacontent") != -1 && methodName.indexOf("modified") == -1) {
                               $('#resultDiv').html('<img src="data:' + data.itype + ';base64,' + data.idata + '" />');
                           }
                           else {
                               $('#resultDiv').jsonView(data);
                           }
       
                           
                       },
                       error: function (XMLHttpRequest, textStatus, errorThrown) {
                           $("#resultDiv").css('display', 'block');
                           $("#divLoading").css('display', 'none');
                           $('#resultDiv').html("!!!!!!!An error Occured");
                       }
                   });
                   return false;
                   
               }
               else {
                   return false;
               }
       
        
    });
    return false;
});