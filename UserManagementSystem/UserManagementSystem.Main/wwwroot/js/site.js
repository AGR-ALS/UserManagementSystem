document.addEventListener('DOMContentLoaded', function () {
    const selectAllCheckbox = document.getElementById('selectAll');
    const userCheckboxes = document.querySelectorAll('.user-checkbox');

    if (selectAllCheckbox) {
        selectAllCheckbox.addEventListener('change', function () {
            userCheckboxes.forEach(checkbox => {
                checkbox.checked = selectAllCheckbox.checked;
            });
        });

        userCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', function () {
                if (!checkbox.checked) {
                    selectAllCheckbox.checked = false;
                } else {
                    selectAllCheckbox.checked = Array.from(userCheckboxes).every(cb => cb.checked);
                }
            });
        });
    }
});