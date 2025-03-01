document.addEventListener("DOMContentLoaded", function () {
    window.confirmToggleDelete = function (cinemaId, action) {
        Swal.fire({
            title: "Are you sure?",
            text: "Do you really want to " + action + " this cinema?",
            icon: action === "delete" ? "warning" : "info",
            showCancelButton: true,
            confirmButtonColor: action === "delete" ? "#d33" : "#ffc107",
            cancelButtonColor: "#6c757d",
            confirmButtonText: "Yes, " + action.charAt(0).toUpperCase() + action.slice(1),
            cancelButtonText: "Cancel"
        }).then((result) => {
            if (result.isConfirmed) {
                document.getElementById("toggleDeleteForm-" + cinemaId).submit();
            }
        });
    };
});
