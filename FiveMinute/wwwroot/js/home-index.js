
$(document).ready(function () {
	$('#delete-template').click(function (e) {
		e.preventDefault(); // Prevent the default behavior
		delete_template(this); // Pass the clicked button element to the function
	});
});

function delete_template(button) {

	console.log("called!");
	var testId = $(button).data("id");

	console.log(testId);

	// if (confirm("Are you sure you want to delete this item?")) {
	// Perform the AJAX request to delete
	$.ajax({
		url: deleteTemplateUrl, // URL to the Delete action
		type: 'POST',
		data: JSON.stringify({ id: testId }), // Send the ID of the item to delete
		contentType: 'application/json; charset=utf-8',
		dataType: 'json',
		success: function (response) {
			if (response.success) {
				// Remove the row from the table if deletion is successful
				$("#test-" + testId).remove();
			} else {
				alert(response.reason);
			}
		},
		error: function (response) {
			alert(response.reason);
		}
	});

}