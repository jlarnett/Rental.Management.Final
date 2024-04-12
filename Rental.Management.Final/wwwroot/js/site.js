// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


PropertyFiles.onchange = evt => {
    const [file] = PropertyFiles.files
    if (file) {
        PropertyPreviewImage.src = URL.createObjectURL(file);
    }
}