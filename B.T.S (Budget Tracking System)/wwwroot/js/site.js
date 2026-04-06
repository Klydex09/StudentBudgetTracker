// Runs after the page has finished loading.
document.addEventListener("DOMContentLoaded", function () {
    // Gets the mobile menu button and sidebar for responsive navigation.
    const menuToggle = document.getElementById("menuToggle");
    const sidebar = document.querySelector("aside");
    const navLinks = document.querySelectorAll("aside nav a");

    // Adds open/close behavior to the mobile sidebar menu.
    if (menuToggle && sidebar) {
        menuToggle.addEventListener("click", function () {
            sidebar.classList.toggle("open");
        });

        // Closes the sidebar after a navigation link is clicked.
        navLinks.forEach(function (link) {
            link.addEventListener("click", function () {
                sidebar.classList.remove("open");
            });
        });
    }

    // Finds buttons that show or hide optional UI sections such as date range fields.
    const toggleButtons = document.querySelectorAll("[data-toggle-target]");
    toggleButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            const targetId = button.getAttribute("data-toggle-target");
            const target = document.getElementById(targetId);

            if (!target) {
                return;
            }

            // Shows or hides the target area.
            target.classList.toggle("hidden");

            // Updates the accessibility state of the button.
            const isExpanded = !target.classList.contains("hidden");
            button.setAttribute("aria-expanded", isExpanded.toString());

            // Changes the button label depending on whether the target is visible.
            const labels = (button.getAttribute("data-toggle-text") || "").split("|");
            const buttonLabel = button.querySelector(".icon-toggle-label");

            if (buttonLabel && labels.length === 2) {
                buttonLabel.textContent = isExpanded ? labels[1] : labels[0];
            }
        });
    });

    // Lets the user expand or collapse long remarks text in the records table.
    const remarksButtons = document.querySelectorAll(".remarks-toggle");
    remarksButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            button.classList.toggle("expanded");
        });
    });

    // Shows a custom text field only when the user selects "Other" as the category.
    const categorySelects = document.querySelectorAll(".category-select");
    categorySelects.forEach(function (select) {
        const targetId = select.getAttribute("data-custom-target");
        const target = document.getElementById(targetId);

        if (!target) {
            return;
        }

        const updateCustomCategory = function () {
            const showCustomField = select.value === "Other";
            target.classList.toggle("hidden", !showCustomField);

            const input = target.querySelector("input");
            if (input) {
                // Makes the custom category required only when the field is visible.
                input.required = showCustomField;
            }
        };

        // Runs once on load and again whenever the category changes.
        updateCustomCategory();
        select.addEventListener("change", updateCustomCategory);
    });
});
