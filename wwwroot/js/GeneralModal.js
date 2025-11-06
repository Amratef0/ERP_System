function openModalWithAjax(url, title = "Details") {
    const modal = new bootstrap.Modal(document.getElementById('generalModal'));
    document.getElementById('generalModalLabel').innerText = title;
    document.getElementById('generalModalBody').innerHTML = `
            <div class="text-center">
                <div class="spinner-border" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </div>`;

    fetch(url, { headers: { "X-Requested-With": "XMLHttpRequest" } })
        .then(response => response.text())
        .then(html => {
            document.getElementById('generalModalBody').innerHTML = html;
        })
        .catch(error => {
            document.getElementById('generalModalBody').innerHTML =
                `<div class="alert alert-danger">Error loading content: ${error}</div>`;
        });

    modal.show();
}