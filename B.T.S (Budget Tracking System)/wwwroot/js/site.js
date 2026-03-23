document.addEventListener("DOMContentLoaded", function () {
    const menuToggle = document.getElementById("menuToggle");
    const sidebar = document.querySelector("aside");
    const navLinks = document.querySelectorAll("aside nav a");

    if (!menuToggle || !sidebar) {
        return;
    }

    menuToggle.addEventListener("click", function () {
        sidebar.classList.toggle("open");
    });

    navLinks.forEach(function (link) {
        link.addEventListener("click", function () {
            sidebar.classList.remove("open");
        });
    });
});
