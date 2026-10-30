document.addEventListener("dragover", function (event) {
    event.preventDefault();
});

document.addEventListener("drop", function (event) {
    var zone = event.target.closest && event.target.closest(".dropzone");
    event.preventDefault();
    if (!zone || !event.dataTransfer || !event.dataTransfer.files.length) {
        return;
    }

    var input = zone.querySelector("input[type=file]");
    if (!input) {
        return;
    }

    input.files = event.dataTransfer.files;
    input.dispatchEvent(new Event("change", { bubbles: true }));
});
