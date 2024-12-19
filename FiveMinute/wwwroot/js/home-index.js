
$(document).ready(function () {
    $(document).on('click', '#delete-template', function (e) { 
		e.preventDefault(); // Prevent the default behavior
        delete_template(this); // Pass the clicked button element to the function
        
    });

    $(document).on('click', '#delete-test', function (e) {
        e.preventDefault(); // Prevent the default behavior
        delete_test(this); // Pass the clicked button element to the function
        console.log("Delete test")
    });
});

function delete_template(button) {
    var templateId = $(button).data("id");

    $.ajax({
        url: deleteTemplateUrl,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ id: templateId }),
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                $("#FMTemplateTable").html(response.templatesHtml);
                $("#FMTestTable").html(response.testsHtml);
                $("#TestCardsRow").html(response.testCardsRowHtml);   
            } else {
                alert(response.reason);
            }
        },
        error: function () {
            alert("An error occurred while deleting the template.");
        }
    });
}

function delete_test(button) {
    var testId = $(button).data("id");

    $.ajax({
        url: deleteTestUrl,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ id: testId }),
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                // Update the content of both tabs
                $("#FMTemplateTable").html(response.templatesHtml);
                $("#FMTestTable").html(response.testsHtml);
                $("#TestCardsRow").html(response.testCardsRowHtml);
                console.log(response.testCardsRowHtml)
            } else {
                alert(response.reason);
            }
        },
        error: function () {
            alert("An error occurred while deleting the template.");
        }
    });
}