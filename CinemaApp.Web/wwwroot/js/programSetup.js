document.addEventListener("DOMContentLoaded", function () {
    var submitButton = document.getElementById("submitButton");
    var toggles = document.querySelectorAll(".toggle-switch");

    toggles.forEach(toggle => {
        toggle.addEventListener("change", function () {
            submitButton.disabled = false;
        });
    });
});
