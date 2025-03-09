function filterCinemas() {
    let searchValue = document.getElementById("searchBar").value.toLowerCase();
    let cityValue = document.getElementById("cityFilter").value.toLowerCase();
    let cinemas = document.querySelectorAll(".cinema-card");

    cinemas.forEach(cinema => {
        let cinemaName = cinema.querySelector(".card-title").textContent.toLowerCase();
        let cinemaCity = cinema.dataset.city.toLowerCase();

        if (
            (searchValue === "" || cinemaName.includes(searchValue)) &&
            (cityValue === "" || cinemaCity === cityValue)
        ) {
            cinema.style.display = "block";
        } else {
            cinema.style.display = "none";
        }
    });
}
