
var dropdownButton = document.getElementById("dropdownButton")
var wrapOptions = document.getElementById("wrapOptions")

dropdownButton.addEventListener("click", () => {
    wrapOptions.classList.toggle("open")
})