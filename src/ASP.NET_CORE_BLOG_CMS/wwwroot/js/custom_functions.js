function textContentStripHTML() {
    $("#postsInfoTable .textcontent").each(function () {

        var html = $(this).text().toString();
        var div = document.createElement("div");
        div.innerHTML = html;
        var text = div.textContent || div.innerText || "";
        $(this).text(text);
    });

}

function addNewCategory() {
    $("#addCategory").on("click", function () {
        var categoryname = $("#newCategory").val();
        $("#Category_Name").append('<option value="' + categoryname + '" selected="true">' + categoryname + '</option>');
    });
}



/*
get first element containning children
count them or do foreach
for each children check if it has children
if it has children check for another children
 
*/


var cssFileData = { cssString : "", postTitle: "" };






function generateCssFile(obj){


    $("form input[type='submit']").click(function (e)
    {
        e.preventDefault();
       
        selectAllChildren(obj);
        createCssFileName();
        $("#Post_textContent").val($("#editor").html());

        $.ajax({
            type: "POST",
            url: "/Posts/generateCssFile",
            data: { newstr: cssFileData },

            dataType: "json",
            success: function () {
                $("#Post_cssFileUrl").val(cssFileData.postTitle);

                $("form").submit();
               
               
            }

        });
       
    });

   
   
}

function selectAllChildren(obj) {                                             
    
    if ($(obj).children().length > 0)
    {
       
        $(obj).children().each(function () {
            var root = $(this);
            var attr = root.attr("style");
            if (typeof attr !== typeof undefined && attr !== false) {
                cssFileData.cssString += "#editor " + $(obj).prop("nodeName").toLowerCase() + " " + root.prop("nodeName").toLowerCase() + " { " + $(root).attr("style") + " } ";
                root.removeAttr("style");
            }
            //console.log($(obj).prop("nodeName").toLowerCase() + " has children:" + root.prop("nodeName").toLowerCase());
          
            return selectAllChildren(root);
        });
        
    }
}


function createCssFileName()
{
    var title = $("#Post_Title").val().replace(/\s+/g, '-').toLowerCase() + ".css";
    cssFileData.postTitle = title;
    
}



    

    


