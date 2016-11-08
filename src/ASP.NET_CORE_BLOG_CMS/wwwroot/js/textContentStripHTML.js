$(document).ready(function () {

  
    $("#postsInfoTable .textcontent").each(function () {
       
       var html = $(this).text().toString();  
       var div = document.createElement("div");
       div.innerHTML = html;
       var text = div.textContent || div.innerText || "";
       $(this).text(text);
    }); 
}); 
