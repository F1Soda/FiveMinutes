function toggleDropdown() {
    const dropdownMenu = document.querySelector('.admin-debug-dropdown .dropdown-menu');
    dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
}

// Close the dropdown when clicking outside of it
window.addEventListener('click', function (e) {
    const dropdown = document.querySelector('.admin-debug-dropdown');
    const dropdownMenu = dropdown.querySelector('.dropdown-menu');
    if (!dropdown.contains(e.target)) {
        dropdownMenu.style.display = 'none';
    }
});
