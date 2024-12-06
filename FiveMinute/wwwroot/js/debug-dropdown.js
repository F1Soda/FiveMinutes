function toggleDropdown() {
    const dropdownMenu = document.querySelector('.admin-debug-dropdown .dropdown-menu');
    dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
}

// Close the dropdown when clicking outside of it
window.addEventListener('click', function (e) {
    const dropdown = document.querySelector('.admin-debug-dropdown');

    // Check if the dropdown exists (it may not exist when the page is loading or dynamically created)
    if (dropdown) {
        const dropdownMenu = dropdown.querySelector('.dropdown-menu');

        // Ensure the dropdown menu exists and the click target is not inside the dropdown
        if (dropdownMenu && !dropdown.contains(e.target)) {
            dropdownMenu.style.display = 'none';  // Close the dropdown
        }
    }
});