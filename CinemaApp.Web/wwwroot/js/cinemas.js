document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchCinemas");
    const filterCity = document.getElementById("filterCity");
    const filterMovies = document.getElementById("filterMovies");
    const cinemaCards = document.querySelectorAll(".cinema-card");

    function filterCinemas() {
        const searchText = searchInput.value.toLowerCase();
        const selectedCity = filterCity.value;
        const showWithMovies = filterMovies.checked;

        cinemaCards.forEach(card => {
            const cinemaName = card.dataset.name.toLowerCase();
            const cinemaCity = card.dataset.city;
            const hasMovies = card.dataset.hasMovies === "true";

            const matchesSearch = cinemaName.includes(searchText);
            const matchesCity = selectedCity === "" || cinemaCity === selectedCity;
            const matchesMovies = !showWithMovies || hasMovies;

            if (matchesSearch && matchesCity && matchesMovies) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        });
    }

    searchInput.addEventListener("input", filterCinemas);
    filterCity.addEventListener("change", filterCinemas);
    filterMovies.addEventListener("change", filterCinemas);
});
