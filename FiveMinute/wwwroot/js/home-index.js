


$(document).ready(function () {
	// Bind the delete button click event
	$("#delete-template").click(function () {

		console.log("called!");
		var testId = $(this).data("id");

		console.log(testId);

		// if (confirm("Are you sure you want to delete this item?")) {
		// Perform the AJAX request to delete
		$.ajax({
			url: '@Url.Action("Delete", "FiveMinuteTemplate")', // URL to the Delete action
			type: 'POST',
			// data: JSON.stringify({ id: testId }), // Send the ID of the item to delete
			//contentType: 'application/json; charset=utf-8',
			//dataType: 'json',
			data: { id: testId }
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
		// }
	});
});