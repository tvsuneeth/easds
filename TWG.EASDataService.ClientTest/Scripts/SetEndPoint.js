$(document).ready(function () {
       
    $('.InstanceLInk').click(function () {                       
        var urlstring = $(this).attr("href");        
        var url = urlstring.substr(6, urlstring.length - 5);

        $.ajax({
            type: "POST",
            url: "/endpoint/submit",
            data: "ServiceInstance.Url=" + url + "&ServiceInstance.Name=none",
            beforeSend: function () {
                $("#divLoading").css('display', 'block');
            },
            success: function () {                
                var url = window.location.protocol + "//" + window.location.host;               
                window.location = url;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown)
            {
                alert("An error occured");
            }
        });


        });    
    return false;
});