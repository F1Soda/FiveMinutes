function showPopup(message, type) {
	const popup = document.getElementById("popup");
	const popupMessage = popup.querySelector(".popup-message");

	// Set the popup text to the provided message
	popupMessage.innerText = message;

	// Apply different styles based on the type of popup
	switch (type) {
		case 'error':
			popup.style.backgroundColor = "#f44336"; // Red for errors
			break;
		case 'notification':
			popup.style.backgroundColor = "#4CAF50"; // Green for notifications
			break;
		case 'warning':
			popup.style.backgroundColor = "#ff9800"; // Orange for warnings
			break;
		default:
			popup.style.backgroundColor = "#333"; // Default color
	}

	// Show the popup
	popup.style.display = "block";

	// Automatically close the popup after 3 seconds (3000 ms)
	setTimeout(function () {
		popup.style.display = "none";
	}, 3000);
}
