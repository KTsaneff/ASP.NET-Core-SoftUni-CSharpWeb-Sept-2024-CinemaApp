document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".view-details-btn").forEach(button => {
        button.addEventListener("click", function () {
            let movieId = this.getAttribute("data-movie-id");

            fetch(`/Movie/DetailsPartial?id=${movieId}`)
                .then(response => response.text())
                .then(html => {
                    document.getElementById("movieDetailsContent").innerHTML = html;
                    new bootstrap.Modal(document.getElementById("movieDetailsModal")).show();
                })
                .catch(error => console.error("Error loading movie details:", error));
        });
    });
});
