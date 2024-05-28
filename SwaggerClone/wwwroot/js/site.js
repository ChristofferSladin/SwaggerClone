// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.collapsible-content').forEach(function (content) {
        const id = content.id;
        const isVisible = localStorage.getItem(id) === "true";
        content.style.display = isVisible ? "block" : "none";
    });
});

function toggleCollapse(id) {
    var content = document.getElementById(id);
    var isVisible = content.style.display === "none" || content.style.display === "";
    content.style.display = isVisible ? "block" : "none";
    localStorage.setItem(id, isVisible);
}
